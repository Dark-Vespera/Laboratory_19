using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WorkButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText; // Ссылка на текст
    public Color normalColor = Color.white; // Обычный цвет
    public Color hoverColor = Color.yellow; // Цвет при наведении

    void Start()
    {
        if (buttonText == null)
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        
        if (buttonText != null)
            buttonText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = normalColor;
    }
}