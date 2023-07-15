using DG.Tweening;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public static bool isPaused;

    public CanvasGroup pausePnl;
    public RectTransform sceneTransPnl;
    public Slider soundSl;

    private void Start()
    {
        isPaused = false;

        pausePnl.GetComponent<RectTransform>().DOScale(0, 0);
        pausePnl.DOFade(0, 0);

        AudioListener.volume = PlayerPrefs.GetFloat("soundVolume", 1);
        soundSl.value = PlayerPrefs.GetFloat("soundVolume", 1);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            pausePnl.DOKill();
            pausePnl.GetComponent<RectTransform>().DOKill();

            if (!isPaused)      //oyunu durdur
            {
                isPaused = true;
                Cursor.lockState = CursorLockMode.None;

                Time.timeScale = 0;
                pausePnl.GetComponent<RectTransform>().DOScale(1, 0).SetUpdate(true);
                pausePnl.DOFade(1, 0.4f).SetUpdate(true);

                PlayerHP.ins.StopOrContinueMove(false);
            }
            else
            {
                isPaused = false;
                Cursor.lockState = CursorLockMode.Locked;

                Time.timeScale = 1;
                pausePnl.GetComponent<RectTransform>().DOScale(0, 0).SetDelay(0.2f).SetUpdate(true);
                pausePnl.DOFade(0, 0.2f).SetUpdate(true);

                PlayerHP.ins.StopOrContinueMove(true);
            }
        }
    }

    public void GoMainMenu()
    {
        sceneTransPnl.DOMoveX(1000, 3).SetUpdate(true).OnComplete(() =>
        {
            SceneManager.LoadScene("MainMenuScene");
        });
        
    }
    public void SetSoundVolume()
    {
        PlayerPrefs.SetFloat("soundVolume", soundSl.value);
        AudioListener.volume = soundSl.value;
    }
}
