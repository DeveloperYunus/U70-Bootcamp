using DG.Tweening;
using UnityEngine;

public class SeasonTrapShowEImage : MonoBehaviour
{
    public int pillerType;
    public CanvasGroup eImage;

    private void OnTriggerEnter(Collider other)
    {
        ShowEImage(true);
    }
    private void OnTriggerExit(Collider other)
    {
        ShowEImage(false);
    }

    void ShowEImage(bool showing)
    {
        if (showing)
        {
            eImage.DOKill();
            eImage.DOFade(1, 0.5f);
        }
        else
        {
            eImage.DOKill();
            eImage.DOFade(0, 0.5f);
        }

        SeasonTrap.ins.SetPillerType(pillerType);
    }
}
