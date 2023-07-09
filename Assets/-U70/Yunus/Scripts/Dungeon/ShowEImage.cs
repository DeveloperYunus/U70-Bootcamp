using DG.Tweening;
using UnityEngine;

public class ShowEImage : MonoBehaviour
{
    public CanvasGroup eImage;
    public string eKeyFunction;

    private void Start()
    {
        ShowEImagee(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        ShowEImagee(true);
    }
    private void OnTriggerExit(Collider other)
    {
        ShowEImagee(false);
    }

    void ShowEImagee(bool showing)
    {
        if (showing)
        {
            PressEKey.isEKeyActive = true;
            PressEKey.eKeyFunction = eKeyFunction;

            eImage.DOKill();
            eImage.DOFade(1, 0.5f);
        }
        else
        {
            PressEKey.isEKeyActive = false;
            PressEKey.eKeyFunction = null;

            eImage.DOKill();
            eImage.DOFade(0, 0.5f);
        }
    }
}
