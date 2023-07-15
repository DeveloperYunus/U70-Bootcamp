using DG.Tweening;
using TMPro;
using UnityEngine;

public class OceanDoor : MonoBehaviour
{
    public CanvasGroup doNotPassTxt;
    private void Start()
    {
        doNotPassTxt.DOFade(0, 0f);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            doNotPassTxt.GetComponent<TextMeshProUGUI>().text = "- You have to kill \"Pirate Captan\"";

            doNotPassTxt.DOKill();
            doNotPassTxt.DOFade(1, 0.3f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            doNotPassTxt.GetComponent<TextMeshProUGUI>().text = "";

            doNotPassTxt.DOKill();
            doNotPassTxt.DOFade(0, 0.5f);
        }
    }
}
