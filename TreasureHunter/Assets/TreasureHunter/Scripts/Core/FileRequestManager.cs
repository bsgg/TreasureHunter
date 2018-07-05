
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{

    [System.Serializable]
    public class IndexFile
    {
        public string FileName;
        public string URL;
        public string Data;
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

    public class FileRequestManager : MonoBehaviour
    {
        #region Instance
        private static FileRequestManager m_Instance;
        public static FileRequestManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (FileRequestManager)FindObjectOfType(typeof(FileRequestManager));

                    if (m_Instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(FileRequestManager) + " is needed in the scene, but there is none.");
                    }
                }
                return m_Instance;
            }
        }
        #endregion Instance

        [SerializeField]
        private string m_FileDataUrl = "http://beatrizcv.com/Data/FileData.json";

        [SerializeField]
        private TreasureData m_Data;
        public TreasureData Data
        {
            get
            {
                return m_Data;
            }
        }

        private float m_PercentProgress;
        private string m_ProgressText;

        public string ProgressText
        {
            get { return m_ProgressText; }
        }
        

        public IEnumerator RequestFiles()
        {
            m_Data = new TreasureData();

            

            m_PercentProgress = 0.0f;
            m_ProgressText = m_PercentProgress.ToString() + " % ";

            if (string.IsNullOrEmpty(m_FileDataUrl))
            {
                Debug.LogWarning("<color=yellow>" + "[FileRequestManager] File Data Url is null or empty" + "</color>");
                yield return null;
            }

            WWW wwwFile = new WWW(m_FileDataUrl);

           

            while (!wwwFile.isDone)
            {
                int progress = (int)(wwwFile.progress * 100);
                TreasureHunt.AppController.Instance.UI.Progress.SetProgress(progress);
            }

            yield return wwwFile;

            TreasureHunt.AppController.Instance.UI.Progress.SetProgress(100);

            if (!string.IsNullOrEmpty(wwwFile.text))
            {
                m_Data = JsonUtility.FromJson<TreasureData>(wwwFile.text);
            }else
            {
                Debug.LogWarning("<color=yellow>" + "[FileRequestManager] File Data Json is null or empty" + "</color>");
            }

                
                    

                    /*Debug.LogWarning("<color=yellow>" + "[FileRequestManager] Requesting... " + m_FileData.Data.Count + " Files " + "</color>");
                    for (int i = 0; i < m_FileData.Data.Count; i++)
                    {
                        if (string.IsNullOrEmpty(m_FileData.Data[i].URL))
                        {
                            continue;
                        }

                        // Request
                        Debug.LogWarning("<color=yellow>" + "[FileRequestManager] Requesting: " + (i + 1) + "/" + m_FileData.Data.Count + " : URL: " + m_FileData.Data[i].URL + " Filename: " + m_FileData.Data[i].FileName + "</color>");
                        WWW www = new WWW(m_FileData.Data[i].URL);
                        while (!www.isDone)
                        {
                            m_PercentProgress = www.progress * 100.0f;
                            m_ProgressText = m_PercentProgress.ToString() + " % ";
                            yield return null;
                        }

                        m_PercentProgress = www.progress * 100.0f;
                        m_ProgressText = m_PercentProgress.ToString() + " % ";

                        m_FileData.Data[i].Data = www.text;

                        Debug.LogWarning("<color=yellow>" + "[FileRequestManager] Got: "+ www.text + "</color>");
                    }*/
                       
        }

    }
}
