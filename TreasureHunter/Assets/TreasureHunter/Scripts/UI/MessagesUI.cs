using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Utility;
using Utility.UI;
using UnityEngine.EventSystems;

namespace TreasureHunt
{
    public class MessagesUI : UIBase
    {
        public delegate void MessagesAction();
        public event MessagesAction OnMessageEnd;

        [SerializeField] private Text m_Title;
        [SerializeField] private ScrollTextUI           m_ScrollMessages;

        private int                                     m_indexLetter = 0;
        private string                                  m_MessageText = "";
        [SerializeField]
        private float m_SpeedBetweenLetters = 0.02f;

        [SerializeField] private EventTrigger m_EventTrigger;

        public override void Show()
        {
            m_Title.text = "";

            m_ScrollMessages.Description = "";

            DisableButton();

            base.Show();
        }

        public void DisableButton()
        {
            m_EventTrigger.enabled = false;
        }

        public void EnableButton()
        {
            m_EventTrigger.enabled = true;
        }


        public void SetMessage(string title, string text, float endPause = 0.1f)
        {
            m_Title.text = title;

            m_ScrollMessages.Description = "";

            m_MessageText = text;

            m_indexLetter = 0;

            if (m_SpeedBetweenLetters > 0.0f)
            {
                StopAllCoroutines();
                StartCoroutine(UpdateMessage(endPause));
            }
            else
            {
                m_ScrollMessages.Description = text;
            }
            
        }

        private IEnumerator UpdateMessage(float endPause)
        {
            do
            {
                if (m_indexLetter < m_MessageText.Length)
                {
                    m_ScrollMessages.Description += m_MessageText[m_indexLetter];
                }

                yield return new WaitForSeconds(m_SpeedBetweenLetters);

                if ((m_indexLetter + 1) <= m_MessageText.Length)
                {
                    m_indexLetter++;
                }

            }

            while (m_indexLetter < m_MessageText.Length);
            // Pause
            yield return new WaitForSeconds(endPause);
            if (OnMessageEnd != null)
            {
                OnMessageEnd();
            }
        }
    }
}
