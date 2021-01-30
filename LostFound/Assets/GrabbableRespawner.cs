using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;

public class GrabbableRespawner : MonoBehaviour
{
    [SerializeField]
    private Collider2D respawnArea;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        GrabbableItem item = col.collider.GetComponent<GrabbableItem>();
        if(item != null)
        {
            item.transform.position = Vector3.up * item.transform.position.z 
                + (Vector3)GeometryUtils.RandomPointInBounds(respawnArea.bounds);
        }
    }

}
