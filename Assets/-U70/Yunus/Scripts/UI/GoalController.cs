using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    public static GoalController ins;  

    public TextMeshProUGUI headerTxt;
    public TextMeshProUGUI objectiveTxt;

    CanvasGroup headerCG, objectiveCG;

    public string[] header;
    public string[] objective;

    public static int goalValue;                //sahne yüklenmelerinde deðer deðiþmesin diye buna atýyoruz (normalde bu þekilde yapmak mantýklý deðil ama zaman az :D)

    private void Awake()
    {
        ins = this;
        if (SceneManager.GetActiveScene().name == "MainScene")
            goalValue = 0;
    }
    void Start()
    {
        headerCG = headerTxt.GetComponent<CanvasGroup>();
        objectiveCG = objectiveTxt.GetComponent<CanvasGroup>();

        Resetobjectives();
        SetObjectives(goalValue);
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

        if (CompassArrow.ins) 
            CompassArrow.ins.SetTarget(textNumber);             //bu numara ayný zamanda target transformalar içinde kullanýlabilir
        goalValue = textNumber;

        headerCG.DOKill();
        objectiveCG.DOKill();

        headerCG.DOFade(1, 0.2f);
        objectiveCG.DOFade(1, 0.2f).SetDelay(0.2f);

        SetScale(headerTxt.GetComponent<RectTransform>(), 0.1f);
        SetScale(objectiveTxt.GetComponent<RectTransform>(), 0.1f);
    }

    void SetScale(RectTransform text, float waitTime)
    {
        text.DOScale(1.1f, 0.2f).SetDelay(waitTime);
        text.DOScale(1f, 0.2f).SetDelay(0.2f + waitTime);
    }
}