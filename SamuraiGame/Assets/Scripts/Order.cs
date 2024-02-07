using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "Order")]
public class Order : ScriptableObject
{
    [SerializeField] public int ID;
    [SerializeField] private float m_Value;
    [SerializeField] List<GameObject> m_OrderedObjects = new List<GameObject>();

    public void AddToOrder(GameObject gameObject)
    {
        m_OrderedObjects.Add(gameObject);
    }

    public float GetValue()
    {
        return m_Value;
    }


    public List<GameObject> GetOrderedObjects()
    {
        return m_OrderedObjects;
    }
}
