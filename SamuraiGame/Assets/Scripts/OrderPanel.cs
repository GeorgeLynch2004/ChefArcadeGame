using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPanel : MonoBehaviour
{
    [SerializeField] private List<Transform> m_AttachPoints;
    [SerializeField] private OrderMaker m_OrderMaker;
    [SerializeField] private GameObject m_ItemDisplayPrefab;

    public void DisplayCurrentOrder(Order currentOrder)
    {
        List<GameObject> requiredItems = currentOrder.GetOrderedObjects();

        for (int i = 0; i < requiredItems.Count; i++)
        {
            Sprite itemSprite = requiredItems[i].GetComponent<SpriteRenderer>().sprite;
            string itemName = requiredItems[i].GetComponent<FoodItem>().GetNameString();

            if (m_AttachPoints[i].childCount == 0)
            {
                GameObject itemDisplayObject = Instantiate(m_ItemDisplayPrefab, m_AttachPoints[i].position, m_AttachPoints[i].rotation, m_AttachPoints[i]);
                ItemDisplay itemDisplay = itemDisplayObject.GetComponent<ItemDisplay>();
                itemDisplay.SetImageOfItem(itemSprite);
                itemDisplay.SetNameOfItem(itemName);
            }
        }
    }

    public void ClearCurrentOrder()
    {
        foreach (Transform attachPoint in m_AttachPoints)
        {
            if (attachPoint.childCount > 0)
            {
                GameObject child = attachPoint.GetChild(0).gameObject;
                Destroy(child);
            }
        }
    }

}
