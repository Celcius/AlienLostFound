using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GrabbableArea : MonoBehaviour
{
    private Collider2D area;
    public Bounds bounds => area.bounds;
    
    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<Collider2D>();
    }
}
