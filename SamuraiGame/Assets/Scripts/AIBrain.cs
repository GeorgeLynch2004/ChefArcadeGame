using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class AIBrain : MonoBehaviour
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

    private void Start() 
    {
        m_Health = m_MaxHealth;
    }

    private void Update() 
    {
        // Search for enemies
        if (m_ClosestAIBrain == null)
        {
            SearchForOpposition();
        }
        
        if (m_ClosestAIBrain != null)
        {
            Vector3 direction = m_ClosestAIBrain.transform.position - transform.position;
            float distance = Vector3.Distance(transform.position, m_ClosestAIBrain.transform.position);
            if (m_CanMove)
            {
                if (distance > 2)
                {
                    
                }
                else
                {
                    m_InBattle = true;
                    timer += Time.deltaTime;

                    if (timer >= m_AttackFrequency)
                    {
                        timer = 0f;
                        StrikeOpponent();
                    }
                } 
            }
            

        }
        else
        {
            m_InBattle = false;
        }

        // If the AI has no health then it must die
        if (m_Health <= 0)
        {
            Destroy(gameObject);
        }

        // If the AI has taken damage it will request a new order of food to heal itself
        if (m_OrderMaker != null)
        {
            if ((m_Health <= m_MaxHealth * .75f) && !m_OrderMade)
            {
                m_OrderMaker.GenerateOrder();
                m_OrderMade = true;
            }
        }
        
        position = transform.position;
    }

    private void SearchForOpposition()
    {
        AIBrain[] aiBrains = FindObjectsOfType<AIBrain>();

        if (aiBrains.Length == 0)
        {
            return;
        }

        float closestDistance = float.MaxValue;
        AIBrain closestBrain = null;

        foreach (AIBrain aiBrain in aiBrains)
        {
            float distance = Vector3.Distance(transform.position, aiBrain.transform.position);

            if ((distance < closestDistance) && (aiBrain.IsHostile() != m_Hostile))
            {
                closestDistance = distance;
                closestBrain = aiBrain;
            }
        }

        m_ClosestAIBrain = closestBrain;
    }

    // private void MoveTowardsOpposition()
    // {
    //     if (m_ClosestAIBrain != null)
    //     {
    //         Vector3 direction = m_ClosestAIBrain.transform.position - transform.position;
    //         float distance = Vector3.Distance(transform.position, m_ClosestAIBrain.transform.position);
    //         if (distance > 2)
    //         {
    //             direction.Normalize();
    //             transform.Translate(direction * m_MovementSpeed * Time.deltaTime);
    //         }
    //         else
    //         {
    //             m_InBattle = true;
    //         }

    //     }
    //     else
    //     {
    //         m_InBattle = false;
    //     }
    // }

    private void StrikeOpponent()
    {
        if (m_ClosestAIBrain != null)
        {
            m_ClosestAIBrain.ChangeHealth(-m_Damage);
        }
    }

    public void ChangeHealth(float healthChange)
    {
        m_Health += healthChange;
    }

    public bool IsHostile()
    {
        return m_Hostile;
    }

    public bool IsInBattle()
    {
        return m_InBattle;
    }

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
