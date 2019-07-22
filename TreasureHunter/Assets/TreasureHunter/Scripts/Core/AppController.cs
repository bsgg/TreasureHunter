
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using UnityEngine.SceneManagement;

namespace TreasureHunt
{
    public class AppController : MonoBehaviour
    {
        #region Instance
        private static AppController m_Instance;
        public static AppController Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (AppController)FindObjectOfType(typeof(AppController));

                    if (m_Instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(AppController) + " is needed in the scene, but there is none.");
                    }
                }
                return m_Instance;
            }
        }
        #endregion Instance

        private enum ESTATE { NONE, INTRO, GAME, END };
        private ESTATE state = ESTATE.NONE;

        private FileRequestManager fileManager;

        [SerializeField] private UIController ui;
        public UIController UI
        {
            get { return ui; }
        }

        // [Header("Egg prefabs")]
        [SerializeField] private List<GameObject> m_ListEggPrefabs;
        private GameObject m_CurrentInstance;

        //private TreasureData m_Data;
        private int m_MessageID;
        private int m_NumberCluesFound;        

        private void Awake()
        {
            fileManager = GetComponent<FileRequestManager>();
        }

        void Start()
        {
            // Create data
            state = ESTATE.NONE;

            m_NumberCluesFound = 0;

            ui.HideAll();  

            // Check internet
            if (Application.internetReachability == NetworkReachability.NotReachable) 
            {
                Debug.Log("No Internet");
                ui.PopupButtons.ShowPopup("", "Please connect to internet and restart Treasur Hunt to download the data.",
                    "Restart App", OkPopup,
                    string.Empty, null,
                    string.Empty, null);

            }else
            {
                //m_UI.Progress.Title = "Wait Downloading data...";
                ui.Progress.SetProgress("Downloading\n0%",0);
                ui.Progress.Show();
                StartCoroutine(Init());
            }
        }

        private void OkPopup(ButtonWithText Button)
        {
            Application.Quit();
        }


        private IEnumerator Init()
        {

            // Request files
            yield return fileManager.RequestFiles();

            ui.Progress.SetProgress("Clues downloaded!", 100);

            yield return new WaitForSeconds(1.0f);

            // m_UI.Progress.Title = "Completed! Ready to play =)";
            ui.Progress.SetProgress("Ready to play =)", 100);

            yield return new WaitForSeconds(1.0f);

            ui.Progress.Hide();

            // Intro 
            m_MessageID = 0;
            state = ESTATE.INTRO;

            ui.MessageUI.Show();
            ui.MessageUI.DisableButton();
            ui.MessageUI.SetMessage("Instructions", fileManager.data.Intro[m_MessageID], 0.3f);
            ui.MessageUI.OnMessageEnd += OnEndOfMessage;
        }

        private void OnEndOfMessage()
        {
            ui.MessageUI.EnableButton();
        }

        public void OnMessageTap()
        {
            ui.MessageUI.DisableButton();
            switch (state)
            {
                case ESTATE.INTRO:

                    m_MessageID++;

                    if (m_MessageID < fileManager.data.Intro.Count)
                    {
                        ui.MessageUI.SetMessage("Instructions", fileManager.data.Intro[m_MessageID], 0.3f);
                    }else
                    {
                        // Show first clue
                        m_MessageID = 0;
                        state = ESTATE.GAME;
                        ui.MessageUI.SetMessage("Look for all the clues!", fileManager.data.Clues[m_MessageID], 0.3f);
                    }
                break;

                case ESTATE.GAME:

                break;

                case ESTATE.END:

                break;
            }
        }

        public void OnMarkerFound(string id, int markerID)
        {
            // Avoid scan when no Game state
            if (state != ESTATE.GAME) return;
            // Find clue
            if ((markerID >= 0) && (markerID < fileManager.data.Clues.Count))
            {
                //m_FileManager.Data.Clues[markerID]
                 m_NumberCluesFound++;
                if (m_NumberCluesFound < fileManager.data.Clues.Count)
                {
                    // Instance egg and set new message
                    InstanceObject();
                    ui.MessageUI.SetMessage("Clue " + id, fileManager.data.Clues[markerID], 0.3f);

                }else
                {
                    // End clues
                    m_MessageID = 0;
                    state = ESTATE.END;
                    ui.MessageUI.SetMessage("Game over", fileManager.data.EndOfGame[m_MessageID], 0.3f);
                }
            }
            
            
        }

        public void OnMarkerLost(string id, int markerID)
        {
            DestroyCurrentObject();
        }

       private void DestroyCurrentObject()
       {
           if (m_CurrentInstance != null)
           {
               Destroy(m_CurrentInstance);
           }
       }

       private void InstanceObject()
       {
            DestroyCurrentObject();
           int idObj = Random.Range(0, m_ListEggPrefabs.Count);
            m_CurrentInstance = Instantiate(m_ListEggPrefabs[idObj]);
       }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
