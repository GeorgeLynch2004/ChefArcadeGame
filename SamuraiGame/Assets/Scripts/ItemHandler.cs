using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] private Transform m_AttachPoint;
    [SerializeField] private bool m_HandsFull;
    [SerializeField] private List<GameObject> m_ActiveCollisions;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // Check if hands are full
        AreHandsFull();  

        // Check for input
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!m_HandsFull)
            {
                // Next check for appliances
                GameObject containsAppliance = m_ActiveCollisions.Find(go => go.CompareTag("Appliance"));
                // Prioritise Items in the list over item containers
                GameObject containsItem = m_ActiveCollisions.Find(go => go.CompareTag("Item"));
                // Next check for cupboards
                GameObject containsCupboard = m_ActiveCollisions.Find(go => go.CompareTag("Cupboard"));
                // Next check for plates
                GameObject containsPlate = m_ActiveCollisions.Find(go => go.CompareTag("Plate"));
                if (containsAppliance != null)
                {
                    containsAppliance.GetComponent<Appliance>().TakeCurrentObject(m_AttachPoint);
                }
                else if ((containsItem != null) && (containsItem.GetComponent<FoodItem>().GetIsPlated() == false))
                {
                    GrabContentsFromObject(containsItem);
                }
                else if (containsCupboard != null)
                {
                    GrabContentsFromObject(containsCupboard);
                }
                else if (containsPlate != null)
                {
                    GrabContentsFromObject(containsPlate);
                }
            }
            else
            {
                // Check for appliances nearby to drop the item onto
                GameObject containsAppliance = m_ActiveCollisions.Find(go => go.CompareTag("Appliance"));
                // Check for plates nearby to drop the item onto
                GameObject containsPlate = m_ActiveCollisions.Find(go => go.CompareTag("Plate"));
                // Check for AI nearby to feed the plate to
                GameObject containsAI = m_ActiveCollisions.Find(go => go.CompareTag("AI"));
                // Check for Order Receivers to hand the order to
                GameObject containsReceiver = m_ActiveCollisions.Find(go => go.CompareTag("Receiver"));
                // Check for bins to bin plate into
                GameObject containsBin = m_ActiveCollisions.Find(go => go.CompareTag("Rubbish Bin"));

                // Get the object we want to attach. AKA food item being held
                GameObject objectToAttach = m_AttachPoint.transform.GetChild(0).gameObject;

                if (containsAppliance != null)
                {
                    containsAppliance.GetComponent<Appliance>().AttachNewObject(objectToAttach, m_AttachPoint);
                }
                else if (containsPlate != null)
                {
                    if (!containsPlate.GetComponent<Plate>().IsDirty() && objectToAttach.GetComponent<Plate>() == null)
                    {
                        containsPlate.GetComponent<Plate>().AddToPlate(objectToAttach, m_AttachPoint);
                    }
                }
                else if (containsAI != null)
                {
                    if (objectToAttach.GetComponent<Plate>() != null)
                    {
                        containsAI.GetComponent<AIBrain>().OrderHandedToAI(objectToAttach.GetComponent<Plate>().GetOrderFromPlate(), objectToAttach.GetComponent<Plate>());
                    }
                }
                else if (containsReceiver != null)
                {
                    if (objectToAttach.GetComponent<Plate>() != null)
                    {
                        containsReceiver.GetComponent<OrderReceiver>().OrderHandedToReceiver(objectToAttach.GetComponent<Plate>().GetOrderFromPlate(), objectToAttach.GetComponent<Plate>());
                    }
                }
                else if (containsBin != null)
                {
                    if (objectToAttach.GetComponent<Plate>() != null && objectToAttach.GetComponent<Plate>().GetOrderFromPlate() != null)
                    {
                        objectToAttach.GetComponent<Plate>().ClearPlate();
                    }
                }
                else
                {
                    // drop on the floor
                    DropContentsFromHand();
                }
                
            }
        }  
    }

    private void GrabContentsFromObject(GameObject other)
    {   
        // if interaction is with an item already spawned
        if ((other.gameObject.tag == "Item") || (other.gameObject.tag == "Plate"))
        {
            // set the position to 0,0
            other.gameObject.transform.position = m_AttachPoint.position;
            // attach it to the attach point by setting it as its parent
            other.gameObject.transform.SetParent(m_AttachPoint);
            return;
        }

        GameObject contents = new GameObject();

        // if interaction is with a cupboard
        if (other.gameObject.tag == "Cupboard")
        {
            // Get what the cupboard contains
            contents = other.gameObject.GetComponent<FoodSpawner>().AccessContents();
        }

        Instantiate(contents, m_AttachPoint);
    }

    private void DropContentsFromHand()
    {
        if (m_HandsFull)
        {
            GameObject gameObject = m_AttachPoint.GetChild(0).gameObject;
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
            m_AttachPoint.DetachChildren();
        }
    }

    private void AreHandsFull()
    {
        if (m_AttachPoint.childCount > 0)
        {
            m_HandsFull = true;
        }
        else m_HandsFull = false;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {   
        if (!m_ActiveCollisions.Contains(other.gameObject))
        {
            m_ActiveCollisions.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        m_ActiveCollisions.Remove(other.gameObject);
    }

}
