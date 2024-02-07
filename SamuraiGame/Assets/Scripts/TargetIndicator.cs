using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private GameObject m_Target;
    [SerializeField] private float m_OffScreenThreshold;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private bool m_IsIndicatorActive;

    private void Start()
    {
        m_MainCamera = Camera.main;
    }

    private void Update()
    {
        if (m_IsIndicatorActive)
        {
            Vector3 targetDirection = m_Target.transform.position - transform.position;
            float distanceToTarget = targetDirection.magnitude;

            if (distanceToTarget < m_OffScreenThreshold)
            {
                gameObject.SetActive(false);
                m_IsIndicatorActive = false;
            }
            else
            {
                Vector3 targetViewportPosition = m_MainCamera.WorldToViewportPoint(m_Target.transform.position);

                if (targetViewportPosition.z > 0 && targetViewportPosition.x > 0 && targetViewportPosition.x < 1 && targetViewportPosition.y > 0 && targetViewportPosition.y < 1)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    Vector3 screenEdge = m_MainCamera.ViewportToWorldPoint(new Vector3(Mathf.Clamp(targetViewportPosition.x, 0.1f, 0.9f), Mathf.Clamp(targetViewportPosition.y, 0.1f, 0.9f), m_MainCamera.nearClipPlane));
                    transform.position = new Vector3(screenEdge.x, screenEdge.y, 0);
                    Vector3 direction = m_Target.transform.position - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0,0,angle);
                }
            }
        }
    }
}
