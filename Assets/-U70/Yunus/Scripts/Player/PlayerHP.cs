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
    public float goCheckPointPosTime;

    [HideInInspector] public float hp;
    [HideInInspector] public bool isAlive;

    //[Header("Objects")]
    [HideInInspector] public TextMeshProUGUI hpTxt;
    [HideInInspector] public Image hpImage;

    [Header("Health")]
    public ParticleSystem incHPEffect;                  //increase hp effect


    private void Awake()
    {
        ins = this;
    }
    void Start()
    {
        hpTxt = GameObject.Find("PlayerHPTxt").GetComponent<TextMeshProUGUI>();
        hpImage = GameObject.Find("HPImage").GetComponent<Image>();


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
            GetDamage(other.GetComponent<EnemyWeapon>().enemyHolder.attackDamage, 0.3f, 6);
        }
    }
    public void GetDamage(float damage, float shakeTime, float shakeFre)
    {
        PistolController.ins.ShakeScreenn(shakeTime, damage / 10, shakeFre);

        hp -= damage - damage * armour;

        if (hp <= 0)
        {
            hp = 0;
            Die();
            //buraya normal yollardan öldüðümüz için dirilme kodunu yaz (check point system)
            Invoke(nameof(Resurrect), goCheckPointPosTime);
            CheckPointSystem.ins.PlayerGoCheckPoint(goCheckPointPosTime - 0.5f);
        }

        hpImage.fillAmount = hp / maxHealth;
        hpTxt.text = hp.ToString();
    }
    public void GetDamageTrap(float damage, float shakeTime, float amplitude, float shakeFre)
    {
        PistolController.ins.ShakeScreenn(shakeTime, amplitude, shakeFre);

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
        if (isAlive)
        {
            isAlive = false;
            GetComponent<FirstPersonController>().isAlive = false;

            UIController.ins.YouDied();
        }
    }
    public void Resurrect()//player'ý canlandýr
    {
        IncreaseHP(maxHealth * 0.5f);
        PlayerCollect.ins.UptAmmo(8);
        isAlive = true;
        GetComponent<FirstPersonController>().isAlive = true;

        UIController.ins.Resurrect();
    }

    public void StopOrContinueMove(bool move)
    {
        isAlive = move;
        GetComponent<FirstPersonController>().isAlive = move;
    }
}
