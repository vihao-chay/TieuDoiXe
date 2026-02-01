using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup introGroup;
    public GameObject menuPanel;
    public Animator introAnimator;
    bool readyToSkip = false;

    void Start()
    {
        menuPanel.SetActive(false);
        introAnimator.Play("FadeIn");
    }

    void Update()
    {
        var state = introAnimator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("FadeIn") && state.normalizedTime >= 1f)
        {
            readyToSkip = true;
        }

        if (readyToSkip && Input.anyKeyDown)
        {
            introAnimator.Play("FadeOut");
        }

        if (state.IsName("FadeOut") && state.normalizedTime >= 1f)
        {
            ShowMenu();  // Gọi function mới
        }
    }

    public void ShowMenu()
    {  // ← THÊM NÀY!
        menuPanel.SetActive(true);
        introGroup.gameObject.SetActive(false);
    }

    public void PlayGame() { SceneManager.LoadScene("GameScene"); }
    public void QuitGame() { Application.Quit(); }
}
