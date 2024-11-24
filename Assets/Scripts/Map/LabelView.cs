using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelView : MonoBehaviour
{
    public Text textComponent; // Ссылка на компонент Text в Unity
    public float duration = 1.0f; // Продолжительность увеличения и уменьшения
    public float delay = 1.0f; // Задержка перед уменьшением

    public string noyHealth = "Refill your health in the store";
    public string noyPower = "Replenish your power in the store";
             

    private void Start()
    {
        textComponent = GetComponent<Text>();
    }

    public void TextHeath()
    {
        textComponent.text = noyHealth;
    }
    public void TextPower()
    {
        textComponent.text = noyPower;
    }

    public void StartAnim()
    {
        StartCoroutine(FadeTextAlpha());
    }

    private IEnumerator FadeTextAlpha()
    {
        // Увеличение альфа-канала
        float elapsedTime = 0f;
        Color startColor = textComponent.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Альфа = 1 (255)

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startColor.a, endColor.a, elapsedTime / duration);
            textComponent.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textComponent.color = endColor; // Убедитесь, что альфа достигла 1

        // Задержка
        yield return new WaitForSeconds(delay);

        // Уменьшение альфа-канала
        elapsedTime = 0f;
        startColor = textComponent.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Альфа = 0

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startColor.a, endColor.a, elapsedTime / duration);
            textComponent.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textComponent.color = endColor; // Убедитесь, что альфа достигла 0
    }
}
