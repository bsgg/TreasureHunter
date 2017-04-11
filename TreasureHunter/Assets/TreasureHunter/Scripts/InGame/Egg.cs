using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TreasureHunt
{
    public class Egg : MonoBehaviour
    {
        [SerializeField]   private float m_Scale;

        void Start()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            float currentScale = 1.0f;
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            transform.localPosition = new Vector3(Random.Range(-3.0f, 5.0f), -3.0f, Random.Range(-3.0f, 5.0f));

            yield return new WaitForSeconds(0.3f);

            transform.DOScale(m_Scale, 0.2f);

            transform.DOJump(new Vector3(0.0f, -4.0f, 0.0f), 15.0f, 1, 0.5f);
            transform.DORotate(new Vector3(0, 80.0f, 0), 1.0f, RotateMode.Fast);

            yield return new WaitForSeconds(0.5f);
            transform.DOJump(new Vector3(0.0f, 0.0f, 0.0f), 1.0f, 1, 0.5f);
        }
    }
}
