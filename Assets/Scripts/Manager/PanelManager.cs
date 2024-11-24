using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [Header("Кнопки в главном меню")]
    public Button buttonReward;
    public Button buttonPersonal;
    public Button buttonOptions;

    [Header("Пенели из главного меню")]
    public GameObject panelReward;
    public GameObject panelPersonal;
    public GameObject panelOptions;

    [Header("кнопки закрытия окон")]
    public Button buttonRewardClose;
    public Button buttonPersonalClose;
    public Button buttonOptionsClose;

    [Space(20)]
    [Header("Кнопки улучшения персонажа")]
    public Button buttonPlayerPanelClose;

    [Header("Текстовые панели в улучшении")]
    public Text textBeforeUpdate;
    public Text textAfterUpdate;
    public Text textPriceOnButton;
    public Text textPlayerDamage;

    public int damage = 350;
    public int updateCost = 150;
    public string idUpdateCost = "_costUpdate";

    public int powerPlayer;
    public string idPowerPlayer = "power";

    public int levelPLayer = 1;
    public string idLevelPLayer = "level_";

    public int countFirstUpdate;
    public double growthFactor = 1.5;

    [Header("Панель улучшения персонажа")]
    public GameObject panelUpdate;
    public Text Lives;
    public Text Health;
    public Text Power;
    public Text Endurance;
    public Text Hunger;

    [Header("Персонаж")]
    public GameObject buttonStart;

    public GameObject[] panelIsActive;

    public AnimationPAnel animCandy;

    [Header("Банель проигрыша")]
    public GameObject panelGameover;
    public UIPanelFade _UIPanelFade;
    public Button buttonBackToMainMenu;

    [Header("Персонаж")]
    public Image character;
    public Sprite updateCharacter;

    [Header("Панель магазина")]
    public GameObject panelShop;
    public Button closePanelShop;
    public Button[] buttonOpenShop;

    [Header("Активация панели улучшения")]
    public GameObject panelCharacter;
    public Button buttonOpenPanelCharacter;
    public Button buttonClosePanelCharacter;

    [Header("Панель выигрыша")]
    public PanelWin panelWin;
    public Button buttonClosePanelWin;

    [Header("Количество здоровья")]
    public GameObject PanelHealth;
    public GameObject imageHealth;
    public List<GameObject> panelHealthList;

    [Header("Текст проигрыша")]
    public Text textResult;

    [Header("Подсказка для свайпа")]
    public GameObject imageClue;

    [Header("Лайбл")]
    public LabelView labelView;
    public Text textLebel;

    public static PanelManager InstancePanel { get; private set; }

    private void Awake()
    {
        if (InstancePanel != null && InstancePanel != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstancePanel = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [Header("Переключатели")]
    public bool isFistPersonActive = false;

    private void Start()
    {
        buttonReward.onClick.AddListener(() => ActivePanel(panelReward));
        buttonPersonal.onClick.AddListener(() => {
            if (isFistPersonActive == false)
            {
                SoundManager.InstanceSound.sountFirstPerson.Play();
                isFistPersonActive = true;
            }
            ActivePanel(panelPersonal); 
        
        });
        buttonOptions.onClick.AddListener(() => ActivePanel(panelOptions));

        buttonRewardClose.onClick.AddListener(() => ClosePanel(panelReward));
        buttonPersonalClose.onClick.AddListener(() => ClosePanel(panelPersonal));
        buttonOptionsClose.onClick.AddListener(() => ClosePanel(panelOptions));

        //buttonPlayerUpdate.onClick.AddListener(() => { PanelUpdateActive(panelUpdate); SetValueForUpdate(); });
        buttonPlayerPanelClose.onClick.AddListener(() => { UplyText(); PanelUpdateDisActive(panelUpdate); });

        //buttonUpdate.onClick.AddListener(() => { UpdatePlayer(); });

        //кнопка проигрыша
        buttonBackToMainMenu.onClick.AddListener(() => 
        {
            imageClue.SetActive(false);
            SetActivePanel();
            SetDisActiveButtonStart();
        });

        buttonOpenPanelCharacter.onClick.AddListener(() => panelCharacter.SetActive(true));
        buttonClosePanelCharacter.onClick.AddListener(() => panelCharacter.SetActive(false));

        closePanelShop.onClick.AddListener(() => ClosePanel(panelShop));

        for (int i = 0; i < buttonOpenShop.Length; i++)
        {
            int count = i;
            buttonOpenShop[i].onClick.AddListener(() => ActivePanel(panelShop));
        }

        buttonClosePanelWin.onClick.AddListener(()=> 
        {
            ClosePanelWin();
            SetActivePanel();
            StartCoroutine(qwe());
        });
    }

    IEnumerator qwe()
    {
        yield return new WaitForSeconds(1f);
        DataManager.InstanceData.mapNextLevel.mapNextLevel.openLock.StartAnimation();
    }

    public void SetHealthForPanel()
    {
        for (int i = 0; i < DataManager.InstanceData.Health; i++)
        {
            Instantiate(imageHealth, transform.position, Quaternion.identity, PanelHealth.transform);
        }
        panelHealthList = new List<GameObject>();
        foreach (Transform child in PanelHealth.transform)
        {
            panelHealthList.Add(child.gameObject);
        }
    }
    public void RemoveChildObject()
    {
        if (panelHealthList.Count > 0)
        {
            GameObject objectH = panelHealthList[0];
            panelHealthList.RemoveAt(0);
            Destroy(objectH);
        }
    }
    public void RemoveAllChildObject()
    {
        if (panelHealthList.Count > 0)
        {
            foreach (GameObject child in panelHealthList)
            {
                Destroy(child.gameObject);
            }
            panelHealthList.Clear();
        }
    }

    public void ClosePanelWin()
    {
        panelWin.ClosePanel();
    }

    public void UplyText()
    {
        Lives.text = DataManager.InstanceData.Lives.ToString();
        Endurance.text = DataManager.InstanceData.Endurance.ToString();
        Health.text = DataManager.InstanceData.Health.ToString();
        Power.text = DataManager.InstanceData.Power.ToString();
        Hunger.text = DataManager.InstanceData.Hunger.ToString();
    }

    public void PanelUpdateActive(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void PanelUpdateDisActive(GameObject panel)
    {
        panel.SetActive(false);
    }
    public void ActivePanel(GameObject panel)
    {
        animCandy.StartAnimation(panel, true);
    }
    public void ClosePanel(GameObject panel)
    {
        animCandy.StartAnimation(panel, false);
    }

    public void SetActivePanel(bool lose = false)
    {
        if (lose == false)
        {
            for (int i = 0; i < panelIsActive.Length; i++)
            {

                StartCoroutine(animCandy.MovePanelOutAndBack(panelIsActive[0], true, panelIsActive));
            }
        }
        else
        {
            for (int i = 0; i < panelIsActive.Length; i++)
            {
                panelIsActive[i].SetActive(true);
            }
        }
    }
    public void SetDisActivePanel()
    {
        StartCoroutine(animCandy.MovePanelOutAndBack(panelIsActive[0], false, panelIsActive));
    }
    public void SetActiveButtonStart()
    {
        animCandy.StartAnimation(buttonStart, true);
    }
    public void SetDisActiveButtonStart()
    {
        _UIPanelFade.FadeOut();
        animCandy.StartAnimation(buttonStart, false);
        buttonStart.SetActive(false);
        SetActivePanel(false);
    }
    //public void SetValueForUpdate()
    //{

    //    textBeforeUpdate.text = powerPlayer.ToString();

    //    levelPLayer++;
    //    int calculatedDamage = Convert.ToInt32(damage * Math.Pow(growthFactor, levelPLayer - 1));
    //    levelPLayer--;
    //    int calculatedPrice = Convert.ToInt32(updateCost * Math.Pow(growthFactor, levelPLayer - 1));
    //    textAfterUpdate.text = $"{calculatedDamage}";
    //    textPriceOnButton.text = $"{calculatedPrice}";
    //}

    //public void UpdatePlayer()
    //{
    //    if (countFirstUpdate <= GameManager.InstanceGame.gold)
    //    {
    //        countFirstUpdate = Convert.ToInt32(updateCost * Math.Pow(growthFactor, levelPLayer - 1));

    //        GameManager.InstanceGame.gold -= countFirstUpdate;
    //        powerPlayer = Convert.ToInt32(damage * Math.Pow(growthFactor, levelPLayer));
    //        textAfterUpdate.text = $"{Convert.ToInt32(damage * Math.Pow(growthFactor, levelPLayer - 1))}";
    //        Debug.Log($"powerPlayer  {powerPlayer}");
    //        textPlayerDamage.text = powerPlayer.ToString();

    //        levelPLayer++;
    //        UpdateCharacterPerson();
    //        SetValueForUpdate();

    //        DataManager.InstanceData.SaveLevelPlayer();
    //        DataManager.InstanceData.SaveGold();
    //        DataManager.InstanceData.SavePowerPlayer();
    //    }
    //}
}