using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    // �V���O���g���̃C���X�^���X
    public static Fade Instance { get; private set; } 

    // �t�F�[�h�p��Image
    public Image fadeImage;

    // �t�F�[�h����
    public float fadeDuration = 2.0f;

    private void Awake()
    {
        // �V���O���g���̐ݒ�
        if (Instance == null)
        {
            Instance = this;
            // �V�[���Ԃł��̃I�u�W�F�N�g��ێ�
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���ɑ��݂���ꍇ�͔j��
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // �V�[���J�n���Ƀt�F�[�h�C��
        FadeIn();
    }

    public void ChangeScene(string sceneName)
    {
        // �t�F�[�h�A�E�g �� �V�[���J�� �� �t�F�[�h�C��
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

        // �m���ɍŏI�l��ݒ�
        color.a = endAlpha; 
        fadeImage.color = color;

        // �t�F�[�h������̏��������s
        onComplete?.Invoke(); 
    }
}
