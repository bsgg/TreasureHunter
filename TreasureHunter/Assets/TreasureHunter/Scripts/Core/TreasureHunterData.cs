using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace TreasureHunt
{
    /*public class TreasureHunterClue
    {
        public string ID;
        public string Clue;
        public bool Active;
        public TreasureHunterClue()
        {
            ID = "";
            Clue = "";
            Active = false;
        }
    }

    public class TreasureHunterDictionary
    {
        private List<TreasureHunterClue> m_treasureHunterClues;
        public List<TreasureHunterClue> TreasureHunterClues
        {
            get { return m_treasureHunterClues; }
            set { m_treasureHunterClues = value; }
        }

        public TreasureHunterDictionary()
        {
            m_treasureHunterClues = new List<TreasureHunterClue>();
        }

        public TreasureHunterClue GetClueByID(string id)
        {
            for (int i = 0; i < m_treasureHunterClues.Count; i++)
            {
                if (m_treasureHunterClues[i].ID == id)
                {
                    return m_treasureHunterClues[i];
                }
            }
            return new TreasureHunterClue();
        }
    }

    public class TreasureHunterData
    {
        private TreasureHunterDictionary m_TreasureDictionary;
        public int NumberClues
        {
            get { return m_TreasureDictionary.TreasureHunterClues.Count; }
        }

        public TreasureHunterData()
        {
            m_TreasureDictionary = new TreasureHunterDictionary();

            // Load JSON file
            string jsonActionsString = LoadJSONResource("Data/TreasureHunterClues");
            if (!string.IsNullOrEmpty(jsonActionsString))
            {
                m_TreasureDictionary = JsonMapper.ToObject<TreasureHunterDictionary>(jsonActionsString);
            }
        }


        /// <summary>
        /// Method to load a JSON with a given path
        /// </summary>
        /// <returns>The JSON resource.</returns>
        /// <param name="pathFile">Path file.</param>
        public static string LoadJSONResource(string pathFile)
        {
            TextAsset text_asset = (TextAsset)Resources.Load(pathFile, typeof(TextAsset));
            if (text_asset == null)
            {
                Debug.Log("ERROR: Could not find file: Assets/Resources/" + pathFile);
                return "";
            }
            string json_string = text_asset.ToString();
            return json_string;
        }

        public TreasureHunterClue GetClueByID(string id)
        {
            return m_TreasureDictionary.GetClueByID(id);
        }

    }*/
}
