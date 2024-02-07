using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderReceiver : MonoBehaviour
{
    // requires access to order manager to get the current ongoing orders
    [SerializeField] private OrderManager m_OrderManager;
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private List<Order> m_CurrentOrders;


    // Start is called before the first frame update
    void Start()
    {
        m_OrderManager = GameObject.Find("OrderManager").GetComponent<OrderManager>();
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        m_CurrentOrders = m_OrderManager.GetCurrentOrders();
    }

    public void OrderHandedToReceiver(Order receievedOrder, Plate plate)
    {
        Debug.Log("OrderHandedToReceiver method called");

        // Check if order or plate is null
        if (receievedOrder == null || plate == null)
        {
            Debug.LogError("Order or plate is null!");
            return;
        }

        // If the given order matches the requested order
        foreach (Order order in m_CurrentOrders)
        {
            if (m_OrderManager.CheckOrderSimilarity(order, receievedOrder))
            {
                Debug.Log("This order was asked for!");
                m_OrderManager.RemoveCurrentOrders(order);
                m_GameManager.ChangeScore(+order.GetValue());
                m_GameManager.IncreaseOrdersCompletedCount();
                plate.ClearPlate();
            }
            else{
                Debug.Log("This order was not asked for" + order.ToString());
            }
        }
        
    }
}
