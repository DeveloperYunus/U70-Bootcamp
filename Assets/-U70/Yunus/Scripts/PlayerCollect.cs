using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public static PlayerCollect ins;

    //[Header("Text")]
    [HideInInspector] public TextMeshProUGUI totalAmmoTxt;
    [HideInInspector] public TextMeshProUGUI collectTxt;

    [Header("Ammo")]
    public CollectableObj bullet;
    public int bulletIncAmount;

    public CollectableObj shipBall;
    public int shipBallIncAmount;

    [Header("HP")]
    [Range(0f, 1f)]
    public float eatHpPercent;                          //when player eat, increase his hp "maxHP * eatHpPercent"
    float eatHPAmount;

    private void Awake()
    {
        ins = this;
    }
    private void Start()
    {
        totalAmmoTxt = GameObject.Find("MaxPocketAmmoTxt").GetComponent<TextMeshProUGUI>();
        collectTxt = GameObject.Find("CollectTxt").GetComponent<TextMeshProUGUI>();

        eatHPAmount = eatHpPercent * PlayerHP.ins.maxHealth;

        UptAmmo(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && PistolController.ins.bullet.ammoAmount < PistolController.ins.maxPocketAmmo)
        {
            AudioManager.ins.PlaySound("ammo");

            Destroy(other.gameObject);

            UptCollectTxt(bulletIncAmount, " Bullet", Color.grey);

            UptAmmo(bulletIncAmount);
        }
        if (other.CompareTag("ShipBall"))
        {
            AudioManager.ins.PlaySound("ammo");

            Destroy(other.gameObject);
            
            shipBall.ammoAmount += shipBallIncAmount;

            UptCollectTxt(shipBallIncAmount, " Cannon Ball", Color.grey);
        }
        if (other.CompareTag("Eat") && PlayerHP.ins.hp < PlayerHP.ins.maxHealth)
        {
            if (Random.Range(0,2)==0)
                AudioManager.ins.PlaySound("eat1");
            else
                AudioManager.ins.PlaySound("eat2");

            Destroy(other.gameObject);

            PlayerHP.ins.IncreaseHP(eatHPAmount);
            UptCollectTxt(eatHPAmount, " health", Color.green);
        }
    }

    void UptCollectTxt(float amount, string kind, Color color)
    {
        collectTxt.text = "+ " + amount.ToString() + kind;
        collectTxt.color = color;

        collectTxt.GetComponent<RectTransform>().DOKill();
        collectTxt.GetComponent<CanvasGroup>().DOKill();

        collectTxt.GetComponent<RectTransform>().DOScale(1.1f, 0.2f);
        collectTxt.GetComponent<RectTransform>().DOScale(1f, 0.3f).SetDelay(0.2f);

        collectTxt.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
        collectTxt.GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetDelay(0.5f);
    }

    public void UptAmmo(int incOrReduAmmo)         //increase or reduce ammo 
    {
        bullet.ammoAmount += incOrReduAmmo;

        if (bullet.ammoAmount > PistolController.ins.maxPocketAmmo)
            bullet.ammoAmount = PistolController.ins.maxPocketAmmo;

        totalAmmoTxt.text = bullet.ammoAmount.ToString() + " / " + PistolController.ins.maxPocketAmmo.ToString();
    }
}
