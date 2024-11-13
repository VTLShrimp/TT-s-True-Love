using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChoiceButtonHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI choiceText;
    private Color normalColor = Color.white;
    private Color highlightColor = Color.yellow;

    private void Awake()
    {
        choiceText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            choiceText.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        choiceText.color = normalColor;
    }

}
