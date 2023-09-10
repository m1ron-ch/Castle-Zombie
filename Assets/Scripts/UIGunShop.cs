using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class UIGunShop : MonoBehaviour
{
    [SerializeField] private List<UIGunShopContent> _contents = new();

    private bool _isOpen = true;
    private RectTransform panelRectTransform;

    private void Awake()
    {
        panelRectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (_isOpen)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log("TOuch");
                    Vector2 touchPosition = touch.position;
                    Debug.Log(!RectTransformUtility.RectangleContainsScreenPoint(panelRectTransform, touchPosition));
                    if (!RectTransformUtility.RectangleContainsScreenPoint(panelRectTransform, touchPosition))
                    {
                        Hide();
                    }
                }
            }
        }
    }

    public void Show()
    {
        transform.gameObject.SetActive(true);
        _isOpen = true;
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
        _isOpen = false;
    }
}
