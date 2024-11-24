using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnLockChest : MonoBehaviour
{
    public RectTransform panel; // Панель, высоту которой мы будем изменять

    public Image imageChest;
    public Sprite lockChest;
    public Sprite unLockChest;

    public GameObject lightObject;

    public Button buttonChestLock;
    private GameObject buttonChest;

    public float targetHeight = 1200f; // Конечное значение высоты
    public float duration = 1f; // Продолжительность анимации

    public void StartCorutinAnimation()
    {
        imageChest.sprite = unLockChest;
        lightObject.SetActive(true);
        buttonChest.SetActive(false);
        StartCoroutine(AnimatePanelHeight(0, targetHeight, duration));
    }

    private IEnumerator AnimatePanelHeight(float startHeight, float endHeight, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newHeight = Mathf.Lerp(startHeight, endHeight, elapsedTime / duration);
            SetPanelHeight(newHeight);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetPanelHeight(endHeight);
    }

    private void SetPanelHeight(float height)
    {
        Vector2 sizeDelta = panel.sizeDelta;
        sizeDelta.y = height;
        panel.sizeDelta = sizeDelta;
    }
}
