using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private List<RectTransform> m_DisplaySlots;

    private void Update()
    {
        OrderChildren();
    }

    public List<RectTransform> GetDisplaySlots()
    {
        return m_DisplaySlots;
    }

    private void OrderChildren()
    {
        // for every slot
        for (int i = 1; i < m_DisplaySlots.Count; i++)
        {
            // if the slot contains a child but the next slot doesnt
            if (m_DisplaySlots[i].childCount == 0 && m_DisplaySlots[i-1].childCount > 0)
            {
                // switch the slot inwhich the child is nested
                GameObject child = m_DisplaySlots[i-1].GetChild(0).gameObject;
                // detach child from initial parent
                m_DisplaySlots[i-1].DetachChildren();
                // attach it to the new one
                child.transform.SetParent(m_DisplaySlots[i]);
                // update position and rotation
                child.transform.position = m_DisplaySlots[i].position;
                child.transform.rotation = m_DisplaySlots[i].rotation;

            }
        }
    }
}