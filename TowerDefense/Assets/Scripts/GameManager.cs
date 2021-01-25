using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> 
{
    
    public TowerBtn ClickedBtn { get; set; }

    public bool shiftToggle;

    private int mana;

    private int wave = 0;

    private int lives;

    private bool gameOver = false;

    [SerializeField]
    private Text livesTxt;
    [SerializeField]
    private Text waveTxt;

    [SerializeField]
    private TextMeshProUGUI manaText;

    [SerializeField]
    private GameObject waveBtn;

    [SerializeField]
    private GameObject gameOverMenu;

    private Tower selectedTower;

    List<Monster> activeMonsters = new List<Monster>();

    public bool WaveActive
    {
        get
        {
            return activeMonsters.Count > 0;
        }
    }

    public ObjectPool Pool { get; set; }

    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            this.lives = value;
            if (lives <= 0)
            {
                this.lives = 0;
                GameOver();
            }
            livesTxt.text = lives.ToString();
        }
    }
    public int Mana 
    {
        get
        {
            return mana;
        }
        set
        {
            this.mana = value;
            manaText.text = value.ToString() + "<color=#00FF00>$</color>";
          
        }
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }
    // Start is called before the first frame update
    void Start()
    {
        shiftToggle = false;
        Lives = 10;
        Mana = 100; 
    }

    // Update is called once per frame
    void Update()
    {
        HandleShift();
        HandleEscape();
    }

    public void PickTower(TowerBtn towerBtn)
    {
        if (Mana >= towerBtn.Price && shiftToggle)
        {
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }
    }
    public void BuyTower()
    {
        if(Mana >= ClickedBtn.Price)
        {
            Mana -= ClickedBtn.Price;
            ClickedBtn = null;
            Hover.Instance.Deactivate();
        }
    }

    public void SelectTower(Tower tower)
    {   
        if (selectedTower == tower)
        {
            DeselectTower();
        } else if (selectedTower != null)
        {
            DeselectTower();
            selectedTower = tower;
            selectedTower.Select();
        }
        else
        {
            selectedTower = tower;
            selectedTower.Select();
        }
    }

    public void DeselectTower()
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = null;
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
            ClickedBtn = null; 
        }
    }
    private void HandleShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (shiftToggle == false)
            {
                shiftToggle = true;
            }
            else
            {
                shiftToggle = false;
                Hover.Instance.Deactivate();
            }
        }
        if (shiftToggle && GameManager.Instance.ClickedBtn != null)
        {
            Hover.Instance.TurnOn();
        }
    }

    public void StartWave()
    {
        wave++;

        waveTxt.text = string.Format("Wave: <color=lime>{0}</color>", wave);
        StartCoroutine(SpawnWave());

        waveBtn.SetActive(false);
    }

    private IEnumerator SpawnWave()
    {
        // int monsterIndex = Random.Range(0, 4);

        int monsterIndex = 1;

        string type = string.Empty;

       
        for (int i = 0; i < wave; i++)
        {
            switch (monsterIndex)
            {
                case 0:
                    type = "BlueMonster";
                    break;
                case 1:
                    type = "E2";
                    break;
                case 2:
                    type = "GreenMonster";
                    break;
                case 3:
                    type = "PurpleMonster";
                    break;
            }

            yield return new WaitForSeconds(2);
            Monster monster = Pool.GetObject(type).GetComponent<Monster>();
            monster.Spawn(0);
            yield return new WaitForSeconds(2);
            Monster monster2 = Pool.GetObject(type).GetComponent<Monster>();
            monster2.Spawn(1);
            yield return new WaitForSeconds(2);
            Monster monster3 = Pool.GetObject(type).GetComponent<Monster>();
            monster3.Spawn(2);
          


            activeMonsters.Add(monster);
        }

        //TIME DELAY UNTIL WAVE SPAWNS
     

    }

    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);

        if (!WaveActive && !gameOver)
        {
            waveBtn.SetActive(true);
        }
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1; 

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
