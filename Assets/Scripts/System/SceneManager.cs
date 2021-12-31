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
        //LoadSceneMode.Single : �ش� �� �ϳ��� �ܵ����� �ҷ����ڴ�. (������ �ִ� ���� ��ε� ��)
        //LoadSceneMode.Additive : �ش� ���� �߰��ؼ� �ε��Ѵ� (������ �ִ� ���� ��ε� ��������)
        UnityEngine.SceneManagement.SceneManager.LoadScene(SCENE.Option.ToString(), LoadSceneMode.Additive);
    }

    IEnumerator FadeIn(float fadeTime)
    {
        panel.gameObject.SetActive(true);   //�г��� Ȱ��ȭ
        panel.color = fadeColor;    //������ �������� ����

        float time = fadeTime;

        while (true)
        {
            //time���� �ð�����ŭ ���µ� 0.0f���϶�� panel�� ���� while ����
            if ((time -= Time.deltaTime) <= 0.0f)
            {
                panel.gameObject.SetActive(false);

                break;
            }

            fadeColor.a = time / fadeTime;  //fade���� ���İ��� ����
            panel.color = fadeColor;    //���� ����

            yield return null;
        }

        fadeColor.a = 0f;   //���İ��� 0f�� ����
    }
    IEnumerator FadeOut(float fadeTime, string nextScene)
    {
        panel.gameObject.SetActive(true);   //�г��� Ȱ��ȭ
        panel.color = fadeColor;    //fade ���� ����

        float time = 0.0f;  

        while (true)
        {
            if ((time += Time.deltaTime) >= fadeTime)   //time�� �ð����� ����
            {
                break;
            }

            fadeColor.a = time / fadeTime;  //���İ� ���
            panel.color = fadeColor;    //���� ����

            yield return null;
        }

        fadeColor.a = 1f;   //���İ��� 1f�� ����
        panel.color = fadeColor;    //panel�� ���� ����

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);  //���� �� �ε�
    }
}
