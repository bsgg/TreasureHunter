using UnityEngine;
using System.Collections;

namespace Utility
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField]  private float m_DelayToDestroy;

	    void Start ()
        {
            Destroy(gameObject, m_DelayToDestroy);
	    }
	
	
	}
}
