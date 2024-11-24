using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPoint : MonoBehaviour
{
    public bool requiresPause = true; // Определяет, нужно ли останавливать персонажа на этой точке
    public bool lastEnemy = false;
    public Enemy enemy;

    public float fadeDuration = 1.0f;

    [Header("Параметры игрока")]
    public GameObject spawnPoint_1;
    public GameObject buttonStart;

    [Space(25)]
    [Header("Свайпы")]
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 swipeDelta;
    public DamagePanel damagePanel;

    private bool swipeAllowed = false;
    private bool canSwipe = false;
    public float swipeThreshold = 50f; // Минимальная длина свайпа, чтобы распознать его как действительный

    private bool isSwiping;

    // Метод для обработки достижения точки (можно добавить другие параметры и действия)
    public void OnReached()
    {
        Lose();

        if (lastEnemy)
        {
            if (DataManager.InstanceData.mapNextLevel.mapNextLevel.isLoad == 0)
            {
                // Панели и уровни, которые могут быть активированы
            }
            else
            {
                PanelManager.InstancePanel.SetActivePanel(false);
            }
            SoundManager.InstanceSound.musicFon.Play();
            SoundManager.InstanceSound.musicLevel.Stop();
            GameManager.InstanceGame.gold += 2500;
            DataManager.InstanceData.SaveGold();
            SoundManager.InstanceSound.sountWictory.Play();

            PanelManager.InstancePanel.panelWin.StartAnim();

            DataManager.InstanceData.SaveHealth();
            DataManager.InstanceData.SavePower();
            DataManager.InstanceData.SaveEndurance();
            DataManager.InstanceData.SaveHunger();
            DataManager.InstanceData.SaveLives();

            PanelManager.InstancePanel.RemoveAllChildObject();
            DataManager.InstanceData.isStartGame = false;
            Debug.Log("End Game");
            return;
        }

        if (requiresPause)
        {
            if (enemy != null)
            {
                if (DataManager.InstanceData.Power < 0 || DataManager.InstanceData.Health <= 0)
                {
                    SoundManager.InstanceSound.musicFon.Play();
                    SoundManager.InstanceSound.musicLevel.Stop();
                    PanelManager.InstancePanel._UIPanelFade.FadeIn();
                }
                else
                {
                    //if (DataManager.InstanceData.mapNextLevel.indexLevel <= 2)
                    //{
                        StartCoroutine(DoubleCutting());
                    //}
                    //else
                    //{
                    //    StartCoroutine(TripleCutting());
                    //}
                }
            }
        }
    }

    public void Lose()
    {
        if (DataManager.InstanceData.isStartGame == true)
        {
            if (DataManager.InstanceData.Power <= 0)
            {
                PanelManager.InstancePanel.RemoveAllChildObject();
                PanelManager.InstancePanel.textResult.text = "You have run out of strength to fight";
                PanelManager.InstancePanel._UIPanelFade.FadeIn();
                DataManager.InstanceData.isStartGame = false;
                return;
            }
            if (DataManager.InstanceData.Health <= 0)
            {
                PanelManager.InstancePanel.RemoveAllChildObject();
                PanelManager.InstancePanel.textResult.text = "Your health is over";
                PanelManager.InstancePanel._UIPanelFade.FadeIn();
                DataManager.InstanceData.isStartGame = false;
                return;
            }
        }
    }

    public IEnumerator DoubleCutting()
    {
        buttonStart.SetActive(false);

        swipeAllowed = true; // Разрешаем свайпы только сейчас
        canSwipe = true; // Включаем обработку свайпов
        PanelManager.InstancePanel.imageClue.SetActive(true);
        // Ожидание свайпа до выполнения следующего действия
        yield return new WaitUntil(() => !canSwipe);

        Debug.Log("Удар от игрока");
        SoundManager.InstanceSound.soundDamage.Play();
        enemy.cutting.gameObject.SetActive(true);
        DataManager.InstanceData.Power--;
        PanelManager.InstancePanel.imageClue.SetActive(false);
        Lose();
        if (DataManager.InstanceData.isStartGame == false)
        {
            yield break;
        }

        // Ожидание завершения текущего действия перед следующим
        yield return new WaitForSeconds(1f);

        enemy.cutting.gameObject.SetActive(false);
        damagePanel.StartAnim();
        SoundManager.InstanceSound.sountDamageInPlayer.Play();
        DataManager.InstanceData.Health--;
        PanelManager.InstancePanel.RemoveChildObject();
        Debug.Log("Удар от противника");
        Lose();

        if (DataManager.InstanceData.isStartGame == false)
        {
            yield break;
        }

        swipeAllowed = true; // Разрешаем свайпы снова
        canSwipe = true; // Включаем обработку свайпов
        PanelManager.InstancePanel.imageClue.SetActive(true);
        // Ожидание свайпа
        yield return new WaitUntil(() => !canSwipe);

        Debug.Log("Удар от игрока");
        SoundManager.InstanceSound.soundDamage.Play();
        enemy.cutting.gameObject.SetActive(true);
        DataManager.InstanceData.Power--;
        PanelManager.InstancePanel.imageClue.SetActive(false);
        Lose();

        if (DataManager.InstanceData.isStartGame == false)
        {
            yield break;
        }

        yield return new WaitForSeconds(1f);

        canSwipe = false; // Отключаем свайпы до следующего удара

        StartCoroutine(FadeAndPlaySound());

        // Отключаем свайпы после завершения атак
        swipeAllowed = false;
        canSwipe = false;
        buttonStart.SetActive(true);
    }

    public IEnumerator TripleCutting()
    {
        buttonStart.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        SoundManager.InstanceSound.soundDamage.Play();
        enemy.cutting.gameObject.SetActive(true);
        DataManager.InstanceData.Power--;

        yield return new WaitForSeconds(0.5f);

        enemy.cutting.gameObject.SetActive(false);
        SoundManager.InstanceSound.sountDamageInPlayer.Play();
        damagePanel.StartAnim();
        DataManager.InstanceData.Health--;

        yield return new WaitForSeconds(0.5f);

        SoundManager.InstanceSound.soundDamage.Play();
        enemy.cutting.gameObject.SetActive(true);
        DataManager.InstanceData.Power--;

        yield return new WaitForSeconds(0.5f);

        enemy.cutting.gameObject.SetActive(false);
        SoundManager.InstanceSound.sountDamageInPlayer.Play();
        damagePanel.StartAnim();
        DataManager.InstanceData.Health--;

        yield return new WaitForSeconds(0.5f);

        SoundManager.InstanceSound.soundDamage.Play();
        enemy.cutting.gameObject.SetActive(true);
        DataManager.InstanceData.Power--;

        StartCoroutine(FadeAndPlaySound());
        buttonStart.SetActive(true);
    }

    private IEnumerator FadeAndPlaySound()
    {
        float elapsedTime = 0f;
        Color originalColor = enemy.spriteEnemy.color;
        Color originalColorcutting = enemy.cutting.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = originalColor;
            Color newColorcutting = originalColorcutting;

            newColor.a = Mathf.Lerp(originalColor.a, 0, t);
            newColorcutting.a = Mathf.Lerp(originalColorcutting.a, 0, t);

            enemy.spriteEnemy.color = newColor;
            enemy.cutting.color = newColorcutting;

            yield return null;
        }

        Color finalColor = enemy.spriteEnemy.color;
        Color finalColorcutting = enemy.cutting.color;

        finalColor.a = 0;
        finalColorcutting.a = 0;

        enemy.spriteEnemy.color = finalColor;
        enemy.cutting.color = finalColorcutting;
    }

    void Update()
    {
        if (canSwipe == true)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
            HandleTouchInput();
#endif
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isSwiping)
            {
                endTouchPosition = Input.mousePosition;
                swipeDelta = endTouchPosition - startTouchPosition;

                if (swipeDelta.magnitude >= swipeThreshold)
                {
                    DetectSwipeDirection();
                }

                isSwiping = false;
            }
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (isSwiping)
                {
                    endTouchPosition = touch.position;
                    swipeDelta = endTouchPosition - startTouchPosition;

                    if (swipeDelta.magnitude >= swipeThreshold)
                    {
                        DetectSwipeDirection();
                    }

                    isSwiping = false;
                }
            }
        }
    }

    private void DetectSwipeDirection()
    {
        float x = swipeDelta.x;
        float y = swipeDelta.y;
        float absX = Mathf.Abs(x);
        float absY = Mathf.Abs(y);

        if (absX > absY)
        {
            if (x > 0)
            {
                OnSwipeRight();
            }
            else
            {
                OnSwipeLeft();
            }
        }
        else
        {
            if (y > 0)
            {
                OnSwipeUp();
            }
            else
            {
                OnSwipeDown();
            }
        }

        // Диагональные свайпы
        if (absX > 0 && absY > 0)
        {
            // Определяем диагональные направления по углу
            float angle = Mathf.Atan2(absY, absX) * Mathf.Rad2Deg;

            if (angle > 45 && angle <= 135)
            {
                if (y > 0)
                {
                    OnSwipeUpLeft();
                }
                else
                {
                    OnSwipeDownLeft();
                }
            }
            else if (angle > 135 || angle <= -135)
            {
                if (x > 0)
                {
                    OnSwipeUpRight();
                }
                else
                {
                    OnSwipeDownRight();
                }
            }
            else
            {
                if (y > 0)
                {
                    OnSwipeUpRight();
                }
                else
                {
                    OnSwipeDownRight();
                }
            }
        }
    }

    // Свайп вверх (вертикальный)
    private void OnSwipeUp() { canSwipe = false; enemy.cutting.transform.Rotate(0, 0, 45); }

    // Свайп вниз (вертикальный)
    private void OnSwipeDown() { canSwipe = false; enemy.cutting.transform.Rotate(0, 0, 45);  }

    // Свайп влево (горизонтальный)
    private void OnSwipeLeft() { canSwipe = false; enemy.cutting.transform.Rotate(0, 0, 130);  }

    // Свайп вправо (горизонтальный)
    private void OnSwipeRight() { canSwipe = false; enemy.cutting.transform.Rotate(0, 0, 130);  }

    // Свайп вверх-влево (диагональный)
    private void OnSwipeUpLeft() { canSwipe = false; enemy.cutting.transform.Rotate(0, 0, 90); }

    // Свайп вверх-вправо (диагональный)
    private void OnSwipeUpRight() { canSwipe = false; enemy.cutting.transform.Rotate(0, 0, 0);  }

    // Свайп вниз-влево (диагональный)
    private void OnSwipeDownLeft() { canSwipe = false; enemy.cutting.transform.Rotate(0, 0, 0);  }

    // Свайп вниз-вправо (диагональный)
    private void OnSwipeDownRight() { canSwipe = false; enemy.cutting.transform.Rotate(0, 0, 90);  }


}