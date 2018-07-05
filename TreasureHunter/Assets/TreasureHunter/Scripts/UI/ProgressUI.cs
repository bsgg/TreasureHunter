using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.UI;

namespace TreasureHunt
{
    public class ProgressUI : Utility.UIBase
    {

        [SerializeField] private Text m_Title;

        public string Title
        {
            set { m_Title.text = value; }
        }

        [SerializeField] private Image m_ProgressImage;
        [SerializeField] private Text m_ProgressText;

        public void SetProgress(int value)
        {
            m_ProgressText.text = value + "%";
            m_ProgressImage.fillAmount = value / 100.0f;
        }
	}
}
