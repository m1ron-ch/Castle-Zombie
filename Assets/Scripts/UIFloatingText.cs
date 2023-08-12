using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIFloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _icon;
    [SerializeField] private float _destroyTime = 1.5f;

    public void ShowText(Vector3 position, int amountResources, UnityEngine.Sprite icon)
    {
        transform.position = position;
        transform.rotation = Camera.main.transform.rotation;
        transform.DOMoveY(3, 0.3f);
        _text.text = $"+{amountResources}";
        _icon.sprite = icon;

        Invoke(nameof(HideText), _destroyTime);
    }

    private void HideText()
    {
        float duration = 0.7f;
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_text.DOFade(0, duration));
        sequence.Join(_icon.DOFade(0, duration));

        sequence.OnComplete(() => Destroy(gameObject));
    }
}
