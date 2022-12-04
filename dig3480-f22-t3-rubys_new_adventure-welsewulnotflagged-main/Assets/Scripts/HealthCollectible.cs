using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{

    public AudioClip collectedClip;
    public ParticleSystem onPickUpEffectParticle;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other) 
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {   
            
            if (this.gameObject.tag == "HealthPickup") {
            
                if(controller.currentHealth < controller.maxHealth)
          {
	       controller.ChangeHealth(1);
	       Destroy(gameObject);
           controller.PlaySound(collectedClip);
           onPickUpEffectParticle.Play();
           
          }
        }

            if (this.gameObject.tag == "AmmoPickup") 
            {
                controller.ChangeAmmo(4);
                Destroy(gameObject); 
                controller.PlaySound(collectedClip);
                onPickUpEffectParticle.Play();


            }

            if (this.gameObject.tag == "SpeedPickup") 
            {
                controller.ChangeSpeed(5.5f, 3);
                Destroy(gameObject); 
                controller.PlaySound(collectedClip);
                onPickUpEffectParticle.Play();


            }

    }
}
}