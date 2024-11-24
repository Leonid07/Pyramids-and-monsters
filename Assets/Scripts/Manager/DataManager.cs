using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{

    public bool isStartGame = false;

    public Map[] levels;
    public Sprite[] spriteNumber;

    [Header("Обучение")]
    public int valueEd = 0; //  0 не пройдёно 1 пройдено
    public string _idLevelEducation = "_idLevelEducation";

    public Map mapNextLevel;

    [Space(50)]
    [Header("Магазин")]
    public Button buttonHealth;
    public Button buttonPower;
    public Button buttonEndurance;
    public Button buttonHunger;
    public Button buttonLives;

    [Header("Характеристики персонажа")]
    public int Health = 10;
    string idHealth = "Health";

    public int Power = 10;
    string idPower = "Power";

    public int Endurance = 10;
    string idEndurance = "Endurance";

    public int Hunger = 2;
    string idHunger = "Hunger";

    public int Lives = 5;
    string idLives = "Lives";

    public static DataManager InstanceData { get; private set; }

    private void Awake()
    {
        if (InstanceData != null && InstanceData != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceData = this;
            DontDestroyOnLoad(gameObject);
        }
        SetSpriteNumber();
    }

    private void Start()
    {
        SetIndexLevel();
        LoadLevel();
        LoadGold();
        LoadFirstUpdate();

        LoadHealth();
        LoadPower();
        LoadEndurance();
        LoadHunger();
        LoadLives();

        buttonHealth.onClick.AddListener(() => { SetValueOrBuyHealth( 10, 500); SaveHealth();});
        buttonPower.onClick.AddListener(() => { SetValueOrBuyPower( 10, 600); SavePower(); });
        buttonEndurance.onClick.AddListener(() => { SetValueOrBuyEndurance(10,600); SaveEndurance(); });
        buttonHunger.onClick.AddListener(() => { SetValueOrBuyHunger(10,200); SaveHunger(); });
        buttonLives.onClick.AddListener(()=> { SetValueOrBuyLives(10,1000); SaveLives(); });
    }

    public void SetValueOrBuyHealth(int count, int countGold)
    {
        if (GameManager.InstanceGame.gold >= countGold)
        {
            GameManager.InstanceGame.gold -= countGold;
            Health += count;
            SaveGold();
        }
    }
    public void SetValueOrBuyPower(int count, int countGold)
    {
        if (GameManager.InstanceGame.gold >= countGold)
        {
            GameManager.InstanceGame.gold -= countGold;
            Power += count;
            SaveGold();
        }
    }
    public void SetValueOrBuyEndurance(int count, int countGold)
    {
        if (GameManager.InstanceGame.gold >= countGold)
        {
            GameManager.InstanceGame.gold -= countGold;
            Endurance += count;
            SaveGold();
        }
    }
    //Lives
    public void SetValueOrBuyHunger(int count, int countGold)
    {
        if (GameManager.InstanceGame.gold >= countGold)
        {
            GameManager.InstanceGame.gold -= countGold;
            Hunger += count;
            SaveGold();
        }
    }
    public void SetValueOrBuyLives(int count, int countGold)
    {
        if (GameManager.InstanceGame.gold >= countGold)
        {
            GameManager.InstanceGame.gold -= countGold;
            Lives += count;
            SaveGold();
        }
    }

    public void SaveHealth()
    {
        PlayerPrefs.SetInt(idHealth, Health);
        PlayerPrefs.Save();
    }
    public void LoadHealth()
    {
        if (PlayerPrefs.HasKey(idHealth))
        {
            Health = PlayerPrefs.GetInt(idHealth);
        }
    }

    public void SavePower()
    {
        PlayerPrefs.SetInt(idPower, Power);
        PlayerPrefs.Save();
    }
    public void LoadPower()
    {
        if (PlayerPrefs.HasKey(idPower))
        {
            Power = PlayerPrefs.GetInt(idPower);
        }
    }
    public void SaveEndurance()
    {
        PlayerPrefs.SetInt(idEndurance, Endurance);
        PlayerPrefs.Save();
    }
    public void LoadEndurance()
    {
        if (PlayerPrefs.HasKey(idEndurance))
        {
            Endurance = PlayerPrefs.GetInt(idEndurance);
        }
    }
    public void SaveHunger()
    {
        PlayerPrefs.SetInt(idHunger, Hunger);
        PlayerPrefs.Save();
    }
    public void LoadHunger()
    {
        if (PlayerPrefs.HasKey(idHunger))
        {
            Hunger = PlayerPrefs.GetInt(idHunger);
        }
    }
    public void SaveLives()
    {
        PlayerPrefs.SetInt(idLives, Lives);
        PlayerPrefs.Save();
    }
    public void LoadLives()
    {
        if (PlayerPrefs.HasKey(idLives))
        {
            Lives = PlayerPrefs.GetInt(idLives);
        }
    }

    public void SetSpriteNumber()
    {
        for (int i =0; i < levels.Length; i++)
        {
            levels[i].imageNumber = spriteNumber[i];
        }
    }

    public void SetIndexLevel()
    {
        int count = 1;
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].indexLevel = count;
            count++;
        }
    }

    public void SaveLevel()
    {
        for (int i =0; i < levels.Length; i++)
        {
            PlayerPrefs.SetInt(levels[i].idLevel, levels[i].isLoad);
            PlayerPrefs.Save();
        }
    }
    public void LoadLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (PlayerPrefs.HasKey(levels[i].idLevel))
            {
                levels[i].isLoad = PlayerPrefs.GetInt(levels[i].idLevel);
                levels[i].CheckLevel();
            }
        }
    }

    public void SaveFirstUpdate()
    {
        PlayerPrefs.SetInt(PanelManager.InstancePanel.idUpdateCost, PanelManager.InstancePanel.updateCost);
        PlayerPrefs.Save();
    }

    public void LoadFirstUpdate()
    {
        if (PlayerPrefs.HasKey(PanelManager.InstancePanel.idUpdateCost))
        {
            PanelManager.InstancePanel.updateCost = PlayerPrefs.GetInt(PanelManager.InstancePanel.idUpdateCost);
        }
    }

    public void SaveGold()
    {
        GameManager.InstanceGame.ApplyGold();
        PlayerPrefs.SetInt(GameManager.InstanceGame.idGold, GameManager.InstanceGame.gold);
        PlayerPrefs.Save();
    }
    public void LoadGold()
    {
        if (PlayerPrefs.HasKey(GameManager.InstanceGame.idGold))
        {
            GameManager.InstanceGame.gold = PlayerPrefs.GetInt(GameManager.InstanceGame.idGold);
            GameManager.InstanceGame.ApplyGold();
        }
    }
}
