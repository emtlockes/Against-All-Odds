using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public bool IsEmpty { get; private set; }

    private Tower myTower;

    private Color32 fullColor = new Color32(255, 118, 118, 255);
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite placedSprite;

    [SerializeField]
    public bool Walkable { get; set; }

    public bool placeToggle;


    public Point GridPosition { get; private set; }

    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        placeToggle = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && placeToggle && GameManager.Instance.ClickedBtn != null && this.tag == "PlaceableTile")
        {
            PlaceTower();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBox" && GameManager.Instance.shiftToggle)
        {
            if (IsEmpty)
            {
                ColorTile(emptyColor);
                placeToggle = true;
            }
            else
            {
                ColorTile(fullColor);
                placeToggle = false;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "PlayerBox" && GameManager.Instance.shiftToggle)
        {
            if (IsEmpty && this.tag == "PlaceableTile")
            {
                ColorTile(emptyColor);
                placeToggle = true;
            }
            else
            {
                ColorTile(fullColor);
                placeToggle = false;
            }
             
        }
        if (other.tag == "PlayerBox" && !GameManager.Instance.shiftToggle)
        {
            UnColorTile();
            placeToggle = false;
       
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PlayerBox")
        {
            UnColorTile();
            placeToggle = false;
        }
    }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        if (this.tag == "PlaceableTile" || this.tag == "Outlet")
        {
            Walkable = false;
        }
        else
        {
            Walkable = true;
        }
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);

    }
   
    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            if (myTower != null)
            {
                GameManager.Instance.SelectTower(myTower);
            }
            else
            {
                GameManager.Instance.DeselectTower();
            }
        }
    }
    private void PlaceTower()
    {
            float tileSize = LevelManager.Instance.TileSize * 0.5f;
            Vector3 pos = new Vector3 (transform.position.x + tileSize, transform.position.y - (0.5f * tileSize), 0);
            GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, pos, Quaternion.identity);

            tower.transform.SetParent(transform);

            IsEmpty = false; 
            GameManager.Instance.BuyTower();

            this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();
            this.spriteRenderer.sprite = placedSprite;
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
    
    private void UnColorTile()
    {
        ColorTile(Color.white);
    }
}
