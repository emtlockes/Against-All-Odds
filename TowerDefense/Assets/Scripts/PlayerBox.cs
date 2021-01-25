using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBox : MonoBehaviour
{
    

    // Update is called once per frame
    void Start()
    {   
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        transform.position = new Vector3(player.transform.position.x + (LevelManager.Instance.TileSize) , player.transform.position.y, player.transform.position.z);
    }
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        float x = player.transform.position.x;
        float y = player.transform.position.y;
        float z = player.transform.position.z;
        float tile = LevelManager.Instance.TileSize;

        if (playerMovement.isMoving)
        {
            if (playerMovement.isFacing == "up")
            {
                transform.position = new Vector3(x, y + tile, z);
            }
            if (playerMovement.isFacing == "down")
            {
                transform.position = new Vector3(x, y - tile, z);
            }
            if (playerMovement.isFacing == "left")
            {
                transform.position = new Vector3(x - tile, y, z);
            }
            if (playerMovement.isFacing == "right")
            {
                transform.position = new Vector3(x + tile, y, z);
            }
            if (playerMovement.isFacing == "up-left")
            {
                transform.position = new Vector3(x - tile, y + tile, z);
            }
            if (playerMovement.isFacing == "up-right")
            {
                transform.position = new Vector3(x + tile, y + tile, z);
            }
            if (playerMovement.isFacing == "down-left")
            {
                transform.position = new Vector3(x - tile, y - tile, z);
            }
            if (playerMovement.isFacing == "down-right")
            {
                transform.position = new Vector3(x + tile, y - tile, z);
            }
        }

    }
}
