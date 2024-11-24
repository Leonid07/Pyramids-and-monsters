using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePanel : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float duration = 1.0f;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void StartAnim()
    {
        StartCoroutine(AnimateAlpha());
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

        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0f; // Сброс elapsedTime перед началом второй анимации

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }
    }
}
