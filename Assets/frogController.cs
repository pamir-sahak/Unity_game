using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogController : MonoBehaviour
{
    public float leftBorder = -32.7f;
    public float rightBorder = -24.26f;

    private bool facingLeft = true;

    [SerializeField] private float jumpLenght = 5f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private LayerMask ground;

    private Collider2D frogCollider;

    private Rigidbody2D frogRB;
    private void Start()
    {
        frogCollider = GetComponent<Collider2D>();
        frogRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement();
    }

    // this function controls the movement of the frog
    private void movement()
    {
        if (facingLeft)
        {
            // if were inside our left borders frog will jump to the left
            if (transform.position.x > leftBorder)

            {
                // facing the frog to right posintion
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                // if frog is touching the ground than it will jump
                if (frogCollider.IsTouchingLayers(ground))
                {
                    // jump
                    frogRB.velocity = new Vector2(-jumpLenght, jumpHeight);

                }
            }

            else
            {
                facingLeft = false;
            }
        }
        else
        {
            // if were inside our left borders frog will jump to the left
            if (transform.position.x < rightBorder)

            {
                // facing the frog to right posintion
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                // if frog is touching the ground than it will jump
                if (frogCollider.IsTouchingLayers())
                {
                    // jump
                    frogRB.velocity = new Vector2(jumpLenght, jumpHeight);

                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
