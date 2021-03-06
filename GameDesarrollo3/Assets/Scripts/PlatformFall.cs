﻿using UnityEngine;
using System.Collections;

public class PlatformFall : MonoBehaviour
{

    public float fallDelay = 1f;
    public float DisableColliderDelay = 3f;
    private Rigidbody2D rb2d;
    private Collider2D col2d;
    private Vector2 originalPosition;
    [SerializeField] float _timeToReset;
    [SerializeField] AudioClip crackSound;
    GameObject ball;

    AudioSource audioS;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        originalPosition = this.transform.position;
        audioS = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            audioS.PlayOneShot(crackSound, 1F);
            Invoke("Fall", fallDelay);
        }
      
    }    

    void Fall()
    {
        if(ball)
            ball.transform.parent = null;

        rb2d.bodyType = RigidbodyType2D.Dynamic;      
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        this.transform.Find("FallChild").gameObject.GetComponent<Collider2D>().enabled = false;       
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(ExecuteAfterTime(_timeToReset));
    }

    public void ResetPosition()
    {
        CancelInvoke();      
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        this.transform.Find("FallChild").gameObject.GetComponent<Collider2D>().enabled = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;               
        rb2d.velocity = Vector2.zero;
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        this.transform.position = originalPosition;        
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        // Code to execute after the delay
        ResetPosition();
    }
}