using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    // implementing some variables for later usage in code

    private Rigidbody2D rb;
    private Animator anim;
    private enum State {idle, running, jumping, falling, hurt}
    private State state = State.idle;

    private Collider2D playerCollider;
    [SerializeField] private LayerMask ground;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 9f;
    [SerializeField] private float hurtForce = 6f;


    [SerializeField] private int coins = 0;
    [SerializeField] private int points = 0;
    [SerializeField] private Text coinCounter;
    [SerializeField] private Text pointCouter;

    private int life = 3;
    [SerializeField] private Text lifeCounter;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

        lifeCounter.text = life.ToString();
    }

     

    private void Update()
    {
        // while player is not hurting it  can move around
        if(state != State.hurt)
        {
            movementManager();
        }
        animationState();
        anim.SetInteger("state", (int)state);
    }

    // this function destroys the coins when touched and increase the coins number in screen
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check the object that charachter touches is a coin
        if(collision.tag == "collectable")
        {
            Destroy(collision.gameObject);
            coins += 1;
            coinCounter.text =  coins.ToString();
        }

        if (collision.gameObject.tag == "barier")
        {
            Destroy(this.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // this function destroys enemy if player jumps on top of it.
    private void OnCollisionEnter2D(Collision2D other)
    {
        // check that the object charachter touches is enemy
        if(other.gameObject.tag == "enemy")
        {
            // if player is jumping the enemy will be killed
            if(state == State.falling) { 
                Destroy(other.gameObject);
                points += 1;
                pointCouter.text = points.ToString();
                jump();

            }

            // else player will get hurt
            else{
                
                state = State.hurt;

                life -= 1;
                lifeCounter.text = life.ToString();

                // show game over message
                if(life <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                // the enemey is to the right of the player. player gets hurt and move left
                if(other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }

                // else enemy is to the left of the player. player gets hurt and move to right
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
           }
        }

        
    }

    // this function makes the player jump
    private void jump()
    {
        // jump code for the player
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    // this function controls the players movement
    private void movementManager()
    {
        float hDirection = Input.GetAxis("Horizontal");

        // moving player to the left
        if (hDirection < 0)
        {
            // changing the player's speed in x axis
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            // making the player to face left
            transform.localScale = new Vector2(-1, 1);
        }
        // moving player to the right right
        else if (hDirection > 0)
        {
            // changing the player's speed in x axis
            rb.velocity = new Vector2(speed, rb.velocity.y);
            // making the player to face right
            transform.localScale = new Vector2(1, 1);
        }

        // player jumping
        if (Input.GetButtonDown("Jump") && playerCollider.IsTouchingLayers(ground))
        {
            jump();
        }

        
    }

    // this function controls the animation states
    private void animationState()
    {
        // jumping state animation
        if(state == State.jumping)
        {
            if(rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }
        // running state animation
        else if(state == State.falling)
        {
            if (playerCollider.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        // hurt state animation
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.idle;
            }
        }
        //  moving state animation 
        else if (Mathf.Abs(rb.velocity.x) > 2f) {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }
}
