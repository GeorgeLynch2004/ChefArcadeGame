using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionApplication : Appliance
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            RemoveNewObject();
        }
    }

    public void RemoveNewObject()
    {
        GameObject objectToRemove = GetCurrentObject();
        m_AttachPoint.DetachChildren();
        objectToRemove.transform.SetParent(m_PreviousParent);
    }


}
