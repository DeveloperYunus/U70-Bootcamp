using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public static PlayerHP ins;

    [Header("HP")]
    public float maxHealth;
    [Range(0f, 1f)]
    public float armour;

    [HideInInspector] public float hp;
    [HideInInspector] public bool isAlive;

    [Header("Objects")]
    public TextMeshProUGUI hpTxt;
    public Image hpImage;

    [Header("Health")]
    public ParticleSystem incHPEffect;                  //increase hp effect


    private void Awake()
    {
        ins = this;
    }
    void Start()
    {
        hp = maxHealth;

        isAlive = true;

        hpImage.fillAmount = 1;
        hpTxt.text = hp.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon") && other.GetComponent<EnemyWeapon>().enemyHolder.canGiveDmg)
        {
            other.GetComponent<EnemyWeapon>().enemyHolder.canGiveDmg = false;
            GetDamage(other.GetComponent<EnemyWeapon>().enemyHolder.attackDamage);
        }
    }
    public void GetDamage(float damage)
    {
        PistolController.ins.ShakeScreenn(0.3f, damage / 10, 6);

        hp -= damage - damage * armour;

        if (hp <= 0)
        {
            hp = 0;
            Die();
        }

        hpImage.fillAmount = hp / maxHealth;
        hpTxt.text = hp.ToString();
    }
    public void IncreaseHP(float value)
    {
        hp += value;
        incHPEffect.Play();

        if (hp > maxHealth)
        {
            hp = maxHealth;
        }

        hpImage.fillAmount = hp / maxHealth;
        hpTxt.text = hp.ToString();
    }

    void Die()
    {
        isAlive = false;
        GetComponent<FirstPersonController>().isAlive = false;

        UIController.ins.YouDied();
    }
    public void Resurrect()//player'� canland�r
    {
        IncreaseHP(maxHealth);
        isAlive = true;
        GetComponent<FirstPersonController>().isAlive = true;

        UIController.ins.Resurrect();
    }
}
