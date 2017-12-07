using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;

namespace TreasureHunt
{
    public class TrackableCard : MonoBehaviour, ITrackableEventHandler    
    {
        [SerializeField] private GameObject m_ConfetiPrefab;
        private GameObject m_Confeti;

        [SerializeField]        private GameObject m_MessagePrefab;
        private GameObject m_Message;

        [SerializeField]
        private float m_DelayToShowMessage;
        [SerializeField]
        private float m_DelayToShowFade;
        [SerializeField]
        private float m_DelayToChangeScene;

        private bool m_CardInitialize = false;

        [SerializeField]
        private Utility.FadeEffect m_FadeEffect;
        [SerializeField]
        private string m_NameScene;

        private TrackableBehaviour m_TrackableBehaviour;

        void Start()
        {
            m_CardInitialize = false;
            m_TrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (m_TrackableBehaviour)
            {
                m_TrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
               newStatus == TrackableBehaviour.Status.TRACKED ||
               newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                if (!m_CardInitialize)
                {
                    StartCoroutine(AnimateCard());
                }
            }            
        }

        private IEnumerator AnimateCard()
        {
            m_Confeti = Instantiate(m_ConfetiPrefab);

            yield return new WaitForSeconds(m_DelayToShowMessage);
            m_Message = Instantiate(m_MessagePrefab);


            yield return new WaitForSeconds(m_DelayToShowFade);
            m_FadeEffect.DoFadeIn();
            m_CardInitialize = true;

            yield return new WaitForSeconds(m_DelayToChangeScene);
            SceneManager.LoadScene(m_NameScene);

        }
    }
}
