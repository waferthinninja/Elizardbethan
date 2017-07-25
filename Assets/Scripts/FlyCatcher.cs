using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCatcher : MonoBehaviour
{
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Fly")
        {
            // kill the fly and stick it to the tongue
            FlyController fc = other.gameObject.GetComponent<FlyController>();
            fc.Die();
            fc.transform.SetParent(this.transform);            
        }

    }
}
