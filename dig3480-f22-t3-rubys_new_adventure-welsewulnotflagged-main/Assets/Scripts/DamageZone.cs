using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{    

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("OnCollisionEnter2D");
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
}
