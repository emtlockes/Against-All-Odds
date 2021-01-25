using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private CameraMovement cameraMovement;
   
    [SerializeField]
    private Transform map;

    [SerializeField]
    private int[] enemySpawnX;

    [SerializeField]
    private int[] enemySpawnY;

    [SerializeField]
    private int[] enemyDespawnX;

    [SerializeField]
    private int[] enemyDespawnY;

    private Point[] enemySpawns;
    
    private Point[] enemyDespawns;

    [SerializeField]
    private GameObject[] enemySpawnPrefabs;

    [SerializeField]
    private GameObject[] enemyDespawnPrefabs;

    public Point[] EnemySpawns { get => enemySpawns; set => enemySpawns = value; }

    public GameObject[] EnemySpawnPrefabs { get => enemySpawnPrefabs; set => enemySpawnPrefabs = value; }

    private int tempSpawnIndex;

    public int TempSpawnIndex { get => tempSpawnIndex; set => tempSpawnIndex = value; }

    private Point mapSize;

    public Dictionary<Point, TileScript> Tiles { get; set; }

    public Dictionary<Point, Vector3> AllPoints { get; set; }

    public float TileSize
    {
        get {return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;}
    }

    // Start is called before the first frame update
    void Start()
    {
        enemySpawns = CreatePoints(enemySpawnX, enemySpawnY);

        enemyDespawns = CreatePoints(enemyDespawnX, enemyDespawnY);

        UnityEngine.Random.seed = 50;
        CreateLevel();
        
    }

    private Point[] CreatePoints (int[] x, int[] y)
    {
        int length = x.Length;
        Point[] tmpPoints = new Point[length];
        for (int i = 0; i < length; i++)
        {
            tmpPoints[i] = new Point(x[i], y[i]);
        }
        return tmpPoints;
    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();
        AllPoints = new Dictionary<Point, Vector3>();

        string[] mapData = ReadLevelText();

        int mapX = mapData[0].Split(' ').Length;
        int mapY = mapData.Length;

        mapSize = new Point(2 * mapX, 2 * mapY);

        Vector3 maxTile = Vector3.zero;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapY; y++)
        {
            string[] lineData = mapData[y].Split(' ');

            for (int x = 0; x < mapX; x++)
            {
                
                PlaceTile(lineData[x], x, y, worldStart);
            }
        }
        maxTile = Tiles[new Point(2 * (mapX - 1), 2 *(mapY - 1))].transform.position; 

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
        PlayerMovement.Instance.SetPlayerLimits(worldStart, new Vector3(maxTile.x + TileSize, maxTile.y - TileSize), TileSize);

        SpawnPortals();
    }

    
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
       
        int tileIndex = int.Parse(tileType);

        if (tileIndex == 0)
        {
            tileIndex = UnityEngine.Random.Range(30, 37);
        }
        if (tileIndex == 1)
        {
            tileIndex = UnityEngine.Random.Range(37, 44);
        }

        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        newTile.Setup(new Point(2 * x, 2 * y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);

        AllPoints.Add(new Point(2 * x, 2 * y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0));
        AllPoints.Add(new Point((2 * x) + 1, 2 * y), new Vector3(worldStart.x + (TileSize * x) + (0.5f * TileSize), worldStart.y - (TileSize * y), 0));
        AllPoints.Add(new Point(2 * x, (2 * y) + 1), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y) - (0.5f * TileSize), 0));
        AllPoints.Add(new Point((2 * x) + 1, (2 * y) + 1), new Vector3(worldStart.x + (TileSize * x) + (0.5f * TileSize), worldStart.y - (TileSize * y) - (0.5f * TileSize), 0));

    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level2") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

   

    private void SpawnPortals()
    {
        int spawnLength = enemySpawns.Length;
        for (int i = 0; i < spawnLength; i++)
        {
            TempSpawnIndex = i;
            
            GameObject tmp = Instantiate(enemySpawnPrefabs[i], AllPoints[enemySpawns[i]], Quaternion.identity);
          
            tmp.name = "Enemy Spawn" + TempSpawnIndex;
        }

        int despawnLength = enemyDespawns.Length;
        for (int i = 0; i < despawnLength; i++)
        {
            Instantiate(enemyDespawnPrefabs[i], AllPoints[enemyDespawns[i]], Quaternion.identity);
        }
    }

    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X <= mapSize.X && position.Y <= mapSize.Y;
    }
}
