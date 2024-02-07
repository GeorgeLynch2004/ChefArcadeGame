using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationSystem : MonoBehaviour
{
    [SerializeField] private Animator m_AIAnimator;
    [SerializeField] private Vector3 m_LastFramePosition;

    private void Update()
    {
        if (transform.position != m_LastFramePosition)
        {
            m_AIAnimator.SetTrigger("Moving");
        }
        else
        {
            m_AIAnimator.SetTrigger("Idle");
        }

        m_LastFramePosition = transform.position;
    }

}
