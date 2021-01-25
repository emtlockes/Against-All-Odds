using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 0;

    private float xMax;
    private float yMin;

    public Vector3 offset = new Vector3(0, 0, -1);
    // Update is called once per frame

    private void FixedUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        if (playerMovement.isMoving)
        {
            transform.position = new Vector3(
                player.transform.position.x + offset.x,
                player.transform.position.y + offset.y,
                player.transform.position.y + offset.z);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, yMin, 0), -10);
        }
        else
        {
            GetCameraInput();
        }

       
    }

    private void GetCameraInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, yMin, 0), -10);
    }

    public void SetLimits(Vector3 maxTile)
    {
        Vector3 wp = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));

        xMax = maxTile.x - wp.x;
        yMin = maxTile.y - wp.y;
    } 
}
