
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

                TreasureHunt.AppController.Instance.UI.Progress.SetProgress("Downloading\n"+ progress+ "%", progress);
            }

            yield return wwwFile;

            TreasureHunt.AppController.Instance.UI.Progress.SetProgress("Downloading\n" + 100 + "%", 100);

            if (!string.IsNullOrEmpty(wwwFile.text))
            {
                m_Data = JsonUtility.FromJson<TreasureData>(wwwFile.text);
            }else
            {
                Debug.LogWarning("<color=yellow>" + "[FileRequestManager] File Data Json is null or empty" + "</color>");
            }
            
        }

    }
}
