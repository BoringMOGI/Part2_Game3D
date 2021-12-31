using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public enum TITLE_MENU
    {
        Main,
        Option,
        // Load,
        // Download,
    }

    [SerializeField] TITLE_MENU firstMenu;
    [SerializeField] GameObject[] menus;

    private void Start()
    {
        ChangeMenu(firstMenu);          // ���۽�
    }

    public void ChangeMenu(TITLE_MENU menu)
    {
        // ��� �޴��� ��ȸ�ϸ鼭 �ŰԺ��� menu�� ���� �޴��� �Ҵ�.
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].gameObject.SetActive((TITLE_MENU)i == menu);
        }
    }

    public void OnStartNewGame()
    {
        // popup..
        // clear save file.
        SceneManager.Instance.LoadNextScene(SceneManager.SCENE.Game);
    }
    public void OnStartContinue()
    {

    }

}
