using UnityEngine;
using System.Collections;

namespace TreasureHunt
{
    public class CardIntroManager : MonoBehaviour
    {
        [SerializeField] private MessagesUI m_MessageUI;

        private string m_CardMessage = "Hurry up! Scan the card!!";

        void Start()
        {
            // Set Intro
            DoIntro();
        }

        private void DoIntro()
        {
            m_MessageUI.SetMessage(m_CardMessage);
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
