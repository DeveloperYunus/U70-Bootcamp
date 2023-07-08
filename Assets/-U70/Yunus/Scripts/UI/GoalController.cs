using DG.Tweening;
using TMPro;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public TextMeshProUGUI headerTxt;
    public TextMeshProUGUI objectiveTxt;

    CanvasGroup headerCG, objectiveCG;

    void Start()
    {
        headerCG = headerTxt.GetComponent<CanvasGroup>();
        objectiveCG = objectiveTxt.GetComponent<CanvasGroup>();


        Resetobjectives();
    }
    
    public void Resetobjectives()
    {
        headerTxt.text = "";
        objectiveTxt.text = "";

        headerCG.DOFade(0, 0.5f);
        objectiveCG.DOFade(0, 0.5f);
    }
    public void SetObjectives(string header, string objective)
    {
        headerTxt.text = header;
        objectiveTxt.text = objective;

        headerCG.DOFade(1, 0.2f);
        objectiveCG.DOFade(1, 0.2f).SetDelay(0.2f);

        SetScale(headerTxt.GetComponent<RectTransform>(), 0.5f);
        SetScale(objectiveTxt.GetComponent<RectTransform>(), 0.5f);
    }

    void SetScale(RectTransform text, float waitTime)
    {
        text.DOScale(1.1f, 0.2f).SetDelay(waitTime);
        text.DOScale(1f, 0.2f).SetDelay(0.2f + waitTime);
    }
}