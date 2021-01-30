using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryArea : MonoBehaviour
{
    Collider2D col;
    HashSet<GrabbableItem> items = new HashSet<GrabbableItem>();
    public GrabbableItem[] Items => new List<GrabbableItem>(items).ToArray();
    
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    public void ClearArea()
    {
        items.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        GrabbableItem item = other.GetComponent<GrabbableItem>();
        if(item != null)
        {
            items.Add(item);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        GrabbableItem item = other.GetComponent<GrabbableItem>();
        if(item != null && items.Contains(item))
        {
            items.Remove(item);
        }
    }

    public void Remove(GrabbableItem item)
    {
        items.Remove(item);
    }

    public bool Contains(GrabbableItem item)
    {
        if(item == null)
        {
            return false;
        }
        return items.Contains(item);
    }
}
