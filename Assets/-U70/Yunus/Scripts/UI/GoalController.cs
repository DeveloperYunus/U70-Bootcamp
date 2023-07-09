using DG.Tweening;
using TMPro;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public static GoalController ins;  

    public TextMeshProUGUI headerTxt;
    public TextMeshProUGUI objectiveTxt;

    CanvasGroup headerCG, objectiveCG;

    public string[] header;
    public string[] objective;


    private void Awake()
    {
        ins = this;
    }
    void Start()
    {
        headerCG = headerTxt.GetComponent<CanvasGroup>();
        objectiveCG = objectiveTxt.GetComponent<CanvasGroup>();

        Resetobjectives();
        SetObjectives(0);
    }
    
    public void Resetobjectives()
    {
        headerTxt.text = "";
        objectiveTxt.text = "";

        headerCG.DOFade(0, 0.5f);
        objectiveCG.DOFade(0, 0.5f);
    }
    public void SetObjectives(int textNumber)
    {
        headerTxt.text = header[textNumber];
        objectiveTxt.text = objective[textNumber];

        CompassArrow.ins.SetTarget(textNumber);             //bu numara ayný zamanda target transformalar içinde kullanýlabilir

        headerCG.DOKill();
        objectiveCG.DOKill();

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