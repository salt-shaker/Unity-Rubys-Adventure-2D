using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;

    Animator animator;

    public ParticleSystem smokeEffect;

    public GameObject cogCollision;
    AudioSource enemeyAudio;
    public AudioClip fixAudioClip;

    public GameObject questObject;

    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        enemeyAudio = GetComponent<AudioSource>();

        questObject = GameObject.Find("Quests");
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
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
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        GameObject cogCollisionObject = Instantiate(cogCollision, rigidbody2D.position, Quaternion.identity);

        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        enemeyAudio.Stop();
        enemyOneShot(fixAudioClip);
        questObject.GetComponent<Quests>().BotFixed();
    }

    public void enemyOneShot(AudioClip clip)
    {
        enemeyAudio.PlayOneShot(clip);
    }
}