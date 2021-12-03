using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public AudioClip collectedClip;
    public ParticleSystem healEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                Instantiate(healEffect, transform.position,transform.rotation);
                Destroy(gameObject);

                controller.PlaySound(collectedClip);
            }
        }
    }
}
