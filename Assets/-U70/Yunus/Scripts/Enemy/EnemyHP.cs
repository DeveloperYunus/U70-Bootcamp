using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    [Header("HP")]
    public float maxHealth;
    [Range(0f, 1f)]
    public float armour;

    float hp;
    bool dead;

    [Header("UI Objects")]
    public RectTransform hpCanvas;
    public TextMeshProUGUI hpTxt;
    public Image hpImage;

    [Header("Drop Collectable")]
    [Range(0, 100)] public int dropRate;
    public float bulletDieTime;


    void Start()
    {
        hp = maxHealth;
        dead = false;

        hpImage.fillAmount = 1;
        hpTxt.text = hp.ToString();
    }
    public void GetDamage(float damage)
    {
        if (enabled)
        {
            AudioManager.ins.PlaySound("enemyHit");

            hp -= damage - damage * armour;

            if (hp <= 0)
            {
                hp = 0;
                Die();
            }

            hpImage.fillAmount = hp / maxHealth;
            hpTxt.text = hp.ToString();
            GetComponent<EnemyNavMesh>().FollowPlayer();
        }
    }

    void Die()
    {
        if (!dead)
        {
            AudioManager.ins.PlaySound("enemyDie");

            dead = true;
            UIDisappear();

            //Die type
            if (GetComponent<EnemyNavMesh>())
            {
                InsBullet();

                GetComponent<EnemyNavMesh>().Die();
            }
            else
                Destroy(gameObject);
        }
    }
    void UIDisappear()
    {
        hpCanvas.DOScale(hpCanvas.localScale.x * 1.4f, 1f).SetEase(Ease.OutBack);
        hpCanvas.GetComponent<CanvasGroup>().DOFade(0, 1f);
    }
    void InsBullet()
    {
        if (Random.Range(0, 100) < dropRate)
        {
            GeneralPool.BulletAmmo(transform.position, bulletDieTime);
        }
    }

    public void SetSkeletonStats(float hpS, float armourS, float damage)
    {
        maxHealth = hpS;
        hp = hpS;
        armour = armourS;
        GetComponent<EnemyNavMesh>().attackDamage = damage;
    }
}
