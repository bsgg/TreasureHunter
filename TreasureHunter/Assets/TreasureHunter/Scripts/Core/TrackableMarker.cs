﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace TreasureHunt
{
    public class TrackableMarker : MonoBehaviour, ITrackableEventHandler
    {
        private TrackableBehaviour m_TrackableBehaviour;

        [SerializeField] private int m_ID = -1;

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
            if (m_TrackableBehaviour == null) return;

            if (newStatus == TrackableBehaviour.Status.DETECTED ||
               newStatus == TrackableBehaviour.Status.TRACKED ||
               newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {

                AppController.Instance.OnMarkerFound(m_TrackableBehaviour.TrackableName, m_ID);
            }
            else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                      newStatus == TrackableBehaviour.Status.NO_POSE)
            {
                AppController.Instance.OnMarkerLost(m_TrackableBehaviour.TrackableName, m_ID);
            }
            else
            {
                AppController.Instance.OnMarkerLost(m_TrackableBehaviour.TrackableName, m_ID);
            }
        }

    }
}
