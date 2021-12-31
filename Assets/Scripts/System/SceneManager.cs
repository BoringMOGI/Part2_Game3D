using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    public enum SCENE
    {
        Main,
        Game,
        Option,
    }

    [SerializeField] private Image panel;

    private static SCENE currentScene;

    Color fadeColor;
    bool isLoading;

    private void Start()
    {
        fadeColor = new Color(0f, 0f, 0f, 1f);

        StartCoroutine(FadeIn(1.0f));
    }

    public void LoadNextScene(SCENE nextScene, float fadeTime = 1f)
    {
        if (isLoading)
        {
            return;
        }

        currentScene = nextScene;
        isLoading = true;

        StartCoroutine(FadeOut(fadeTime, nextScene.ToString()));
    }

    public void CloseOption()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(SCENE.Option.ToString());
    }
    public void OpenOption()
    {
        //LoadSceneMode.Single : 해당 씬 하나만 단독으로 불러오겠다. (기존에 있던 씬은 언로드 됨)
        //LoadSceneMode.Additive : 해당 씬을 추가해서 로드한다 (기존에 있던 씬은 언로드 되지않음)
        UnityEngine.SceneManagement.SceneManager.LoadScene(SCENE.Option.ToString(), LoadSceneMode.Additive);
    }

    IEnumerator FadeIn(float fadeTime)
    {
        panel.gameObject.SetActive(true);   //패널을 활성화
        panel.color = fadeColor;    //색상은 불투명한 검정

        float time = fadeTime;

        while (true)
        {
            //time값을 시간값만큼 빼는데 0.0f이하라면 panel을 끄고 while 종료
            if ((time -= Time.deltaTime) <= 0.0f)
            {
                panel.gameObject.SetActive(false);

                break;
            }

            fadeColor.a = time / fadeTime;  //fade색상에 알파값을 줄임
            panel.color = fadeColor;    //색상 대입

            yield return null;
        }

        fadeColor.a = 0f;   //알파값을 0f로 조정
    }
    IEnumerator FadeOut(float fadeTime, string nextScene)
    {
        panel.gameObject.SetActive(true);   //패널을 활성화
        panel.color = fadeColor;    //fade 색상 대입

        float time = 0.0f;  

        while (true)
        {
            if ((time += Time.deltaTime) >= fadeTime)   //time에 시간값을 더함
            {
                break;
            }

            fadeColor.a = time / fadeTime;  //알파값 계산
            panel.color = fadeColor;    //색상 대입

            yield return null;
        }

        fadeColor.a = 1f;   //알파값을 1f로 고정
        panel.color = fadeColor;    //panel에 색상 대입

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);  //다음 씬 로드
    }
}
