using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class OrderDisplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Timer;
    [SerializeField] private List<Image> m_Images;
    [SerializeField] private OrderManager m_OrderManager;
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private Order m_CurrentOrder;
    [SerializeField] private float timer;

    private void Start() 
    {
        m_OrderManager = GameObject.Find("OrderManager").GetComponent<OrderManager>();
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update() {

        if (!m_GameManager.GameRunning()) return;

        m_Timer.text = Math.Round(timer).ToString();
        
        if (timer <= 0f)
        {
            m_GameManager.ChangeScore(-m_CurrentOrder.GetValue());
            m_OrderManager.RemoveCurrentOrders(m_CurrentOrder);
        }

        timer -= Time.deltaTime;
    }

    public void CreateOrderDisplayUI(Order order, float timeBeforeFailure)
    {
        List<GameObject> orderedObjects = order.GetOrderedObjects();
        m_CurrentOrder = order;
        timer = timeBeforeFailure;

        for (int i = 0; i < orderedObjects.Count; i++)
        {
            Sprite image = orderedObjects[i].GetComponent<SpriteRenderer>().sprite;
            m_Images[i].sprite = image;
            m_Timer.text = Math.Round(timer).ToString();
        }
    }

    public Order GetOrderBeingDisplayed()
    {
        return m_CurrentOrder;
    }

}
