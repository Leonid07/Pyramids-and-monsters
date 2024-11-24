using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationPAnel : MonoBehaviour
{
    //public float duration = 1.0f;
    //public float delay = 1.0f;
    //public float rotationSpeed = 90f; // Скорость вращения в градусах в секунду
    public float delayBeforeReturn = 2f;
    public float fadeDuration = 1f;
    CanvasGroup canvasGroup;

    private Vector2 initialPosition;
    private bool hasLoggedMessage = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartAnimation(GameObject panel, bool isActive = false)
    {
        StartCoroutine(MovePanelOutAndBack(panel, isActive));
    }
    //public void StartAnimationUnLockLevel(GameObject panel)
    //{
    //StartCoroutine(AnimatePanelUnlock(panel));
    //}
    //private IEnumerator AnimatePanelUnlock(GameObject panel)
    //{
    //    panel.SetActive(true);

    //    yield return new WaitForSeconds(delay);
    //}

    public IEnumerator MovePanelOutAndBack(GameObject panel, bool isActive = false, GameObject[] panels = null)
    {
        // Осветление (увеличение прозрачности до 1)
        float elapsedTime = 1f;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Задержка перед возвратом
        yield return new WaitForSeconds(delayBeforeReturn);

        if (!isActive)
        {
            panel.SetActive(false);
            if (panels != null)
            {
                foreach (var p in panels)
                {
                    p.SetActive(false);
                }
            }
        }
        else
        {
            panel.SetActive(true);
            if (panels != null)
            {
                foreach (var p in panels)
                {
                    p.SetActive(true);
                }
            }
        }
        hasLoggedMessage = true;

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        hasLoggedMessage = false; // Сбрасываем флаг для следующей анимации
    }
}