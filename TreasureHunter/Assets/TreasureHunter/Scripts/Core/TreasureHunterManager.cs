using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunt
{
    public class TreasureHunterManager : MonoBehaviour
    {
        [SerializeField] private TreasureControlUI m_TreasureUI;
        [SerializeField] private List<TreasureHunterTrackableEvent> m_ListMarkers;

        [Header("Egg prefabs")]
        [SerializeField] private List<GameObject> m_ListEggPrefabs;
        private GameObject m_CurrentEgg;

        private TreasureHunterData m_DataTreasure;

        private enum TYPEMESSAGE { NONE, INTRO, END };
        private TYPEMESSAGE m_TMessage;

        private string[] m_IntroMessage = new string[7]
            {
            "Hey! Welcome to this little game specially prepared for you =) \n (Tap to continue)",
            "First, allow me to explain how to play. \n (Tap to continue)",
            "You'll find little pictures hiding all over the place. \n (Tap to continue)",
            "Use your camera to scan them and you'll be able to get a ... clue! \n (Tap to continue)",
            "Think hard to find out the next location.... A surprise is waiting for you! \n (Tap to continue)",
            "Without any delay... Let's begin the game... \n (Tap to continue)",
            "Ah! I almost forgot.... here is the first clue: \n (Tap to continue)"
            };

        private string[] m_EndHuntMessage = new string[2]
           {
            "Will I see you again? Come back and play again!! \n (Tap to continue)",
            "Enjoy your prize."
           };
        private int m_IndexMessage;
        private int m_NumberCluesFound;

        void Start()
        {
            // Create data
            m_TMessage = TYPEMESSAGE.NONE;
            m_DataTreasure = new TreasureHunterData();
            m_NumberCluesFound = 1;

            // Set Intro
            DoIntro();
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
            m_TreasureUI.MessageUI.SetMessage(m_IntroMessage[m_IndexMessage], 0.08f, 0.0f);
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
                if (m_IndexMessage < m_IntroMessage.Length)
                {

                    m_TreasureUI.MessageUI.SetMessage(m_IntroMessage[m_IndexMessage]);
                }
                else
                {
                    MessagesUI.OnEndShowMessage -= OnEndMessageLine;
                    // Get the first clue, for the clue
                    TreasureHunterClue clue = m_DataTreasure.GetClueByID("Clue0");
                    m_TreasureUI.MessageUI.SetMessage(clue.Clue, 0.08f, 2.5f);

                    // Subscribe to the markers
                    for (int i = 0; i < m_ListMarkers.Count; i++)
                    {
                        if (m_ListMarkers[i] != null)
                        {
                            m_ListMarkers[i].OnClueFound += OnClueFound;
                            m_ListMarkers[i].OnClueLost += OnClueLost;
                        }
                    }

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

        public void OnClueFound(string id)
        {
            // Find clue
            TreasureHunterClue clue = m_DataTreasure.GetClueByID(id);
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
                    for (int i = 0; i < m_ListMarkers.Count; i++)
                    {
                        m_ListMarkers[i].OnClueFound -= OnClueFound;
                        m_ListMarkers[i].OnClueLost -= OnClueLost;
                    }

                    // Show end message
                    m_TMessage = TYPEMESSAGE.END;
                    m_IndexMessage = 0;
                }
            }
        }

        public void OnClueLost(string id)
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
