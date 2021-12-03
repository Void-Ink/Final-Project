using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public static int level = 1;

    public float speed = 3.0f;

    public int maxHealth = 5;

    public GameObject projectilePrefab;

    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip collectedClip;
    public AudioClip fixedSound;
    public AudioClip WinMusic;
    public AudioClip LoseMusic;

    public int score;
    public TMP_Text robotsFixed;

    public bool levelonedone;
    public GameObject nextMissionBox;
    public float displaytime2 = 5.0f;
    public float timerDisplay2;

    public GameObject gameOverBox;
    public bool GameOver;
    public TMP_Text GameOverText;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public int cogs;
    public TMP_Text cogCount;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    public ParticleSystem hurtEffect;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        score = 0;
        robotsFixed.text = "Robots Fixed: 0";

        cogs = 4;
        cogCount.text = "Cogs: 4";

        nextMissionBox.SetActive(false);

        gameOverBox.SetActive(false);
        GameOverText.text = "";

        levelonedone = false;
        GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null  && levelonedone == false)
                {
                    character.DisplayDialog();
                }
                else if (character != null && levelonedone == true)
                {
                    level = 2;
                    SceneManager.LoadScene("Level 2");
                    
                }
            }
        }

        if (timerDisplay2 >= 0)
        {
            timerDisplay2 -= Time.deltaTime;
            if (timerDisplay2 < 0)
            {   
                nextMissionBox.SetActive(false);
            }
        }

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(GameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    public void ChangeCogs(int cogAmount)
    {
        cogs = cogs + cogAmount;
        cogCount.text = "Cogs: " + cogs.ToString();
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            animator.SetTrigger("Hit");

            PlaySound(hitSound);
            Instantiate(hurtEffect, transform.position, transform.rotation);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if(currentHealth <= 0)
        {
            GameOver = true;
            audioSource.Stop();
            audioSource.clip = LoseMusic;
            audioSource.Play();
            speed = 0;
            gameOverBox.SetActive(true);
            GameOverText.text = "Game Over, You Lost!  Press R to try again.";
        }
    }

    public void ChangeScore(int scoreAmount)
    {
        if (scoreAmount >= 0)
        {
            PlaySound(fixedSound);
        }

        score = score + scoreAmount;
        robotsFixed.text = "Robots Fixed: " + score.ToString();

        if(score >= 4)
        {
            if (level == 1)
            {
                NextMission();
            }

            else if (level == 2)
            {
                GameOver = true;
                speed = 0;
                audioSource.Stop();
                audioSource.clip = WinMusic;
                audioSource.Play();
                gameOverBox.SetActive(true);
                GameOverText.text = "You win!  Game created by Dakota Robinson.  Press R to play again!";
            }
            
        }
    }

    public void Launch()
    {
        if(cogs == 0)
        {
            return;
        }

        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwSound);

        cogs = cogs - 1;
        cogCount.text = "Cogs: " + cogs.ToString();

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void NextMission()
    {
        levelonedone = true;
        timerDisplay2 = displaytime2;
        nextMissionBox.SetActive(true);
    }

}