using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressEKey : MonoBehaviour
{
    public static bool isEKeyActive;
    public static string eKeyFunction;
    bool isPlayerHadKey;                                //dunageona girmek i�in gerekli anahtar -- tower kulesindeki sand�kta gizli

    public int targetFPS;

    [Header("--- NEEDS FOR FUNCTIONS---")]
    public CanvasGroup papirusImg;
    public TextMeshProUGUI papirusTxt;

    [Header("--- Fish Isle ---")]
    public string goTowerIsleTxt;

    [Header("--- Tower Isle ---")]
    public float transitionTime;
    public Transform playerCapsule;
    public string goCastleIsleTxt;
    public GameObject keyObj;

    public Transform[] pos;

    [Header("--- Castle Isle ---")]
    public RectTransform sceneTransition;
    public float sceneUpDownTime;
    public CanvasGroup doNotPassTxt;

    [Header("--- Castle Isle ---")]
    public string winGameTxt;
    public CanvasGroup endGameTxt;
    public float winChestReadTime;
    public float endGameReadTime;


    private void Start()
    {
        Application.targetFrameRate = targetFPS;

        doNotPassTxt.DOFade(0, 0);

        sceneTransition.GetComponent<RectTransform>().DOScale(1, 0).SetUpdate(true);
        sceneTransition.DOMoveX(-2000, sceneUpDownTime).SetUpdate(true).SetDelay(3f);
        isEKeyActive = false;

        papirusImg.alpha = 0;
        papirusImg.GetComponent<RectTransform>().DOScale(0f, 0f);

        endGameTxt.alpha = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isEKeyActive && eKeyFunction != null) 
        {
            EKeyFunction();
        }
    }

    public void ClosePapirusImg()
    {
        SetCursorState(true);
        PlayerHP.ins.StopOrContinueMove(true);

        papirusImg.DOFade(0, 0.5f);
        papirusImg.GetComponent<RectTransform>().DOScale(0f, 0f).SetDelay(0.5f);
    }
    public void OpenPapirusImg(string text)
    {
        SetCursorState(false);
        PlayerHP.ins.StopOrContinueMove(false);

        papirusImg.DOFade(1, 0.5f);
        papirusImg.GetComponent<RectTransform>().DOScale(1f, 0f);

        papirusTxt.text = text;
    }

    void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void EKeyFunction()
    {
        AudioManager.ins.PlaySound("eKey");

        switch (eKeyFunction)
        {
            case "FishIsleChest":
                OpenPapirusImg(goTowerIsleTxt);
                GoalController.ins.SetObjectives(1);                //goal text bu sat�r ile g�ncellenir
                break;

            case "GoTowerFirstFloor":
                PlayerHP.ins.StopOrContinueMove(false);
                playerCapsule.DOMove(pos[0].position, transitionTime).OnComplete(() => PlayerHP.ins.StopOrContinueMove(true));
                break;

            case "GoTowerZeroFloor":
                PlayerHP.ins.StopOrContinueMove(false);
                playerCapsule.DOMove(pos[1].position, transitionTime).OnComplete(() => PlayerHP.ins.StopOrContinueMove(true));
                break;

            case "GoTowerFirstFloor2":
                PlayerHP.ins.StopOrContinueMove(false);
                playerCapsule.DOMove(pos[2].position, transitionTime).OnComplete(() => PlayerHP.ins.StopOrContinueMove(true));
                break;

            case "GoTowerSecondFloor":
                PlayerHP.ins.StopOrContinueMove(false);
                playerCapsule.DOMove(pos[3].position, transitionTime).OnComplete(() => PlayerHP.ins.StopOrContinueMove(true));
                break;

            case "TowerIsleChest":
                keyObj.SetActive(false);
                isPlayerHadKey = true;

                OpenPapirusImg(goCastleIsleTxt);
                GoalController.ins.SetObjectives(2);                //goal text bu sat�r ile g�ncellenir
                break;

            case "GoDungeon":
                if (isPlayerHadKey)
                {
                    sceneTransition.DOMoveX(1000, sceneUpDownTime).SetUpdate(true).OnComplete(() =>
                    {
                        SceneManager.LoadScene("BossRoom");
                    });

                    GoalController.ins.SetObjectives(3);
                }
                else
                {
                    doNotPassTxt.GetComponent<TextMeshProUGUI>().text = "- You do not have \"The Key\"!";

                    doNotPassTxt.DOKill();
                    doNotPassTxt.DOFade(1, 0.3f);
                    doNotPassTxt.DOFade(0, 0.5f).SetDelay(2);
                }
                break;

            case "WinGameChest":
                OpenPapirusImg(winGameTxt);
                Invoke(nameof(WinGameFunc), winChestReadTime);

                Invoke(nameof(GoLevelScene), endGameReadTime + winChestReadTime);
                break;
                
        }
    }

    void WinGameFunc()
    {
        PlayerHP.ins.StopOrContinueMove(false);
        
        sceneTransition.DOMoveX(1000, sceneUpDownTime).OnComplete(() =>
        {
            endGameTxt.DOFade(1, 2f);
        });
    }

    void GoLevelScene() 
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
