using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenLock : MonoBehaviour
{
    public RectTransform panel; // ������, ������� ����� �����������
    public Vector2 expandedSize; // ������ ������ ����� ����������
    public Image image; // ����������� ��� ��������
    public float imageMoveDistanceY; // ����������, �� ������� ����� ��������� ����������� ����� � ����
    public float animationDuration = 1f; // ������������ ������ ��������

    private Vector2 initialPosition;
    private Vector2 initialSize;

    public Animator animator;
    public GameObject imageRock;

    public Canvas canvas; // �������� Canvas
    //private Canvas tempCanvas; // ��������� Canvas ��� ��������
    private GraphicRaycaster mainGraphicRaycaster; // GraphicRaycaster �� �������� Canvas

    public void StartAnimation()
    {
        mainGraphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        // ������������ ����� ������� � ��������� �����������
        initialPosition = panel.anchoredPosition; // ��������� ��������� ������� ������
        initialSize = panel.sizeDelta; // ��������� ��������� ������ ������
        animator = GetComponent<Animator>();

        // ������� ��������� Canvas
        CreateTemporaryCanvas();

        Vector2 targetPosition = GetCenterOfCanvas();
        StartCoroutine(AnimatePanel(targetPosition));
    }

    Vector2 GetCenterOfCanvas()
    {
        // ��������� ����� �������
        Vector2 canvasCenter = new Vector2(canvas.GetComponent<RectTransform>().rect.width / 2, canvas.GetComponent<RectTransform>().rect.height / 2);
        return canvasCenter;
    }
    Canvas can;
    void CreateTemporaryCanvas()
    {
        // ������� ��������� Canvas � ������������� ��� Sort Order
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
        // ��������� GraphicRaycaster �� �������� Canvas
        mainGraphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        if (mainGraphicRaycaster != null)
        {
            mainGraphicRaycaster.enabled = false;
        }

        imageRock.SetActive(true);

        // ���������� ������ � ����� �������
        yield return MovePanelToCenter(panel, panel.position, targetPosition, animationDuration);

        // ����������� ������
        yield return ResizePanel(panel, panel.sizeDelta, expandedSize, animationDuration);

        // �������� �����������
        animator.Play("Anima");
        DataManager.InstanceData.mapNextLevel.OpenLevel();

        // ���������� ������ � �������� ������
        yield return ResizePanel(panel, expandedSize, initialSize, animationDuration);

        // ���������� ������ � ��������� ���������
        yield return MovePanelBack(panel, initialPosition, animationDuration);

        // �������� GraphicRaycaster �� �������� Canvas
        if (mainGraphicRaycaster != null)
        {
            mainGraphicRaycaster.enabled = true;
        }

        // ������� ��������� Canvas ����� ���������� ��������
        DestroyTemporaryCanvas();

        Debug.Log("�������� ���������.");
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
        Vector2 start = rectTransform.anchoredPosition; // �������� ������� �������� �������

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
