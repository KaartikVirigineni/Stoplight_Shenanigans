using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    public Button yourButton;
    public Animator animator;
    public string animationName = "YourAnimation";
    public bool enableCanvas = false;
    public Canvas targetCanvas;
    public string sceneToLoad = "YourSceneName";

    private void Start()
    {
        yourButton.onClick.AddListener(OnButtonPress);
    }

    private void OnButtonPress()
    {
        if (animator != null && !string.IsNullOrEmpty(animationName))
        {
            animator.Play(animationName);
        }
        StartCoroutine(HandleActionAfterAnimation());
    }

    private IEnumerator HandleActionAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (enableCanvas)
        {
            if (targetCanvas != null)
            {
                targetCanvas.gameObject.SetActive(true);
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
