using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appliance : MonoBehaviour
{
    private enum PreperationFunction{
        None,
        Fry,
        Cook,
        Boil,
        Chop,
        Wash
    }

    [SerializeField] private PreperationFunction m_PreperationFunction;
    [SerializeField] private GameObject m_CurrentObject;
    [SerializeField] public Transform m_AttachPoint;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Animator m_PotnPanAnimator;
    public Transform m_PreviousParent;

    private void Update()
    {
        m_CurrentObject = GetCurrentObject();
    }

    public GameObject GetCurrentObject()
    {
        // Check if there are any children under m_AttachPoint
        if (m_AttachPoint.childCount > 0)
        {
            return m_AttachPoint.GetChild(0).gameObject;
        }
        else
        {
            return null;
        }
    }
    private void SetCurrentObject(GameObject gameObject)
    {
        m_CurrentObject = gameObject;
    }

    public void TakeCurrentObject(Transform newAttachPoint)
    {
        m_CurrentObject.transform.SetParent(newAttachPoint);
        m_CurrentObject.transform.position = newAttachPoint.position;
        m_CurrentObject.transform.rotation = newAttachPoint.rotation;
        m_CurrentObject.transform.localScale = newAttachPoint.localScale;
        m_AttachPoint.transform.DetachChildren();
        
    }

    public void AttachNewObject(GameObject newObject, Transform previousParent)
    {
        if (newObject.GetComponent<FoodItem>().m_PreperationMethod.ToString() != m_PreperationFunction.ToString())
        {
            return;
        }
        m_PreviousParent = previousParent;
        previousParent.DetachChildren();
        // if there is already an object then pull a switcheroni
        if (GetCurrentObject() != null)
        {
            // get current slot object
            GameObject gameObject = GetCurrentObject();
            // detach from the attach point
            m_AttachPoint.DetachChildren();
            // attach it to the player
            gameObject.transform.position = previousParent.position;
            gameObject.transform.rotation = previousParent.rotation;
            gameObject.transform.localScale = previousParent.localScale;
            gameObject.transform.SetParent(previousParent);
        }

        // attach the new object
        newObject.transform.position = m_AttachPoint.position;
        newObject.transform.SetParent(m_AttachPoint);

        // start the preperation
        StartCoroutine(FoodPreperation(GetCurrentObject().GetComponent<FoodItem>().GetPrepTime()));
    }

    private IEnumerator FoodPreperation(float PreperationTime)
    {
        // play preperation audio and animation
        m_Animator.SetTrigger("Working");
        if (m_PotnPanAnimator != null)
        {
            m_PotnPanAnimator.SetTrigger("Working");
        }

        yield return new WaitForSeconds(PreperationTime);

        // if the object has not been removed from the appliance
        if (GetCurrentObject() != null)
        {
            // set it to prepped
            GetCurrentObject().GetComponent<FoodItem>().OnPrepped(m_AttachPoint);
        }

        m_Animator.SetTrigger("Idle"); 

        if (m_PotnPanAnimator != null)
        {
            m_PotnPanAnimator.SetTrigger("Idle"); 
        }
              
    }
}
