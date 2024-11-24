using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelView : MonoBehaviour
{
    public Text textComponent; // ������ �� ��������� Text � Unity
    public float duration = 1.0f; // ����������������� ���������� � ����������
    public float delay = 1.0f; // �������� ����� �����������

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
        // ���������� �����-������
        float elapsedTime = 0f;
        Color startColor = textComponent.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // ����� = 1 (255)

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startColor.a, endColor.a, elapsedTime / duration);
            textComponent.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textComponent.color = endColor; // ���������, ��� ����� �������� 1

        // ��������
        yield return new WaitForSeconds(delay);

        // ���������� �����-������
        elapsedTime = 0f;
        startColor = textComponent.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // ����� = 0

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startColor.a, endColor.a, elapsedTime / duration);
            textComponent.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textComponent.color = endColor; // ���������, ��� ����� �������� 0
    }
}
