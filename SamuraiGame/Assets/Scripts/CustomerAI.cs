using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAI : MonoBehaviour
{
    [SerializeField] public int ID;
    [SerializeField] private bool m_CanMove;
    [SerializeField] private bool m_Hostile;
    [SerializeField] private AIBrain m_ClosestAIBrain;
    [SerializeField] private OrderMaker m_OrderMaker;
    [SerializeField] private bool m_OrderMade;
    [SerializeField] private bool m_InBattle;
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_Damage;
    [SerializeField] private float m_AttackFrequency;
    [SerializeField] private float m_Health;
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float timer = 0f;
    private Vector3 position;

    public void OrderHandedToAI(Order order, Plate plate)
    {
        Debug.Log("OrderHandedToAI method called");

        // Check if m_OrderMaker is null
        if (m_OrderMaker == null)
        {
            Debug.LogError("m_OrderMaker is null!");
            return;
        }

        // Check if order or plate is null
        if (order == null || plate == null)
        {
            Debug.LogError("Order or plate is null!");
            return;
        }

        // If the given order matches the requested order
        if (m_OrderMaker.CheckOrder(order, plate))
        {
            m_Health = m_MaxHealth;
            m_OrderMade = false;
            m_OrderMaker.ClearCurrentOrder();
        }
    }

    public float GetCurrentHealth()
    {
        return m_Health;
    }

    public float GetMaxHealth()
    {
        return m_MaxHealth;
    }

    public bool CanMove()
    {
        return m_CanMove;
    }

    public bool GetOrderMade()
    {
        return m_OrderMade;
    }

    public void SetOrderMade(bool state)
    {
        m_OrderMade = state;
    }
}
