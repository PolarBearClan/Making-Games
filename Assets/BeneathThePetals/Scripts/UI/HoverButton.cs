using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Ease hoverEase = Ease.OutBack;
    [SerializeField] private Ease exitEase = Ease.InBack;

    private Image[] images;
    private TMP_Text[] textComponents;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        if (GetComponentsInChildren<Image>() != null)
        {
            images = GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                image.enabled = false;
            }
        }

        if (GetComponentsInChildren<TMP_Text>() != null)
        {
            textComponents = GetComponentsInChildren<TMP_Text>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowHoverEffects();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideHoverEffects();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        HideHoverEffects();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowHoverEffects();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HideHoverEffects();
    }

    private void ShowHoverEffects()
    {
        foreach (var image in images)
        {
            image.enabled = true;
        }

        foreach (var text in textComponents)
        {
            text.transform.DOScale(originalScale * scaleFactor, animationDuration).SetEase(hoverEase);
        }
    }

    private void HideHoverEffects()
    {
        foreach (var image in images)
        {
            image.enabled = false;
        }

        foreach (var text in textComponents)
        {
            text.transform.DOScale(originalScale, animationDuration).SetEase(exitEase);
        }
    }
}
