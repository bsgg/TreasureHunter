using UnityEngine;
using System.Collections;

namespace TreasureHunt
{
    public class Letter : MonoBehaviour
    {
        [SerializeField] private MeshRenderer   m_MeshRenderer;
        private Color                           m_CurrentColorMesh;

        private Vector3 m_FinalRotation;
        private float m_SpeedRotation;

        public void InitLetter(Vector3 InitRot, Vector3 finalRot, float speed)
        {
            m_CurrentColorMesh = m_MeshRenderer.materials[0].color;
            m_CurrentColorMesh.a = 0.0f;
            m_MeshRenderer.materials[0].color = m_CurrentColorMesh;

            transform.localRotation = Quaternion.Euler(InitRot);
            m_FinalRotation = finalRot;
            m_SpeedRotation = speed;
        }

        public void StartAnimation()
        {
            m_CurrentColorMesh.a = 1.0f;
            m_MeshRenderer.materials[0].color = m_CurrentColorMesh;
            StartCoroutine(RoutineRotation());
        }

        private IEnumerator RoutineRotation()
        {
            float time = 0.0f;
            while (time < 1.0f)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(m_FinalRotation), Time.deltaTime * m_SpeedRotation);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

        }
    }
}
