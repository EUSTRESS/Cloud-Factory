using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingSceneController : MonoBehaviour
{
    public Canvas canvas;
    private ChangeCursor mouseCursor;

    private static LoadingSceneController instance;
    public static LoadingSceneController Instance
    {
        get
        {
            if (null == instance) return null;

            return instance;
        }
    }

    void Awake()
    {
        mouseCursor = GameObject.Find("MouseCursorManger").GetComponent<ChangeCursor>();
        // 인스턴스 할당
        if (null == instance)
        {
            instance = this;
            // 모든 씬에서 날짜 계산해야하므로
            // 단, title씬에서는 제외한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // 이미 존재하면 이전부터 사용하던 것을 사용함
            Destroy(this.gameObject);
        }
    }

    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image progressBar;

    private string loadSceneName;
    private int loadSceneInt;
    

    public void LoadScene(string sceneName)
    {
        mouseCursor.SetLoadCursor();
        canvas.sortingOrder = 10;
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());
    }
    public void LoadScene(int sceneNumber)
    {
        mouseCursor.SetLoadCursor();
        canvas.sortingOrder = 10;
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneInt = sceneNumber;
        StartCoroutine(LoadSceneProcessInt());
    }

    private IEnumerator LoadSceneProcess()
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private IEnumerator LoadSceneProcessInt()
    {
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneInt);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        else if (arg0.buildIndex == loadSceneInt)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            mouseCursor.SetDefCursor();
            gameObject.SetActive(false);
        }
    }
}
