using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public float moveSpeed = 5;

    public Rigidbody2D rb;

    Vector2 movement;

    public Vector3 playerPos = Vector3.zero;

    private float xMin;
    private float yMax;
    private float xMax;
    private float yMin;
    // Update is called once per frame

    public bool isMoving;
    public string isFacing;

    void Update()
    {
        //Input
        movement.x = 0;
        movement.y = 0;
        isMoving = false;

        if (Input.GetKey(KeyCode.W))
        {
           if (Input.GetKey(KeyCode.A))
            {
                movement.x -= 0.5f;
                movement.y += 0.5f;
                isMoving = true;
                isFacing = "up-left";
            }
           if (Input.GetKey(KeyCode.D))
            {
                movement.x += 0.5f;
                movement.y += 0.5f;
                isMoving = true;
                isFacing = "up-right";
            }
           if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                movement.y += 0.9f;
                isMoving = true;
                isFacing = "up";
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.A))
            {
                movement.x -= 0.5f;
                movement.y -= 0.5f;
                isMoving = true;
                isFacing = "down-left";
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement.x += 0.5f;
                movement.y -= 0.5f;
                isMoving = true;
                isFacing = "down-right";
            }
            if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                movement.y -= 0.9f;
                isMoving = true;
                isFacing = "down";
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
            {
                movement.x -= 0.9f;
                isMoving = true;
                isFacing = "left";
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
            {
                movement.x += 0.9f;
                isMoving = true;
                isFacing = "right";
            }
        }

        rb.position = new Vector2(Mathf.Clamp(rb.position.x, xMin, xMax), Mathf.Clamp(rb.position.y, yMin, yMax ));
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        playerPos = rb.position;
       
    }

    public void SetPlayerLimits(Vector3 worldStart, Vector3 maxTile, float tileSize)
    {
        xMin = worldStart.x + 0.5f * tileSize; // upper left hand x coordinate
        yMax = worldStart.y - 0.5f * tileSize; // upper left hand y coordinate;
        xMax = maxTile.x - 0.5f * tileSize;
        yMin = maxTile.y + 0.5f * tileSize;
    }
}
