using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tooltip : Singleton<Tooltip>
{
    [SerializeField] Text tipText;

    RectTransform tipRectTransform;

    float tipHeight;

    private void Start()
    {
        tipRectTransform = GetComponent<RectTransform>();       // 나의 Rect를 검색한다.
        tipHeight = tipRectTransform.rect.height;               // Rect에서 height값을 대입한다.

        Close(); 
    }

    public void Show(string tip)
    {
        string[] splits = tip.Split('\n');

        float width = tipRectTransform.sizeDelta.x;     // 너비 계산.
        float height = tipHeight * splits.Length;       // 높이 계산.

        tipRectTransform.sizeDelta = new Vector2(width, height);
        gameObject.SetActive(true);
        tipText.text = tip;
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;       // 나의 위치를 마우스 포인터의 위치로.
    }
}
