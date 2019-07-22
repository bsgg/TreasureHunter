
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

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

        public TreasureData data { get; set; }

        private float percentProgress;
        public string progressText { get; private set; }
       

        public IEnumerator RequestFiles()
        {
            data = new TreasureData();

            percentProgress = 0.0f;
            progressText = percentProgress.ToString() + " % ";

            if (string.IsNullOrEmpty(m_FileDataUrl))
            {
                Debug.LogWarning("<color=yellow>" + "[FileRequestManager] File Data Url is null or empty" + "</color>");
                yield return null;
            }

            TreasureHunt.AppController.Instance.UI.Progress.SetProgress("Downloading Clues...", 0);

            using (UnityWebRequest webRequest = UnityWebRequest.Get(m_FileDataUrl))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    Debug.LogWarning("<color=yellow>" + "[FileRequestManager] There was an error: " + webRequest.isNetworkError.ToString() + "</color>");

                    yield return null;
                }

                DownloadHandler handler = webRequest.downloadHandler;

                Debug.LogWarning("<color=cyan>" + "[FileRequestManager] Received: " + handler.text + "</color>");

                data = JsonUtility.FromJson<TreasureData>(handler.text);

            }

            TreasureHunt.AppController.Instance.UI.Progress.SetProgress("Clues downloaed", 100);           
        }

    }
}
