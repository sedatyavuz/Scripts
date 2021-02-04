using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenManager : MonoBehaviour
{
    [SerializeField] private Animator fadeAnim;
    [SerializeField] private GameObject[] logos;
    [SerializeField] private float duration = 0.5f;


    private AsyncOperation sceneLoadAsyncOp;

    void Start()
    {
        StartCoroutine("StartLoading");
    }

    private IEnumerator StartLoading()
    {
        sceneLoadAsyncOp = SceneManager.LoadSceneAsync("MainMenu");
        sceneLoadAsyncOp.allowSceneActivation = false;

        for (int i = 0; i < logos.Length; i++)
        {
            logos[i].SetActive(true);
            fadeAnim.Play("FadeOut");
            yield return new WaitForSeconds(fadeAnim.GetCurrentAnimatorStateInfo(0).length + duration);

            fadeAnim.Play("FadeIn");
            yield return new WaitForSeconds(fadeAnim.GetCurrentAnimatorStateInfo(0).length);
            logos[i].SetActive(false);
        }
        sceneLoadAsyncOp.allowSceneActivation = true;
    }
}
