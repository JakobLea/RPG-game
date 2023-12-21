using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rb;
    private Vector2 input;

    Animator anim;
    private Vector2 lastmoveDirection;
    private bool facingLeft = true;

    public Transform Aim;
    bool isWalking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    //Hello
    void Update()
    {
        ProccessInputs();
        Animate();

        if (input.x < 0 && !facingLeft || input.x > 0 && facingLeft)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = input * speed * Time.fixedDeltaTime;
        if (isWalking)
        {
            Vector3 vector3 = Vector3.left * input.x + Vector3.down * input.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
    }

    void ProccessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if ((moveX == 0 && moveY == 0) && (input.x != 0 || input.y != 0))
        {
            isWalking = false;
            lastmoveDirection = input;
            Vector3 vector3 = Vector3.left * lastmoveDirection.x + Vector3.down * lastmoveDirection.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
        else if (moveX != 0 || moveY != 0)
        {
            isWalking = true;
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();
    }

    void Animate()
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("MoveMagnitude", input.magnitude);
        anim.SetFloat("LastMoveX", lastmoveDirection.x);
        anim.SetFloat("LastMoveY", lastmoveDirection.y);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }
}