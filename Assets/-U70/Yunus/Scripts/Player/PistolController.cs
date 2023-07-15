using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public class PistolController : MonoBehaviour
{
    public static PistolController ins;

    [Header("Pistol Follow")]
    Transform pistolObj;
    public Transform followPivot;
    public float followSpeed;


    [Header("Pistol Anim")]
    public Animator pistolAnim;
    public float attackSpeed, reloadTime;

    bool canAtk;

    [Header("Is Pistol In Ground")]
    public Transform wallTestRayPos;
    public float rayRange;
    public LayerMask ground;

    bool isFrontWall;


    [Header("Attack VFX/Effect")]
    public ParticleSystem muzzleFlashPS;
    public ParticleSystem sparklingPS;
    public CinemachineVirtualCamera cam;

    float defauldFre, defauldAmp;

    [Header("Attack System")]
    public GameObject bulletPrefab;
    public TextMeshProUGUI ammoTxt;
    public CollectableObj bullet;
    public float bulletDamage;
    public int maxPistolAmmo;
    public int maxPocketAmmo;

    Transform muzzlePos;
    int mask;                                               //linecast'teki layer ignore için 
    int ammo;
    bool smoothResetCam;                                    //kamera titredikten sonra smooth bir þekilde normalleþsin

    [Space(10)]
    public Camera fpsCam;                                   //cameradan ileri ray atacaz ve deydiði yere mermi ateþleyecez
    public float range;


    private void Awake()
    {
        ins = this;
        //Application.targetFrameRate = 100;
    }
    void Start()
    {
        mask = (1 << 8);                                      //enemy layer ýný kaydeder    (enemy, transparanFX, dontClose, ignore raycast)
        mask = ~mask;                                         // "~" ifadesi ile tersini alýr (bu olmasa linecast sadece "8" nolu katmaný arar. Bu ifade ("~") varken sadece "8" nolu katmaný yoksayar)

        pistolObj = GetComponent<Transform>();
        muzzlePos = muzzleFlashPS.transform;

        canAtk = true;
        isFrontWall = false;
        smoothResetCam = false;

        ammo = maxPistolAmmo;
        ammoTxt.text = ammo.ToString();


        defauldFre = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain;
        defauldAmp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain;
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadPistol();
        }


        isFrontWall = Physics.Raycast(wallTestRayPos.position, followPivot.forward, rayRange, ground);
        Debug.DrawRay(wallTestRayPos.position, followPivot.forward * rayRange, Color.red, 0.1f);


        if (!isFrontWall)        //pistol duvarýn içinde deðil
        {
            Vector3 pos = Vector3.Lerp(pistolObj.position, followPivot.position, Time.deltaTime * followSpeed);
            Quaternion euler = Quaternion.Lerp(pistolObj.rotation, followPivot.rotation, Time.deltaTime * followSpeed);

            pistolObj.SetPositionAndRotation(pos, euler);
        }
        else
        {
            Vector3 pos = Vector3.Lerp(pistolObj.position, followPivot.position, Time.deltaTime * followSpeed);
            Quaternion euler = Quaternion.Lerp(pistolObj.rotation, followPivot.rotation * Quaternion.Euler(followPivot.rotation.x + 5, followPivot.rotation.y - 80,
                               followPivot.rotation.z), Time.deltaTime * followSpeed * 0.3f);

            pistolObj.SetPositionAndRotation(pos, euler);
        }

        if (smoothResetCam)
        {
            cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = Mathf.Lerp(cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain, defauldFre, 0.02f);
            cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain, defauldAmp, 0.02f);

            if (cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain * 0.95f < defauldFre)
            {
                smoothResetCam = false;
            }
        }
    }

    public void Shoot() //Daha sonradan mobil için ui'a bir buton eklenecek
    {
        if (canAtk && !isFrontWall && ammo > 0 && PlayerHP.ins.isAlive)
        {
            if (Random.Range(0, 2) == 0)
                AudioManager.ins.PlaySound("pistolShoot1");
            else
                AudioManager.ins.PlaySound("pistolShoot2");


            pistolAnim.SetTrigger("pistolShoot");
            ShakeScreenn(0.2f, 2f, 2);

            sparklingPS.Play();
            Invoke(nameof(SmokeDelay), 0.15f);

            canAtk = false;
            Invoke(nameof(ResetAtkSpeed), attackSpeed);

            Invoke(nameof(InstantiateBullet), 0.15f);

            ammo--;
            ammoTxt.text = ammo.ToString();

            if (ammo <= 0)
            {
                Invoke(nameof(ReloadPistol), attackSpeed + 0.05f);
            }
        }
        else if (ammo <= 0)
        {
            ReloadPistol();
        }
    }
    public void ReloadPistol()
    {
        if (canAtk && !isFrontWall && ammo < maxPistolAmmo && ThereIsAmmo() && PlayerHP.ins.isAlive)
        {
            AudioManager.ins.PlaySound("realoadPistol");
            int reloadAmmo = ReloadAmmo();

            PlayerCollect.ins.UptAmmo(ammo - reloadAmmo);
            ammo = reloadAmmo;
            ammoTxt.text = ammo.ToString();

            pistolAnim.SetTrigger("pistolReload");

            canAtk = false;
            Invoke(nameof(ResetAtkSpeed), reloadTime);
        }
    }
    void InstantiateBullet()
    {
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out RaycastHit hit, range, mask))       //bu ray bir þeye çarparsa true döndürür
        {
            if (hit.transform.CompareTag("EnemyBlood"))
            {
                GeneralPool.BloodEffect(hit.point, 1);
                hit.transform.GetComponentInParent<BossHP>().GetDamage(bulletDamage);

                UIController.ins.ShowHitUI();
            }
            else if (hit.transform.CompareTag("EnemySkeleton"))
            {
                GeneralPool.FlashEffect(hit.point, 1);
                hit.transform.GetComponentInParent<EnemyHP>().GetDamage(bulletDamage);

                UIController.ins.ShowHitUI();
            }
            else
            {
                GeneralPool.FlashEffect(hit.point, 1);
            }
        }
    }
    bool ThereIsAmmo()
    {
        return bullet.ammoAmount > 0;
    }
    int ReloadAmmo()
    {
        if (bullet.ammoAmount + ammo >= maxPistolAmmo)
            return maxPistolAmmo;
        else if (ammo == 0)
            return bullet.ammoAmount;
        else
            return bullet.ammoAmount + ammo;
    }
    public void ShakeScreenn(float time, float amplitude, float frequncy)
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequncy;
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;

        //StartCoroutine(ShakeScreen(0f, amplitude, frequncy));                                    //eskiden bu alttaki 2 satýrý kullanýyorduk þimdi üstteki 2 satýrý kullanýyoz, smooth geçiþ saðlýyorlar
        //StartCoroutine(ShakeScreen(time, defauldAmp, defauldFre));

        Invoke(nameof(ResetCamInvoke), time);
    }
    void ResetCamInvoke()
    {
        smoothResetCam = true;
    }



    void ResetAtkSpeed()
    {
        canAtk = true;
    }
    void SmokeDelay()
    {
        muzzleFlashPS.Play();
    }
}
