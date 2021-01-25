using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer;

    private SpriteRenderer rangeSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        this.rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayerBox();
    }

    private void FollowPlayerBox()
    {
        GameObject playerBox = GameObject.FindGameObjectWithTag("PlayerBox");

        float x = playerBox.transform.position.x;
        float y = playerBox.transform.position.y;
        float z = playerBox.transform.position.z;

        transform.position = new Vector3(x, y + 0.4f, z);

    }
    public void Activate (Sprite sprite)
    {

        this.spriteRenderer.sprite = sprite;
        
    }
    
    public void TurnOn()
    {
        spriteRenderer.enabled = true;
        if (this.spriteRenderer.sprite != null)
        {
            rangeSpriteRenderer.enabled = true;
        }
    }

    public void Deactivate()
    {
        spriteRenderer.enabled = false;
        rangeSpriteRenderer.enabled = false;
    }
}
