using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_Prefab;
    [SerializeField] private bool m_LimitedSupply;
    [SerializeField] private int m_Stock;   

    public GameObject AccessContents()
    {
        if (m_LimitedSupply)
        {
            if (m_Stock > 0)
            {
                m_Stock -= 1;
                return m_Prefab;
            }
            return null;
        }
        return m_Prefab;
    }
}
 