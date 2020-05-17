﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public int speed = 5;

    private Rigidbody2D rigidbody2d;

    private float moveHorizontal;
    private float moveVertical;

    public Vector3 movement;

    public Vector2 direction;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        // if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     moveHorizontal = -1;
        // }
        // else if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     moveHorizontal = 1;
        // }
        // else
        // {
        //     moveHorizontal = 0;
        // }

        // if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     moveVertical = -1;
        // }
        // else if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     moveVertical = 1;
        // }
        // else
        // {
        //     moveVertical = 0;
        // }

        // movement = new Vector3(moveHorizontal, moveVertical);

        movement = Vector3.zero;
        // wasd or arrow keys. Let the user pick?
        movement.x = Input.GetAxisRaw ("Horizontal"); 
        movement.y = Input.GetAxisRaw ("Vertical");

        if(movement != Vector3.zero)
        {
            animator.SetBool("moving", true);
            animator.SetFloat("X_pos", movement.x);
            animator.SetFloat("Y_pos", movement.y);
            direction.x = movement.x;
            direction.y = movement.y;
        }
        else
        {
            animator.SetBool("moving", false);
        }

    }

    private void FixedUpdate()
    {
        // rigidbody2d.AddForce(movement * speed * 10f);
        rigidbody2d.MovePosition(transform.position + movement.normalized * speed * Time.deltaTime);
    }
}
