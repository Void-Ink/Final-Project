using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickup : MonoBehaviour
{

    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if(controller != null)
        {
            controller.ChangeCogs(4);
            Destroy(gameObject);

            controller.PlaySound (collectedClip);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

