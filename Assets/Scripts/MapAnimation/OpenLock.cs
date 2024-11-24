using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenLock : MonoBehaviour
{
    public RectTransform panel; // Панель, которую нужно анимировать
    public Vector2 expandedSize; // Размер панели после увеличения
    public Image image; // Изображение для анимации
    public float imageMoveDistanceY; // Расстояние, на которое будет двигаться изображение вверх и вниз
    public float animationDuration = 1f; // Длительность каждой анимации

    private Vector2 initialPosition;
    private Vector2 initialSize;

    public Animator animator;
    public GameObject imageRock;

    public Canvas canvas; // Основной Canvas
    //private Canvas tempCanvas; // Временный Canvas для анимации
    private GraphicRaycaster mainGraphicRaycaster; // GraphicRaycaster на основном Canvas

    public void StartAnimation()
    {
        mainGraphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        // Рассчитываем центр канваса в локальных координатах
        initialPosition = panel.anchoredPosition; // Сохраняем начальную позицию панели
        initialSize = panel.sizeDelta; // Сохраняем начальный размер панели
        animator = GetComponent<Animator>();

        // Создаем временный Canvas
        CreateTemporaryCanvas();

        Vector2 targetPosition = GetCenterOfCanvas();
        StartCoroutine(AnimatePanel(targetPosition));
    }

    Vector2 GetCenterOfCanvas()
    {
        // Вычисляем центр канваса
        Vector2 canvasCenter = new Vector2(canvas.GetComponent<RectTransform>().rect.width / 2, canvas.GetComponent<RectTransform>().rect.height / 2);
        return canvasCenter;
    }
    Canvas can;
    void CreateTemporaryCanvas()
    {
        // Создаем временный Canvas и устанавливаем его Sort Order
        can = gameObject.AddComponent<Canvas>();
        can.overrideSorting = true;
        can.sortingOrder = 1;
        can.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    void DestroyTemporaryCanvas()
    {
        Destroy(can);
    }

    IEnumerator AnimatePanel(Vector2 targetPosition)
    {
        // Отключаем GraphicRaycaster на основном Canvas
        mainGraphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        if (mainGraphicRaycaster != null)
        {
            mainGraphicRaycaster.enabled = false;
        }

        imageRock.SetActive(true);

        // Перемещаем панель в центр канваса
        yield return MovePanelToCenter(panel, panel.position, targetPosition, animationDuration);

        // Увеличиваем панель
        yield return ResizePanel(panel, panel.sizeDelta, expandedSize, animationDuration);

        // Анимация изображения
        animator.Play("Anima");
        DataManager.InstanceData.mapNextLevel.OpenLevel();

        // Возвращаем панель в исходный размер
        yield return ResizePanel(panel, expandedSize, initialSize, animationDuration);

        // Возвращаем панель в начальное положение
        yield return MovePanelBack(panel, initialPosition, animationDuration);

        // Включаем GraphicRaycaster на основном Canvas
        if (mainGraphicRaycaster != null)
        {
            mainGraphicRaycaster.enabled = true;
        }

        // Удаляем временный Canvas после завершения анимации
        DestroyTemporaryCanvas();

        Debug.Log("Анимация завершена.");
    }

    IEnumerator MovePanelToCenter(RectTransform rectTransform, Vector2 start, Vector2 end, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            rectTransform.position = Vector2.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.position = end;
    }

    IEnumerator MovePanelBack(RectTransform rectTransform, Vector2 end, float duration)
    {
        Vector2 start = rectTransform.anchoredPosition; // Получаем текущее значение позиции

        float time = 0f;
        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = end;
    }

    IEnumerator ResizePanel(RectTransform rectTransform, Vector2 start, Vector2 end, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            rectTransform.sizeDelta = Vector2.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.sizeDelta = end;
    }

    IEnumerator MoveImage(RectTransform rectTransform, Vector2 start, Vector2 end, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = end;
    }
}
