using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonkeyMovement : MonoBehaviour
{
    public float speed = 6f;

    public float jumpPower = 10f;
    
    public float dashSpeed;

    private Vector2 movement;
    private Rigidbody2D rb;




    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    private void Update()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    private void FixedUpdate()
    {
        Move(movement);
    }

    private void Move(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }

}
