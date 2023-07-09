using DG.Tweening;
using TMPro;
using UnityEngine;

public class PressEKey : MonoBehaviour
{
    public static bool isEKeyActive;
    public static string eKeyFunction;

    [Header("--- NEEDS FOR FUNCTIONS---")]
    public CanvasGroup papirusImg;
    public TextMeshProUGUI papirusTxt;

    [Header("--- Fish Isle ---")]
    public string goTowerIsleTxt;

    [Header("--- Tower Isle ---")]
    public float transitionTime;
    public Transform playerCapsule;
    public string goCastleIsleTxt;

    public Transform[] pos;


    private void Start()
    {
        isEKeyActive = false;

        papirusImg.alpha = 0;
        papirusImg.GetComponent<RectTransform>().DOScale(0f, 0f);
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
        switch (eKeyFunction)
        {
            case "FishIsleChest":
                OpenPapirusImg(goTowerIsleTxt);
                GoalController.ins.SetObjectives(1);                //goal text bu satýr ile güncellenir
                break;

            case "GoTowerFirstFloor":
                playerCapsule.DOMove(pos[0].position, transitionTime);
                break;

            case "GoTowerZeroFloor":
                playerCapsule.DOMove(pos[1].position, transitionTime);
                break;

            case "GoTowerFirstFloor2":
                playerCapsule.DOMove(pos[2].position, transitionTime);
                break;

            case "GoTowerSecondFloor":
                playerCapsule.DOMove(pos[3].position, transitionTime);
                break;

            case "TowerIsleChest":
                OpenPapirusImg(goCastleIsleTxt);
                GoalController.ins.SetObjectives(2);                //goal text bu satýr ile güncellenir
                break;
        }
    }
}
