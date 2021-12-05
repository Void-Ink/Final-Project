using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sidequest : MonoBehaviour
{
    
    public AudioClip collectedClip;
    public ParticleSystem healEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if(controller != null)
        {
            controller.SidequestController(1);
            Instantiate(healEffect, transform.position,transform.rotation);
            Destroy(gameObject);

            controller.PlaySound (collectedClip);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
