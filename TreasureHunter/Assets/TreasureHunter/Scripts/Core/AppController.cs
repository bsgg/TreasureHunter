using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunt
{
    [System.Serializable]
    public class TreasureData
    {
        public List<string> Intro;
        public List<string> Clues;
        public List<string> EndOfGame;

        public TreasureData()
        {
            Intro = new List<string>();
            Clues = new List<string>();
            EndOfGame = new List<string>();
        }
    }
    

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

        //public enum EMarkerType { NONE =-1, MARKER1 = 0, MARKER2, MARKER3, MARKER4, MARKER5, MARKER6, MARKER7, MARKER8, MARKER9, MARKER10 };

        private enum ESTATE { NONE, INTRO, GAME, END };
        private ESTATE m_State = ESTATE.NONE;

        [SerializeField] private TreasureControlUI m_UI;
        //[SerializeField] private List<TreasureHunterTrackableEvent> m_ListMarkers;

       // [Header("Egg prefabs")]
        [SerializeField] private List<GameObject> m_ListEggPrefabs;
        private GameObject m_CurrentEgg;

        private TreasureData m_Data;
        private int m_MessageID;

       private int m_NumberCluesFound;

        /*private string[] m_IntroMessage = new string[7]
            {
            "Hey! Welcome to this little game specially prepared for you =) \n (Tap to continue)",
            "First, allow me to explain how to play. \n (Tap to continue)",
            "You'll find little pictures hiding all over the place. \n (Tap to continue)",
            "Use your camera to scan them and you'll be able to get a ... clue! \n (Tap to continue)",
            "Think hard to find out the next location.... A surprise is waiting for you! \n (Tap to continue)",
            "Without any delay... Let's begin the game... \n (Tap to continue)",
            "Ah! I almost forgot.... here is the first clue: \n (Tap to continue)"
            };*/

        /*private string[] m_EndHuntMessage = new string[2]
           {
            "Will I see you again? Come back and play again!! \n (Tap to continue)",
            "Enjoy your prize."
           };*/
        
        

        /*[SerializeField]
        private TreasureData m_IntroData;
        [SerializeField]
        private TreasureData m_CluesData;*/
       
       

        void Start()
        {
            // Create data
            m_State = ESTATE.NONE;
            //m_CluesData = new TreasureData();
            m_NumberCluesFound = 0;


            StartCoroutine(Init());
            // Set Intro
            //DoIntro();
        }


        private IEnumerator Init()
        {
            m_Data = new TreasureData();

            // Retrieve information from server
            yield return Utility.FileRequestManager.Instance.RequestFiles();

            // Map JSON Data
            for (int i=0; i< Utility.FileRequestManager.Instance.FileData.Data.Count; i++)
            {
                string data = Utility.FileRequestManager.Instance.FileData.Data[i].Data;
                m_UI.DebugTxt.text += "Data: " + data;

                Debug.Log("DATA " + data);
                m_Data = JsonMapper.ToObject<TreasureData>(data);
                /*if (Utility.FileRequestManager.Instance.FileData.Data[i].FileName == "Intro")
                {
                    m_IntroData = JsonMapper.ToObject<TreasureData>(data);
                }
                else if (Utility.FileRequestManager.Instance.FileData.Data[i].FileName == "Clues")
                {
                    m_CluesData = JsonMapper.ToObject<TreasureData>(data);
                }*/
            }

            yield return new WaitForEndOfFrame();

            m_State = ESTATE.INTRO;
            m_UI.ActiveButton = false;
            m_UI.MessageUI.SetMessage("");
            m_UI.MessageUI.Hide();

            m_MessageID = -1;
            m_UI.MessageUI.Show(true);
            m_UI.MessageUI.OnMessageEnd += OnEndOfMessage;

            OnAdvanceStep();


            yield return new WaitForSeconds(2.0f);

        }

        private void OnEndOfMessage()
        {
            m_UI.ActiveButton = true;
        }




        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }


        public void OnAdvanceStep()
        {
            if (m_State == ESTATE.INTRO)
            {
                m_MessageID++;
                m_UI.ActiveButton = false;
                if (m_MessageID < m_Data.Intro.Count)
                {
                    m_UI.MessageUI.SetMessage(m_Data.Intro[m_MessageID]);
                }else
                {
                    Debug.Log(" END OF INTRO: ");
                }
            }



            /*if (m_TMessage == TYPEMESSAGE.INTRO)
            {*/
                
                
                /*if (m_IndexMessage < 0)
                {

                    m_TreasureUI.MessageUI.SetMessage(m_IntroData.Intro[m_IndexMessage]);
                }
                else
                {*/
                    //MessagesUI.OnEndShowMessage -= OnEndMessageLine;
                    // Get the first clue, for the clue
                    //TreasureHunterClue clue = m_DataTreasure.GetClueByID("Clue0");
                    //m_TreasureUI.MessageUI.SetMessage(, 0.08f, 2.5f);

                    // Subscribe to the markers
                    /* for (int i = 0; i < m_ListMarkers.Count; i++)
                     {
                         if (m_ListMarkers[i] != null)
                         {
                             m_ListMarkers[i].OnClueFound += OnClueFound;
                             m_ListMarkers[i].OnClueLost += OnClueLost;
                         }
                     }*/

              //  }
           // }
            /*else if (m_TMessage == TYPEMESSAGE.END)
            {
                m_IndexMessage++;
                if (m_IndexMessage < m_EndHuntMessage.Length)
                {
                    m_TreasureUI.MessageUI.SetMessage(m_EndHuntMessage[m_IndexMessage]);
                }
                else
                {
                    MessagesUI.OnEndShowMessage -= OnEndMessageLine;
                }
            }*/
        }

        #region Intro
        /*private void DoIntro()
        {
            m_TMessage = TYPEMESSAGE.INTRO;
            m_TreasureUI.ActiveButton = false;
            m_TreasureUI.MessageUI.SetMessage("");
            m_TreasureUI.MessageUI.Hide();
            StartCoroutine(RoutineIntro());
        }*/

        /*private IEnumerator RoutineIntro()
        {
            m_TreasureUI.MessageUI.Show(true);
            yield return new WaitForSeconds(2.0f);
            m_IndexMessage = 0;
            m_TreasureUI.MessageUI.SetMessage(m_IntroData.Intro[m_IndexMessage], 0.08f, 0.0f);
            MessagesUI.OnEndShowMessage += OnEndMessageLine;
        }*/

        

        #endregion Intro

        

        public void OnMarkerFound(string id, int markerID)
        {
            // Find clue
            /*TreasureHunterClue clue = m_DataTreasure.GetClueByID(id);
            if ((!clue.Active) && (!string.IsNullOrEmpty(clue.ID)))
            {
                clue.Active = true;

                m_NumberCluesFound++;

                if (m_NumberCluesFound < m_DataTreasure.NumberClues)
                {
                    // Instance egg and set new message
                    InstanceEgg();
                    m_TreasureUI.MessageUI.SetMessage(clue.Clue, 0.08f, 0.0f);

                }
                else
                {
                    InstanceEgg();
                    // Show last message
                    m_TreasureUI.MessageUI.SetMessage(clue.Clue, 0.08f, 0.0f);
                    MessagesUI.OnEndShowMessage += OnEndMessageLine;

                    // Unsubscribe to the markers
                   /* for (int i = 0; i < m_ListMarkers.Count; i++)
                    {
                        m_ListMarkers[i].OnClueFound -= OnClueFound;
                        m_ListMarkers[i].OnClueLost -= OnClueLost;
                    }*/

                    // Show end message
                   /* m_TMessage = TYPEMESSAGE.END;
                    m_IndexMessage = 0;*/
              /*  }
            }*/
        }

        public void OnMarkerLost(string id, int markerID)
        {
            //DestroyCurrentEgg();
        }
        /*private void DestroyCurrentEgg()
        {
            if (m_CurrentEgg != null)
            {
                Destroy(m_CurrentEgg);
            }
        }

        private void InstanceEgg()
        {
            DestroyCurrentEgg();
            int idEgg = Random.Range(0, m_ListEggPrefabs.Count);
            m_CurrentEgg = Instantiate(m_ListEggPrefabs[idEgg]);
        }*/
    }
}
