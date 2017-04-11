using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace TreasureHunt
{
    public class MessageIvan : MonoBehaviour
    {
        [Header("Animate on Star")]
        [SerializeField] private bool m_AnimateOnStart = true;
        [SerializeField] private float m_DelayToStart = 0.3f;
        

       void Start()
        {
            if (m_AnimateOnStart) 
            {
                StartCoroutine(RoutineAnimation());
            }
        }

        private IEnumerator RoutineAnimation()
        {
            float currentScale = 0.1f;
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);

            yield return new WaitForSeconds(m_DelayToStart);
            
            transform.DOScale(1.8f, 0.3f);

            yield return new WaitForSeconds(0.3f);

            transform.DOScale(1.0f, 0.3f);

            yield return new WaitForSeconds(0.2f);

            transform.DOScale(1.4f, 0.3f);

            yield return new WaitForSeconds(0.2f);

            transform.DOScale(1.3f, 0.3f);
        }
    }
}
