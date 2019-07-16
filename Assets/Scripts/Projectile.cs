using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2d;

    AudioSource projectileAudio;
    public AudioClip cogCollideClip_01;
    public AudioClip cogCollideClip_02;
    bool selfDestruct = false;
    bool selfDestructing = false;
    bool disableCollider2D = false;
    float timeToDestroy = 2.0f;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        projectileAudio = GetComponent<AudioSource>();
        projectileAudio.volume = 0.35f;
        projectileAudio.Play();
    }

    private void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            selfDestruct = true;
        }
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        if (selfDestruct && !selfDestructing)
        {
            selfDestructing = true;
        }
        if (selfDestruct && selfDestructing)
        {
            timeToDestroy = timeToDestroy - Time.deltaTime;
            if (timeToDestroy < 1)
            {
                GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, Color.clear, Time.deltaTime / timeToDestroy);
            }      
        }
        if(timeToDestroy < 0)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (Random.Range(0, 2) == 0)
        {
            projectileAudio.PlayOneShot(cogCollideClip_01);
        }
        else
        {
            projectileAudio.PlayOneShot(cogCollideClip_02);
        }

        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }

        selfDestruct = true;
        DestroyProjectile();
    }
}
