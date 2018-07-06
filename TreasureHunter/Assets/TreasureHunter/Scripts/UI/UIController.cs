using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Utility;
using Utility.UI;

namespace TreasureHunt
{
    public class UIController : MonoBehaviour
    {

        [SerializeField] private PopupWithButtons m_PopupButtons;
        public PopupWithButtons PopupButtons
        {
            get { return m_PopupButtons; }
        } 

        [SerializeField] private ProgressUI m_Progress;
        public ProgressUI Progress
        {
            get { return m_Progress; }
        }

        [SerializeField] private MessagesUI m_MessageUI;
        public MessagesUI MessageUI
        {
            get { return m_MessageUI; }
        }



        public void HideAll()
        {
            m_MessageUI.Hide();
            m_PopupButtons.Hide();
            Progress.Hide();
        }

        

       /* [SerializeField]
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


        [SerializeField]
        private Text m_DebugTxt;
        public Text DebugTxt
        {
            get { return m_DebugTxt; }
        }

        [SerializeField]
        private Text m_DebugTxt1;
        public Text DebugTxt1
        {
            get { return m_DebugTxt1; }
        }*/


    }
}
