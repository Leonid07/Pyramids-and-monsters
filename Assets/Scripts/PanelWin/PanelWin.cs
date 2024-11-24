using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelWin : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float duration = 0.5f;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void StartAnim()
    {
        StartCoroutine(AnimateAlpha());
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void ClosePanel()
    {
        StartCoroutine(AnimateAlphaClose());
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator AnimateAlphaClose()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }
    }

    private IEnumerator AnimateAlpha()
    {
        float elapsedTime = 0f;
        // Увеличение alpha обратно до 1
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }
    }
}
