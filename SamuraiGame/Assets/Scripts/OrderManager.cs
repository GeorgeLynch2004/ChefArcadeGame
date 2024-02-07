using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private int m_MaxCurrentOrders;
    [SerializeField] private List<Order> m_OrderPool;
    [SerializeField] private List<Order> m_CurrentOrders;
    [SerializeField] private GameObject m_OrderDisplayUIPrefab;
    [SerializeField] private GameObject m_UICanvas;

    private void Start() 
    {
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update() 
    {
        if (!m_gameManager.GameRunning()) return;

        int gameDifficulty = m_gameManager.GetGameDifficulty();

        if (m_CurrentOrders.Count < gameDifficulty)
        {
            int randomNumber = Random.Range(1, 50);

            if (randomNumber == 1)
            {
                Order order = GetRandomOrder();
                if (!m_CurrentOrders.Contains(order))
                {
                    AddCurrentOrders(order);
                }
            }
        }
    }

    public Order GetRandomOrder()
    {
        int randomIndex = Random.Range(0, m_OrderPool.Count);
        return m_OrderPool[randomIndex];
    }

    public int GetMaxCurrentOrders()
    {
        return m_MaxCurrentOrders;
    }

    public void AddCurrentOrders(Order order)
    {
        m_CurrentOrders.Add(order);

        // Get the list of display slots
        List<RectTransform> displaySlots = m_UICanvas.GetComponent<UICanvas>().GetDisplaySlots();
        RectTransform orderAttachPoint = null;

        // find the first one that is free
        foreach (RectTransform slot in displaySlots)
        {
            if (slot.childCount == 0)
            {
                orderAttachPoint = slot;
            }
        }

        // Instantiate the new order display
        GameObject gameObject = Instantiate(m_OrderDisplayUIPrefab, orderAttachPoint.position, orderAttachPoint.rotation, orderAttachPoint);
        gameObject.GetComponent<OrderDisplayUI>().CreateOrderDisplayUI(order, 120 / m_gameManager.GetGameDifficulty());
    }

    public void RemoveCurrentOrders(Order targetOrder)
    {
        // take down the UI for the order
        RemoveOrderUI(targetOrder);

        // search through current orders to find the target order and remove it
        foreach (Order order in m_CurrentOrders)
        {
            if (CheckOrderSimilarity(order, targetOrder))
            {
                m_CurrentOrders.Remove(order);
                return;
            }
        }
        
    }

    public void RemoveOrderUI(Order orderToRemove)
    {
        Debug.Log("RemoveOrderUI method called");

        // iterate through each slot the UI can be displayed in
        foreach (RectTransform slot in m_UICanvas.GetComponent<UICanvas>().GetDisplaySlots())
        {
            // if a slot contains an order display within
            if (slot.childCount > 0)
            {
                Debug.Log("Found a child");
                // get the order display UI component of the child
                OrderDisplayUI orderDisplay = slot.GetChild(0).gameObject.GetComponent<OrderDisplayUI>();
                
                if (orderDisplay != null)
                {
                    Debug.Log("Order display isnt null");
                    // get the order being displayed within the UI object
                    Order displayedOrder = orderDisplay.GetOrderBeingDisplayed();
                    
                    if (CheckOrderSimilarity(displayedOrder, orderToRemove))
                    {
                        GameObject removeObject = slot.GetChild(0).gameObject;
                        Destroy(removeObject);
                        return;
                    }
                }
            }
        }
    }

    public bool CheckOrderSimilarity(Order order1, Order order2)
    {
        List<GameObject> order1Items = order1.GetOrderedObjects();
        List<GameObject> order2Items = order2.GetOrderedObjects();
        
        // check that both orders have the same amount of items
        if (order1Items.Count != order2Items.Count)
        {
            return false;
        }

        List<string> order1Strings = new List<string>(order1Items.Count);
        List<string> order2Strings = new List<string>(order2Items.Count);

        foreach (GameObject item in order1Items)
        {
            string name = item.GetComponent<FoodItem>().GetNameString();
            order1Strings.Add(name);
        }

        foreach (GameObject item in order2Items)
        {
            string name = item.GetComponent<FoodItem>().GetNameString();
            order2Strings.Add(name);
        }

        order1Strings.Sort();
        order2Strings.Sort();

        Debug.Log(order1Strings);
        Debug.Log(order2Strings);

        for (int i = 0; i < order1Strings.Count; i++)
        {
            if (order1Strings[i] != order2Strings[i])
            {
                return false;
            }
        }

        return true;

    }

    public List<Order> GetCurrentOrders()
    {
        return m_CurrentOrders;
    }
}
