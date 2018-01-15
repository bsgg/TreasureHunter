using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TreasureHunt
{
    public class MessagesUI : MonoBehaviour
    {
        public delegate void MessagesAction();
        public event MessagesAction OnMessageEnd;
        [SerializeField] private CanvasGroup            m_Panel;
        [SerializeField] private Text                   m_LblMessage;
        private int                                     m_indexLetter = 0;
        private string                                  m_MessageText = "";
        [SerializeField]
        private float m_SpeedBetweenLetters = 0.02f;
        
        public void Show(bool fade = false)
        {
            if (fade)
            {
                StartCoroutine(Utility.UIAnimation.FadeCanvasGroup(m_Panel, 0.0f, 1.0f, 0.3f));
            }else
            {
                m_Panel.alpha = 1.0f;
            }
        }        

        public void Hide(bool fade = false)
        {
            if (fade)
            {
                StartCoroutine(Utility.UIAnimation.FadeCanvasGroup(m_Panel, 1.0f, 0.0f, 0.1f));
            }else
            {
                m_Panel.alpha = 0.0f;
            }
        }

        public void SetMessage(string text, float endPause = 0.1f)
        {
            m_LblMessage.text = "";
            m_MessageText = text;

            //StopAllCoroutines();
            if (m_SpeedBetweenLetters > 0.0f)
            {
                StartCoroutine(RoutineMessage(endPause));
            }
            else
            {
                m_LblMessage.text = text;
            }
            
        }

        private IEnumerator RoutineMessage(float endPause)
        {
            do
            {
                if (m_indexLetter < m_MessageText.Length)
                {
                    m_LblMessage.text += m_MessageText[m_indexLetter];
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
