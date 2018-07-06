
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
        private ESTATE m_State = ESTATE.NONE;


        [SerializeField] private FileRequestManager m_FileManager;

        [SerializeField] private UIController m_UI;
        public UIController UI
        {
            get { return m_UI; }
        }
        //[SerializeField] private List<TreasureHunterTrackableEvent> m_ListMarkers;

        // [Header("Egg prefabs")]
        [SerializeField] private List<GameObject> m_ListEggPrefabs;
        private GameObject m_CurrentInstance;

        //private TreasureData m_Data;
        private int m_MessageID;
        private int m_NumberCluesFound;        

        private void Awake()
        {
            if (m_FileManager == null)
            {
                m_FileManager = GetComponent<FileRequestManager>();
            }
        }

        void Start()
        {
            // Create data
            m_State = ESTATE.NONE;

            m_NumberCluesFound = 0;

            m_UI.HideAll();  

            // Check internet
            if (Application.internetReachability == NetworkReachability.NotReachable) 
            {
                Debug.Log("No Internet");
                m_UI.PopupButtons.ShowPopup("", "Please connect to internet and restart Treasur Hunt to download the data.",
                    "Restart App", OkPopup,
                    string.Empty, null,
                    string.Empty, null);

            }else
            {
                m_UI.Progress.Title = "Wait Downloading data...";
                m_UI.Progress.SetProgress(0);
                m_UI.Progress.Show();
                StartCoroutine(Init());
            }
        }

        private void OkPopup(Utility.ButtonWithText Button)
        {
            Application.Quit();
        }


        private IEnumerator Init()
        {
            // Request files
            yield return m_FileManager.RequestFiles();

            m_UI.Progress.SetProgress(100);

            yield return new WaitForSeconds(1.0f);

            m_UI.Progress.Title = "Completed! Ready to play =)";
            yield return new WaitForSeconds(1.0f);

            m_UI.Progress.Hide();


            // Intro 
            m_MessageID = 0;
            m_State = ESTATE.INTRO;

            m_UI.MessageUI.Show();
            m_UI.MessageUI.DisableButton();
            m_UI.MessageUI.SetMessage("Instructions", m_FileManager.Data.Intro[m_MessageID], 0.3f);
            m_UI.MessageUI.OnMessageEnd += OnEndOfMessage;
        }

        private void OnEndOfMessage()
        {
            m_UI.MessageUI.EnableButton();
        }

        public void OnMessageTap()
        {
            m_UI.MessageUI.DisableButton();
            switch (m_State)
            {
                case ESTATE.INTRO:

                    m_MessageID++;

                    if (m_MessageID < m_FileManager.Data.Intro.Count)
                    {
                        m_UI.MessageUI.SetMessage("Instructions", m_FileManager.Data.Intro[m_MessageID], 0.3f);
                    }else
                    {
                        // Show first clue
                        m_MessageID = 0;
                        m_State = ESTATE.GAME;
                        m_UI.MessageUI.SetMessage("Look for all the clues!", m_FileManager.Data.Clues[m_MessageID], 0.3f);
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
            if (m_State != ESTATE.GAME) return;
            // Find clue
            if ((markerID >= 0) && (markerID < m_FileManager.Data.Clues.Count))
            {
                //m_FileManager.Data.Clues[markerID]
                 m_NumberCluesFound++;
                if (m_NumberCluesFound < m_FileManager.Data.Clues.Count)
                {
                    // Instance egg and set new message
                    InstanceObject();
                    m_UI.MessageUI.SetMessage("Clue " + id, m_FileManager.Data.Clues[markerID], 0.3f);

                }else
                {
                    // End clues
                    m_MessageID = 0;
                    m_State = ESTATE.END;
                    m_UI.MessageUI.SetMessage("Game over", m_FileManager.Data.EndOfGame[m_MessageID], 0.3f);
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
