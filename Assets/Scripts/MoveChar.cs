using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChar : MonoBehaviour
{
    public float moveSpeed = 4;
    public float jumpForce = 5;
    public float fallMultiplier = 2.5f;
    public float lowJumpFallMultiplier = 1.5f;

    private bool isJumping = false;
    private bool isGrouned = false;

    private Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaSpeed = moveSpeed * Time.deltaTime;
        Vector2 instamove = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.D))
        {
            instamove.x += deltaSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            instamove.x -= deltaSpeed;
        }
        rigidbody.position += instamove;
        HandleJump();
    }

    void HandleJump()
    {
        if (isGrouned && !isJumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else if (!isGrouned && isJumping)
        {
            if (rigidbody.velocity.y < 0)
            {
                rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
        }
    }
    void Jump()
    {
        rigidbody.velocity += Vector2.up * jumpForce;
        isJumping = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrouned = true;
        isJumping = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrouned = true;
        isJumping = false;
        //Debug.Log("Staying");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrouned = false;
        //Debug.Log("Exiting");
    }
}
