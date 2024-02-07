using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class FoodItem : MonoBehaviour
{ 
    public enum PreperationMethod{
        None,
        Fry,
        Cook,
        Boil,
        Chop,
        Wash
    }

    [SerializeField] public PreperationMethod m_PreperationMethod;
    [SerializeField] private float m_TimeToPrep;
    [SerializeField] private string m_NameString;
    [SerializeField] private GameObject m_PreppedObject;
    [SerializeField] private bool m_IsPlated;

    private void Start() 
    {
        m_IsPlated = false;
    }

    public float GetPrepTime()
    {
        return m_TimeToPrep;
    }

    public string GetNameString()
    {
        return m_NameString;
    }

    public bool GetIsPlated()
    {
        return m_IsPlated;
    }

    public void SetIsPlated(bool state)
    {
        m_IsPlated = state;
    }

    public void OnPrepped(Transform attachPoint)
    {
        if (m_PreppedObject != null)
        {
            Instantiate(m_PreppedObject, transform.position, transform.rotation, attachPoint);
        }
        Destroy(gameObject);
    }

}
