using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    // シングルトンのインスタンス
    public static Fade Instance { get; private set; } 

    // フェード用のImage
    public Image fadeImage;

    // フェード時間
    public float fadeDuration = 2.0f;

    private void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            // シーン間でこのオブジェクトを保持
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 既に存在する場合は破棄
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // シーン開始時にフェードイン
        FadeIn();
    }

    public void ChangeScene(string sceneName)
    {
        // フェードアウト → シーン遷移 → フェードイン
        FadeOut(() => SceneManager.LoadScene(sceneName));
    }

    private void FadeIn(Action onComplete = null)
    {
        StartCoroutine(FadeEffect(1, 0, onComplete));
    }

    private void FadeOut(Action onComplete = null)
    {
        StartCoroutine(FadeEffect(0, 1, onComplete));
    }

    private IEnumerator FadeEffect(float startAlpha, float endAlpha, Action onComplete)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // 確実に最終値を設定
        color.a = endAlpha; 
        fadeImage.color = color;

        // フェード完了後の処理を実行
        onComplete?.Invoke(); 
    }
}
