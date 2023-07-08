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
        papirusImg.DOFade(0, 1f);
    }
    void EKeyFunction()
    {
        switch (eKeyFunction)
        {
            case "FishIsleChest":
                papirusImg.DOFade(1, 0.5f);
                papirusTxt.text = goTowerIsleTxt;
                print("Update goal text");
                break;
        }
    }
}
