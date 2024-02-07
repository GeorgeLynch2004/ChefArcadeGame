using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Order m_OrderBeingCarried;
    [SerializeField] private List<Transform> m_AttachPoints;
    [SerializeField] private bool m_IsDirty;
    [SerializeField] private GameObject m_DirtyPlatePrefab;

    public void CreateNewOrder()
    {
        m_OrderBeingCarried = Order.CreateInstance<Order>();
    }

    public void AddToPlate(GameObject gameObject, Transform previousParent)
    {
        // Check if there is an existing order
        if (m_OrderBeingCarried == null)
        {
            CreateNewOrder();
        }
        // Add the gameObject to the order
        m_OrderBeingCarried.AddToOrder(gameObject);
        // Add the gameObject to the attach points
        foreach (Transform AttachPoint in m_AttachPoints)
        {
            if (AttachPoint.childCount == 0)
            {
                previousParent.DetachChildren();
                gameObject.transform.position = AttachPoint.position;
                gameObject.transform.SetParent(AttachPoint);
                gameObject.GetComponent<FoodItem>().SetIsPlated(true);
                return;
            }
        }
    }

    public Order GetOrderFromPlate()
    {
        if (m_OrderBeingCarried != null)
        {
            return m_OrderBeingCarried;
        }
        else return null;
    }

    public void ClearPlate()
    {
        Instantiate(m_DirtyPlatePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public bool IsDirty()
    {
        return m_IsDirty;
    }
}
