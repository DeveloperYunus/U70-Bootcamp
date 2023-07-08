using DG.Tweening;
using TMPro;
using UnityEngine;

public class PressEKey : MonoBehaviour
{
    public static bool isEKeyActive;
    public static string eKeyFunction;

    [Header("--- Needs For Functions ---")]
    public CanvasGroup papirusImg;
    public TextMeshProUGUI papirusTxt;
    public string goTowerIsleTxt;

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
    void UpdateGoalTxt()
    {
        print("Update goal text");
    }

    void EKeyFunction()
    {
        switch (eKeyFunction)
        {
            case "FishIsleChest":
                OpenPapirusImg(goTowerIsleTxt);

                UpdateGoalTxt();
                break;
        }
    }
}
