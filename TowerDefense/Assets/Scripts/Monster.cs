using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Point[] path;

    private int pathIndex;

    private Animator myAnimator;

    private SpriteRenderer mySpriteRenderer;
    
    [SerializeField]
    private Animator gunsAnimator;

    public Point GridPosition { get; set; }

    private Vector3 destination;

    private ERange range;

    public bool IsActive { get; set; }

    private float progress;

    public float Progress { get => progress; set => progress = value; }

    [SerializeField]
    private int totalHealth;

    [SerializeField]
    private int health;

    private SpriteRenderer healthSpriteRenderer;
    
    [SerializeField]
    private Sprite[] healthSprites;

    private void Update()
    {
        Move();

        if (IsActive)
        {
            Progress += (speed * Time.deltaTime);
        }

        if (range.TargetPos == Vector3.zero)
        {
            gunsAnimator.SetBool("Attacking", false);
        }
        if (range.TargetPos != Vector3.zero)
        {
            gunsAnimator.SetBool("Attacking", true);
        }
    }

    public void Spawn(int spawnIndex)
    {
        //initializing stuff

        this.health = totalHealth;

        this.healthSpriteRenderer = gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

        IsActive = true; 

        //animating stuff

        myAnimator = GetComponent<Animator>();

        range = gameObject.GetComponentInChildren<ERange>();

        //pathing stuff

        this.Progress = 0;
        this.pathIndex = 0;

        this.GridPosition = LevelManager.Instance.EnemySpawns[spawnIndex];

        this.path = SetPath(LevelManager.Instance.EnemySpawnPrefabs[spawnIndex].GetComponent<SpawnScript>().PathXCoordinates, LevelManager.Instance.EnemySpawnPrefabs[spawnIndex].GetComponent<SpawnScript>().PathYCoordinates);

        transform.position = GetPosFromPoint(GridPosition);

        destination = GetPosFromPoint(path[pathIndex]);
    }

    private void Move()
    {
        if (IsActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (transform.position == destination)
            {
                GridPosition = path[pathIndex];
                Animate(GridPosition, path[pathIndex + 1]);
                this.pathIndex++;
                destination = GetPosFromPoint(path[pathIndex]);
            }
        }
    }

    private Vector3 GetPosFromPoint(Point x)
    {
        return LevelManager.Instance.AllPoints[x];
    }

    private Point[] SetPath(int[] spawnPathX, int[] spawnPathY)
    {
        int length = spawnPathX.Length;

        int[] pathX = new int[length];
        int[] pathY = new int[length];

        for (int i = 0; i < length; i++)
        {
            pathX[i] = spawnPathX[i];
        }
        for (int i = 0; i < length; i++)
        {
            pathY[i] = spawnPathY[i];
        }
       
        Point[] tmpPoints = new Point[length];

        for (int i = 0; i < length; i++)
        {
            tmpPoints[i] = new Point(pathX[i], pathY[i]);
        }
        return tmpPoints;

    }

    private void Animate(Point currentPos, Point newPos)
    {
        if (currentPos.X < newPos.X)
        {
            myAnimator.SetInteger("Horizontal", 1);
            myAnimator.SetInteger("Vertical", 0);
            //moving right 
        }
        else if (currentPos.X > newPos.X)
        {
            myAnimator.SetInteger("Horizontal", -1);
            myAnimator.SetInteger("Vertical", 0);
            //moving left
        }
        else if (currentPos.Y > newPos.Y)
        {
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", 1);
        }  else if (currentPos.Y < newPos.Y)
        {
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", -1);
            //moving up
        } 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RedPortal")
        {
            Release(gameObject);
            GameManager.Instance.Lives--;
        }

        if (other.tag == "BlueBall")
        {
            this.health -= 15;
            SetHealth();
        }
    }

    public void SetHealth()
    {
        if (health > (0.8 * totalHealth))
        {
            healthSpriteRenderer.sprite = healthSprites[0];
        } else if (health > (0.6 * totalHealth))
        {
            healthSpriteRenderer.sprite = healthSprites[1];
        } else if (health > (0.4 * totalHealth))
        {
            healthSpriteRenderer.sprite = healthSprites[2];
        } else if (health > (0.2 * totalHealth))
        {
            healthSpriteRenderer.sprite = healthSprites[3];
        } else if (health > 0)
        {
            healthSpriteRenderer.sprite = healthSprites[4];
        } else
        {
            healthSpriteRenderer.sprite = healthSprites[5];
        }
    }

    private void Release(GameObject gameObject)
    {
       
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        IsActive = false;
        GameManager.Instance.RemoveMonster(this);
    }
}
