using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


﻿public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    
    public int maxHealth = 5;
    
    public GameObject projectilePrefab;
    public AudioClip audioClipThrow;
    public AudioClip audioClipHit;
    public ParticleSystem onHitEffect;
    public ParticleSystem onPickUpEffect;

    
    public int health { get { return currentHealth; }}
    public int currentHealth;
    int currentAmmo;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    AudioSource audioSource;
    public Text ammoCount;
     
    
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        onHitEffect.Stop();
        onPickUpEffect.Stop();

        
        currentHealth = maxHealth;
        currentAmmo = 4; 
        ammoCount.text = "Ammo: " + currentAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (currentAmmo >= 1) {
            Launch();
            ChangeAmmo(-1);
            }
            else 
            {
                ammoCount.text = "No ammo left!";
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
                
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();
                    }  
                    
            }
        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(audioClipHit);
            onHitEffect.Play();
            animator.SetTrigger("Hit");

        }
        

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    public void ChangeAmmo(int ammo)
    {
        currentAmmo = currentAmmo + ammo;
        ammoCount.text = "Ammo: " + currentAmmo;
    }

     public void ChangeSpeed(float newSpeed, int duration) 
    {
        StartCoroutine(Countdown(newSpeed, duration));
    }

    public IEnumerator Countdown(float newSpeed, int duration) 
    {

        float oldSpeed = speed;
        speed = newSpeed;
        yield return new WaitForSeconds(duration);  
        speed = oldSpeed;
    }
    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        ProjectileController projectile = projectileObject.GetComponent<ProjectileController>();
        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");
        PlaySound(audioClipThrow);
        
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}