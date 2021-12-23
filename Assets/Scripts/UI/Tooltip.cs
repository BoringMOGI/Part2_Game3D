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
        tipRectTransform = GetComponent<RectTransform>();       // ���� Rect�� �˻��Ѵ�.
        tipHeight = tipRectTransform.rect.height;               // Rect���� height���� �����Ѵ�.

        Close(); 
    }

    public void Show(string tip)
    {
        string[] splits = tip.Split('\n');

        float width = tipRectTransform.sizeDelta.x;     // �ʺ� ���.
        float height = tipHeight * splits.Length;       // ���� ���.

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
        transform.position = Input.mousePosition;       // ���� ��ġ�� ���콺 �������� ��ġ��.
    }
}
