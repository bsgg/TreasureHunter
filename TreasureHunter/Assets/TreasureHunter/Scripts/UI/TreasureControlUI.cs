using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace TreasureHunt
{
    public class TreasureControlUI : MonoBehaviour
    {
        [SerializeField]
        private MessagesUI m_MessageUI;
        public MessagesUI MessageUI
        {
            get { return m_MessageUI; }
        }

        [SerializeField]
        private Button m_TapButton;
        private bool m_ActiveButton;
        public bool ActiveButton
        {
            get { return m_ActiveButton; }
            set
            {
                m_ActiveButton = value;
                m_TapButton.gameObject.SetActive(value);
            }
        }

    }
}
