using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


﻿public class EnemyController : MonoBehaviour
{
 public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public ParticleSystem smokeEffect;
    
    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    public int changerHealth;
    bool broken = true;
    public static int fixedRobots = 0;
    
    public AudioClip winAudioClip;
    AudioSource winAudioSource;
    private RubyController winController;

    
    Animator animator;
    public Text fixedRobotsCount;
    public GameObject levelTextObject;
    
    // Start is called before the first frame update
    void Start()
    {
        winAudioSource = GetComponent<AudioSource>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        levelTextObject.SetActive(false);
        winController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<RubyController>();
        //Debug.Log(fixedRobotsCount);

    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    
    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        Vector2 position = rigidbody2D.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody2D.MovePosition(position);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        { 
            if (this.gameObject.tag == "Enemy") 
            {
                player.ChangeHealth(-1); 
            }
            
            if (this.gameObject.tag == "EnemyStrong") 
            {
                player.ChangeHealth(-changerHealth); 
            }
           
        }
    }
    
    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {   
        //Debug.Log("fix");
        broken = false;
        rigidbody2D.simulated = false;

        //optional if you added the fixed animation
        animator.SetTrigger("Fixed");
        fixedRobots++;
        fixedRobotsCount.text = "Fixed Robots: " + fixedRobots.ToString() + "/5";
        smokeEffect.Stop();

        if (fixedRobots == 5) 
        {   
            levelTextObject.SetActive(true);
            winAudioSource.clip = winAudioClip;
            winAudioSource.Play();
            winController.enabled = false;
        }
    }

}