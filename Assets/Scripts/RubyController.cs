using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubyController : MonoBehaviour
{
    private const string N = "walkingAudio";
    public float speed = 3.0f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    public int health { get { return currentHealth; } }
    int currentHealth;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;

    AudioSource characterAudio;
    
    public AudioClip cogThrowClip;
    public AudioClip playerHitClip;
    public AudioClip playerRunning;
    bool startedRunning;
    bool isRunning;

    public GameObject walkingAudioObject;
    AudioSource runningAudio;

    public Image damageImage;
    public Color damageFlashColor;
    public float damageFlashSpeed = 0.5f;
    bool characterDamaged = false;
    bool characterHealed = false;

    public Image healImage;
    public Color healFlashColor;
    public float healFlashSpeed = 0.5f;

    public GameObject GameOverMenu;
    bool gameOverState = false;
    float horizontal;
    float vertical;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        characterAudio = GetComponent<AudioSource>();

        walkingAudioObject = transform.Find(N).gameObject;
        runningAudio = walkingAudioObject.GetComponent<AudioSource>();
    }

    public void characterOneShot(AudioClip clip)
    {
        characterAudio.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOverState)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");


            Vector2 position = rigidbody2d.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;

            Vector2 move = new Vector2(horizontal, vertical);


            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
                startedRunning = true;
            }
            else if (Mathf.Approximately(move.x, 0.0f) || Mathf.Approximately(move.y, 0.0f))
            {
                isRunning = false;
                startedRunning = false;
                runningAudio.Stop();
            }

            if (startedRunning && !isRunning)
            {
                isRunning = true;
                runningAudio.Play();
            }


            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);

            position = position + move * speed * Time.deltaTime;

            rigidbody2d.MovePosition(position);

            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 0)
                    isInvincible = false;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Launch();
                characterOneShot(cogThrowClip);
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

            DamagaeFlash(false);
            HealFlash(false);
        }
    }

    public void ChangeHealth(int amount)
    {
        if (!gameOverState)
        {
            if (amount < 0)
            {
                if (isInvincible)
                    return;

                isInvincible = true;
                invincibleTimer = timeInvincible;
            }

            if (amount < 0)
            {
                characterOneShot(playerHitClip);
                characterDamaged = true;
            }
            else if (amount > 0)
            {
                characterHealed = true;
            }

            DamagaeFlash(characterDamaged);
            HealFlash(characterHealed);

            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

            if (currentHealth == 0)
            {
                GameOverMenu.SetActive(true);
                gameOverState = true;
            }
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 400);

        animator.SetTrigger("Launch");
    }

    void DamagaeFlash(bool damaged)
    {
        if (damaged)
        {
            damageImage.color = damageFlashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, Time.deltaTime / damageFlashSpeed);
        }

        characterDamaged = false;
    }

    void HealFlash(bool healed)
    {
        if (healed)
        {
            healImage.color = healFlashColor;
        }
        else
        {
            healImage.color = Color.Lerp(healImage.color, Color.clear, Time.deltaTime / healFlashSpeed);
        }

        characterHealed= false;
    }
}
