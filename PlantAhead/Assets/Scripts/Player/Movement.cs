using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public int speed = 5;

    private Rigidbody2D rigidbody2d;

    private float moveHorizontal;
    private float moveVertical;

    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveHorizontal = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveHorizontal = 1;
        }
        else
        {
            moveHorizontal = 0;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveVertical = -1;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            moveVertical = 1;
        }
        else
        {
            moveVertical = 0;
        }

        movement = new Vector2(moveHorizontal, moveVertical);
    }

    private void FixedUpdate()
    {
        rigidbody2d.AddForce(movement * speed * 10f);
    }
}
