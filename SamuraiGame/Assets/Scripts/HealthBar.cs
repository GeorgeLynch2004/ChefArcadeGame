using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private AIBrain m_AIBrain;
    [SerializeField] private float m_PercentageHealth;
    [SerializeField] private Transform m_HealthBar;

    private void Update() 
    {
        m_PercentageHealth = GetPercentageHealth();

        Vector3 scale = new Vector3(GetPercentageHealth(), 1, 1);
        m_HealthBar.localScale = scale;
    }

    private float GetPercentageHealth()
    {
        return (m_AIBrain.GetCurrentHealth() / m_AIBrain.GetMaxHealth());
    }
}
