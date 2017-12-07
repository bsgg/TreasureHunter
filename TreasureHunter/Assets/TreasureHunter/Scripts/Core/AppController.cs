using LitJson;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TreasureHunt
{
    [System.Serializable]
    public class IndexFile
    {
        public string FileName;
        public string URL;
    }

    [System.Serializable]
    public class FileData
    {
        public List<IndexFile> Data;
        public FileData()
        {
            Data = new List<IndexFile>();
        }
    }


    [System.Serializable]
    public class TreasureData
    {
        public List<string> Data;

        public TreasureData()
        {
            Data = new List<string>();
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

        public enum EMarkerType { NONE =-1, MARKER1 = 0, MARKER2, MARKER3, MARKER4, MARKER5, MARKER6, MARKER7, MARKER8, MARKER9, MARKER10 };

        [SerializeField] private TreasureControlUI m_TreasureUI;
        //[SerializeField] private List<TreasureHunterTrackableEvent> m_ListMarkers;

        [Header("Egg prefabs")]
        [SerializeField] private List<GameObject> m_ListEggPrefabs;
        private GameObject m_CurrentEgg;

        private TreasureData m_TreasureData;

        private enum TYPEMESSAGE { NONE, INTRO, END };
        private TYPEMESSAGE m_TMessage;

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

        private string[] m_EndHuntMessage = new string[2]
           {
            "Will I see you again? Come back and play again!! \n (Tap to continue)",
            "Enjoy your prize."
           };
        private int m_IndexMessage;
        private int m_NumberCluesFound;

        [SerializeField]
        private TreasureData m_IntroData;
        [SerializeField]
        private TreasureData m_CluesData;


        [SerializeField]
        private string m_FileDataUrl = "http://beatrizcv.com/Data/FileData.json";
        [SerializeField]
        private FileData m_FileData;

        void Start()
        {
            // Create data
            m_TMessage = TYPEMESSAGE.NONE;
            //m_CluesData = new TreasureData();
            m_NumberCluesFound = 1;


            StartCoroutine(RequestFiles());
            // Set Intro
            DoIntro();
        }

        private IEnumerator RequestFiles()
        {

            // Request File data
            m_FileData = new FileData();

            WWW wwwFile = new WWW(m_FileDataUrl);
            yield return wwwFile;
            string jsonData = wwwFile.text;
            if (!string.IsNullOrEmpty(jsonData))
            {
                if (!string.IsNullOrEmpty(jsonData))
                {
                    m_FileData = JsonMapper.ToObject<FileData>(jsonData);
                }
            }

            yield return new WaitForEndOfFrame();
            for (int i=0; i<m_FileData.Data.Count; i++)
            {
                m_TreasureUI.DebugTxt.text += "Retrieving file: " + m_FileData.Data[i].FileName;

                if (string.IsNullOrEmpty(m_FileData.Data[i].URL))
                {
                    continue;
                }
                WWW www = new WWW(m_FileData.Data[i].URL);

                float progress = 0.0f;
                string progressText = "DOWNLOAD PROGRESS: " + progress.ToString() + "%";
                while (!www.isDone)
                {
                    progress = www.progress * 100.0f;
                    progressText = "DOWNLOAD PROGRESS: " + progress.ToString() + "%";
                    m_TreasureUI.DebugTxt1.text = progressText + "    - Bytes Downloaded: " + www.bytesDownloaded;


                    yield return null;
                }

                progress = www.progress * 100.0f;
                progressText = "DOWNLOAD PROGRESS: " + progress.ToString() + "%";
                m_TreasureUI.DebugTxt1.text = progressText + "    - Bytes Downloaded: " + www.bytesDownloaded;


                jsonData = www.text;
                if (!string.IsNullOrEmpty(www.text))
                {
                    if (!string.IsNullOrEmpty(www.text))
                    {
                        m_TreasureUI.DebugTxt.text += "Data: " + www.text;
                        if (m_FileData.Data[i].FileName == "Intro")
                        {
                            m_IntroData = JsonMapper.ToObject<TreasureData>(www.text);
                        }else if (m_FileData.Data[i].FileName == "Clues")
                        {
                            m_CluesData = JsonMapper.ToObject<TreasureData>(www.text);
                        }

                    }
                }
            }
            yield return new WaitForEndOfFrame();




            /*Debug.Log("[AppController.RetrieveInfo] Start to retrieve info...");

            m_TreasureUI.DebugTxt.text = "Retrieve Intro from: " + m_IntroUrl;

            m_IntroData = new TreasureData();
            m_CluesData = new TreasureData();           

            // Retrieve clues
            WWW wwwIntro = new WWW(m_IntroUrl);

            float progress = wwwIntro.progress * 100.0f;
            string progressText = "DOWNLOAD PROGRESS: " + progress.ToString() + "%";

            m_TreasureUI.DebugTxt1.text = progressText + "    - Bytes Downloaded: " + wwwIntro.bytesDownloaded;

            while (!wwwIntro.isDone)
            {
                progress = wwwIntro.progress * 100.0f;
                progressText = "DOWNLOAD PROGRESS: " + progress.ToString() + "%";
                m_TreasureUI.DebugTxt1.text = progressText + "    - Bytes Downloaded: " + wwwIntro.bytesDownloaded;

                yield return null;
            }

            progress = wwwIntro.progress * 100.0f;
            progressText = "DOWNLOAD PROGRESS: " + progress.ToString() + "%";
            m_TreasureUI.DebugTxt1.text = progressText + "    - Bytes Downloaded: " + wwwIntro.bytesDownloaded;

            yield return wwwIntro;

            Debug.Log("[AppController.RetrieveInfo] End...");

            m_TreasureUI.DebugTxt.text = "Data: " + wwwIntro.text;

            string jsonCluesData = wwwIntro.text;
            if (!string.IsNullOrEmpty(jsonCluesData))
            {
                Debug.Log("[AppController.RetrieveInfo] Info: " + jsonCluesData);

                if (!string.IsNullOrEmpty(jsonCluesData))
                {
                    m_IntroData = JsonMapper.ToObject<TreasureData>(jsonCluesData);
                }
            }

            yield return new WaitForEndOfFrame();

            // Retrieve clues
            WWW wwwClues = new WWW(m_UrlClues);
            yield return wwwClues;           

            Debug.Log("[AppController.RetrieveInfo] End...");
           jsonCluesData = wwwClues.text;
            if (!string.IsNullOrEmpty(jsonCluesData))
            {
                Debug.Log("[AppController.RetrieveInfo] Info: " + jsonCluesData);

                if (!string.IsNullOrEmpty(jsonCluesData))
                {
                    m_CluesData = JsonMapper.ToObject<TreasureData>(jsonCluesData);
                }
            }

            yield return new WaitForEndOfFrame();*/
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        #region Intro
        private void DoIntro()
        {
            m_TMessage = TYPEMESSAGE.INTRO;
            m_TreasureUI.ActiveButton = false;
            m_TreasureUI.MessageUI.SetMessage("");
            m_TreasureUI.MessageUI.Hide();
            StartCoroutine(RoutineIntro());
        }

        private IEnumerator RoutineIntro()
        {
            m_TreasureUI.MessageUI.Show(true);
            yield return new WaitForSeconds(2.0f);
            m_IndexMessage = 0;
            m_TreasureUI.MessageUI.SetMessage(m_IntroData.Data[m_IndexMessage], 0.08f, 0.0f);
            MessagesUI.OnEndShowMessage += OnEndMessageLine;
        }

        public void OnEndMessageLine()
        {
            m_TreasureUI.ActiveButton = true;
        }

        #endregion Intro

        public void OnTapButton()
        {
            if (m_TMessage == TYPEMESSAGE.INTRO)
            {
                m_TreasureUI.ActiveButton = false;
                m_IndexMessage++;
                if (m_IndexMessage <0)
                {

                    m_TreasureUI.MessageUI.SetMessage(m_IntroData.Data[m_IndexMessage]);
                }
                else
                {
                    MessagesUI.OnEndShowMessage -= OnEndMessageLine;
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

                }
            }
            else if (m_TMessage == TYPEMESSAGE.END)
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
            }
        }

        public void OnMarkerFound(string id, EMarkerType marker)
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

        public void OnMarkerLost(string id, EMarkerType marker)
        {
            DestroyCurrentEgg();
        }
        private void DestroyCurrentEgg()
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
        }
    }
}
