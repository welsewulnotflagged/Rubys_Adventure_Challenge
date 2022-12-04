using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public GameObject loseText;
    private RubyController controller;
    public AudioClip loseClip;
    private AudioSource player;
    bool loseChecker;


    void Start () 
    {
        
        controller = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<RubyController>();
        player = GetComponent<AudioSource>();
    }
    void Update() 
    {

        if (controller.currentHealth <= 0)
        {
            loseText.SetActive(true);
            //player.PlayOneShot(loseClip);
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Level 1");
            }

            if (!loseChecker) 
            {   
                player.clip = loseClip;
                player.Play();
                loseChecker = true;
                controller.enabled = false;
            }
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
    
        if (EnemyController.fixedRobots == 5)
        { 
            LoadGame();    
            EnemyController.fixedRobots = 0; 
        }
    
        
    }

    public void LoadGame() 
    {
        SceneManager.LoadScene("Level 2");
    }
}
