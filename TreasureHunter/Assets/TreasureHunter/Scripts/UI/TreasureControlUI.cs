﻿using System.Collections;
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
        }


    }
}
