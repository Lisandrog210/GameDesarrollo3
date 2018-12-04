﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    [SerializeField] private float jumpForce = 24.0f;
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 19.0f;
    private bool isGrounded;
    public static BallMove instance;
    private bool moving;
    private float colAngle;
    private float numberOfBounces;
    public bool jumpAvailable;
    Vector2 lastContactPos = new Vector2();
    GameObject pausePanel;    

    public static BallMove Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BallMove>();
            }
            return instance;
        }
    }

    void Awake()
    {
        jumpAvailable = true;
        rb = GetComponent<Rigidbody2D>();
        instance = this;
        numberOfBounces = 1;              
        pausePanel = GameObject.FindGameObjectWithTag("PausePanel");
        
        
    }

    void Update()
    {
        if (InputManager.Instance.GetJumpButton() == true &&
            /*isGrounded == true*/  jumpAvailable == true &&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {           
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpAvailable = false;
            //this.transform.SetParent(null);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("On application pause");
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void FixedUpdate()
    {
        //MOVIMIENTO izq-derecha// Si esta en el aire el movimiento es ínfimo----------------------
        this.transform.rotation = Quaternion.identity;

        if (isGrounded == true)
        {
            rb.AddForce(Vector2.right * InputManager.Instance.GetHorizontalAxis() * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.right * InputManager.Instance.GetHorizontalAxis() * moveSpeed / 4 * Time.deltaTime, ForceMode2D.Impulse);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            //isGrounded = true;
            this.transform.SetParent(collision.transform);
            numberOfBounces = 1;
            jumpAvailable = true;
            //lastContactPos = collision.contacts[0].point;
        }
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("FallingPlatforms"))
        {
            //isGrounded = true;
            numberOfBounces = 1;
            jumpAvailable = true;
            //lastContactPos = collision.contacts[0].point;
        }
        if (collision.collider.gameObject.tag == "FastPlatform")
        {
            rb.AddForce(Vector2.right * moveSpeed * 1f, ForceMode2D.Impulse);
            numberOfBounces = 1;
            jumpAvailable = true;
            //lastContactPos = collision.contacts[0].point;
        }
        if (collision.collider.gameObject.tag == "FastPlatformx2")
        {
            rb.AddForce(Vector2.right * moveSpeed * 8f, ForceMode2D.Impulse);
            numberOfBounces = 1;
            jumpAvailable = true;
            //lastContactPos = collision.contacts[0].point;
        }
        if (collision.collider.gameObject.tag == "MovingPlatform")
        {
            FrictionJoint2D rb2d = collision.gameObject.GetComponent<FrictionJoint2D>();
            numberOfBounces = 1;
            rb2d.connectedBody = rb;
            collision.gameObject.GetComponent<PlatformMove>().activate = true;
            jumpAvailable = true;
            //lastContactPos = collision.contacts[0].point;
        }
        if (collision.collider.gameObject.tag == "SmallPlatform")
        {
            FrictionJoint2D rb2d = collision.gameObject.GetComponent<FrictionJoint2D>();
            numberOfBounces = 1;
            rb2d.connectedBody = rb;
            jumpAvailable = true;
            //lastContactPos = collision.contacts[0].point;
        }

        if (collision.gameObject.CompareTag("Spring"))
        {
            colAngle = Vector2.Angle(-collision.contacts[0].normal, new Vector2(collision.transform.up.x, collision.transform.up.y));
            if (colAngle > 120)
            {
                numberOfBounces += 0.1f;
                rb.AddForce((transform.up * 30) * numberOfBounces, ForceMode2D.Impulse);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Platforms") /*|| 
            collision.collider.gameObject.layer == LayerMask.NameToLayer("FallingPlatforms")*/)
        {
            isGrounded = false;
            //Debug.Log(isGrounded+" -- " +collision.collider.name);            
            this.transform.SetParent(null);
            /*Vector2 aux = new Vector2();
            aux.x = this.transform.position.x;
            aux.y = this.transform.position.y;            
            Debug.Log("herhe" + Vector2.Distance(aux, lastContactPos));
            if (Vector2.Distance(aux, lastContactPos) > 1.25f)
            {
                Debug.Log("herhe" + Vector2.Distance(aux, lastContactPos));
                Debug.Log("Bola sale del padre, fue intencional?");
                //this.transform.SetParent(null);
                
            }*/

        }
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("FallingPlatforms"))
        {
            isGrounded = false;
        }
        if (collision.collider.gameObject.tag == "MovingPlatform")
        {
            FrictionJoint2D rb2d = collision.gameObject.GetComponent<FrictionJoint2D>();
            rb2d.connectedBody = null;
            //collision.gameObject.GetComponent<PlatformMove>().activate = true;
        }
        if (collision.collider.gameObject.tag == "SmallPlatform")
        {
            FrictionJoint2D rb2d = collision.gameObject.GetComponent<FrictionJoint2D>();
            rb2d.connectedBody = null;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Platforms") || collision.collider.gameObject.layer == LayerMask.NameToLayer("FallingPlatforms"))
        {
            isGrounded = true;
            //Debug.Log(isGrounded + " -- " + collision.collider.name);
            this.transform.SetParent(collision.transform);
            //lastContactPos = collision.contacts[0].point;
        }

    }
}


