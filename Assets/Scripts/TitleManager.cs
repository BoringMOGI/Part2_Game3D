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
        ChangeMenu(firstMenu);          // 시작시
    }

    public void ChangeMenu(TITLE_MENU menu)
    {
        // 모든 메뉴를 순회하면서 매게변수 menu와 같은 메뉴만 켠다.
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
