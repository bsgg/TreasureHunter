using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TreasureHunt
{
    public class MessageLetter : MonoBehaviour
    {
        [Header("Animate on Star")]
        [SerializeField] private bool                           m_AnimateOnStart;
        [SerializeField] private float                          m_DelayToStart = 0.3f;

        [Header("List Letters")]
        [SerializeField] private List<Letter>                  m_ListObjects;

        [Header("Rotation Settings")]
        [SerializeField] private float                         m_DelayToNextLetter = 0.1f;
        [SerializeField] private Vector3                       m_InitRotation;
        [SerializeField] private Vector3                       m_FinalRotation;
        [SerializeField] private float                         m_SpeedRotation = 1.0f;

        void Start()
        {
            if (m_AnimateOnStart)
            {
                Init();
            }
        }

        private void Init()
        {
            for (int i = 0; i < m_ListObjects.Count; i++)
            {
                m_ListObjects[i].InitLetter(m_InitRotation, m_FinalRotation, m_SpeedRotation);
            }
            StartCoroutine(RoutineInit());
        }

        private IEnumerator RoutineInit()
        {            
            yield return new WaitForSeconds(m_DelayToStart);

            for (int i = 0; i < m_ListObjects.Count; i++)
            {
                m_ListObjects[i].StartAnimation();
                yield return new WaitForSeconds(m_DelayToNextLetter);
            }
        }

    }
}
