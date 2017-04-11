using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace TreasureHunt
{
    public class TreasureHunterTrackableEvent : MonoBehaviour, ITrackableEventHandler
    {
        public delegate void DelegateOnMessageMarker(string id);
        public event DelegateOnMessageMarker OnClueFound;
        public event DelegateOnMessageMarker OnClueLost;

        [SerializeField] private string m_IDClue = "Clue0";
        private TrackableBehaviour m_TrackableBehaviour;

        void Start()
        {
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

                if (OnClueFound != null)
                {
                    OnClueFound(m_IDClue);
                }
            }
            else
            {
                if (OnClueLost != null)
                {
                    OnClueLost(m_IDClue);
                }
            }
        }

    }
}
