using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class OrderMaker : MonoBehaviour
{
    [SerializeField] private Order m_CurrentOrder;
    [SerializeField] private OrderManager m_OrderManager;
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private AIBrain m_AIBrain;
    [SerializeField] private OrderPanel m_OrderPanel;

    private void Start() {
        m_OrderManager = GameObject.Find("OrderManager").GetComponent<OrderManager>();
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_OrderPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!m_GameManager.GameRunning()) return;

        // if the ai is in a stationary level
        if (!m_AIBrain.CanMove())
        {
            // Get the current orders
            List<Order> currentOrders = m_OrderManager.GetCurrentOrders();

            if (currentOrders.Count < m_OrderManager.GetMaxCurrentOrders() && m_AIBrain.GetOrderMade() == false)
            {
                GenerateOrder();
                m_AIBrain.SetOrderMade(true);
            }
        }
    }

    public void GenerateOrder()
    {
        m_CurrentOrder = m_OrderManager.GetRandomOrder();
        m_OrderManager.AddCurrentOrders(m_CurrentOrder);
        m_OrderPanel.gameObject.SetActive(true);
        m_OrderPanel.DisplayCurrentOrder(m_CurrentOrder);
    }

    public void ClearCurrentOrder()
    {
        Debug.Log("ClearCurrentOrder method called");

        // Check if m_OrderManager is null
        if (m_OrderManager == null)
        {
            Debug.LogError("m_OrderManager is null!");
            return;
        }

        // Check if m_CurrentOrder is null
        if (m_CurrentOrder == null)
        {
            Debug.LogError("m_CurrentOrder is null!");
            return;
        }

        // Clear the current order
        Order order = m_CurrentOrder;
        m_OrderManager.RemoveCurrentOrders(order);
        m_CurrentOrder = null;
    }

    public bool CheckOrder(Order receivedOrder, Plate plate)
    {
        List<GameObject> receivedItems = receivedOrder.GetOrderedObjects();
        List<GameObject> requestedItems = m_CurrentOrder.GetOrderedObjects();

        string[] receivedItemNames = new string[receivedItems.Count];
        string[] requestedItemNames = new string[requestedItems.Count];

        if (receivedItemNames.Length != requestedItemNames.Length)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < requestedItems.Count; i++)
            {
                if (!requestedItemNames.Contains(receivedItemNames[i]))
                {
                    return false;
                }
            }
        }
        plate.ClearPlate();
        m_OrderPanel.ClearCurrentOrder();
        m_OrderPanel.gameObject.SetActive(false);
        return true;
    }

    public Order GetCurrentOrder()
    {
        return m_CurrentOrder;
    }
}
