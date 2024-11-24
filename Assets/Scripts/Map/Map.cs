using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour
{
    [Header("Индекс уровня")]
    public int indexLevel = 0;

    Button thisButton;
    public Image imageBlockAndNumber;
    public Sprite imageNumber;
    public Sprite imageBlock;

    public int isLoad = 0; // 0 не пройдено 1 пройдено
    public string idLevel;

    public Map mapNextLevel;

    public Map thisLevel;
    public Image thisImage;

    public OpenLock openLock;

    private void Awake()
    {
        thisLevel = GetComponent<Map>();
        thisImage = GetComponent<Image>();
        idLevel = gameObject.name;
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnPointerClick);
        CheckLevel();
    }

    private void Start()
    {
        openLock = GetComponent<OpenLock>();
    }

    public void OnPointerClick()
    {
        LoadLevel();
    }
    public bool isHealth = false;
    public bool isPower = false;
    public void LoadLevel()
    {
        if (isLoad == 0)
            return;

        if (DataManager.InstanceData.Power >= 1)
        {
            isPower = true;
        }
        else
        {
            PanelManager.InstancePanel.labelView.StartAnim();
            PanelManager.InstancePanel.labelView.TextPower();
            isHealth = false;
            isPower = false;
            return;
        }
        if (DataManager.InstanceData.Health >= 1)
        {
            isHealth = true;
        }
        else
        {
            PanelManager.InstancePanel.labelView.StartAnim();
            PanelManager.InstancePanel.labelView.TextHeath();
            isHealth = false;
            isPower = false;
            return;
        }
        if (isPower == true && isHealth == true)
        {
                Level();
        }
        isHealth = false;
        isPower = false;

    }
    public void Level()
    {
        DataManager.InstanceData.isStartGame = true;

        PanelManager.InstancePanel.SetHealthForPanel();
        Debug.Log("SetHealthForPanel");
        PanelManager.InstancePanel.SetDisActivePanel();
        Debug.Log("SetDisActivePanel");
        GameManager.InstanceGame.player.RestartMovement();
        Debug.Log("RestartMovement");
        SoundManager.InstanceSound.musicLevel.Play();
        Debug.Log("Play");
        SoundManager.InstanceSound.musicFon.Stop();
        Debug.Log("Stop");
        DataManager.InstanceData.mapNextLevel = thisLevel;
        Debug.Log("thisLevel");
        GameManager.InstanceGame.StartGame(indexLevel);
        Debug.Log("StartGame");
        GameManager.InstanceGame.RestartEnemy();
        Debug.Log("RestartEnemy");
        PanelManager.InstancePanel.SetActiveButtonStart();
        Debug.Log("SetActiveButtonStart");

        DataManager.InstanceData.Endurance--;
        DataManager.InstanceData.Hunger--;
    }

    public void OpenLevel()
    {
        mapNextLevel.isLoad = 1;
        mapNextLevel.CheckLevel();
        DataManager.InstanceData.SaveLevel();
    }

    public void CheckLevel()
    {
        if (isLoad == 0)
        {
            imageBlockAndNumber.sprite = imageBlock;
            //thisImage.sprite = spriteBlockGround;
        }
        if (isLoad == 1)
        {
            imageBlockAndNumber.sprite = imageNumber;
            //thisImage.sprite = spriteUnBlockGround;
        }
    }
}