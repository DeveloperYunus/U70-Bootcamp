using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [Header("FireBall Attack")]
    public GameObject fireball;
    public float fireBallDamage;
    public float fireballSpeed;
    public Transform muzzleHand;
    public Transform childTransform;                            //animasyondan dolay�
    public float distanceCheckTime;
    public float atkSpeedFireBall;
    public float fireBallYHeight;                               //Fireball player �n ne kadar yukar�s�na g�nderilecek

    float range;                                                // bu range player pistol daki range ile ayn� yoksa ikisinden birir menzil avantaj� kazanm�� olurdu
    float distCheck;
    float rangeSqr;                                             //range de�i�keninin karesi optimizasyon i�in
    int attackCount;

    bool canAtk, isAlive;

    Transform pcTransform;                                      //PistolController objesinin transformunu tutar
    Animator anim;

    [Header("Skeleton Attack")]
    public GameObject enemy;
    public int calledEnemyCount;
    public float callRange;
    public int whichTimeFireBall;                               // boss ka� sald�r�da bir iskelet ��kartacak
    public float atkSpeedSkeletonCall;

    [Space(10)]
    public ParticleSystem callShield;
    public ParticleSystem callCircle;
    public Collider colliderr;

    BossHP bossHP;


    void Start()
    {
        pcTransform = PlayerHP.ins.transform;
        anim = GetComponentInChildren<Animator>();
        bossHP = GetComponent<BossHP>();

        range = PistolController.ins.range + 1;
        rangeSqr = range * range;
        distCheck = distanceCheckTime;

        canAtk = true;
        isAlive = true;
        attackCount = 0;
    }
    void Update()
    {
        if (!isAlive)
            return;

        if (distCheck > 0)
        {
            if (DistCheckPlayer())
            {
                distCheck = distanceCheckTime;

                if (canAtk)
                {
                    if (attackCount < whichTimeFireBall)
                    {
                        attackCount++;
                        FireBallAttack();
                    }
                    else
                    {
                        attackCount = 0;
                        CallSkeleton();
                    }
                }
            }
        }
        else
            distCheck -= Time.deltaTime;
    }

    bool DistCheckPlayer()
    {
        if (Vector3.SqrMagnitude(pcTransform.position - transform.position) < rangeSqr)     //player silah�n�n range de�erinin i�erisinde 
        {
            Vector3 horizontalTarget = new(pcTransform.position.x, transform.position.y, pcTransform.position.z);   //bir objeye sadece yatayda bakma kody
            transform.LookAt(horizontalTarget);

            return true;
        }
        return false;
    }
    void FireBallAttack()
    {
        AudioManager.ins.PlaySound("fireBallGo");

        canAtk = false;
        Invoke(nameof(ResetAtk), atkSpeedFireBall);

        anim.SetTrigger("fireball");

        Invoke(nameof(InsFireBall), 0.3f);
    }
    void InsFireBall()
    {
        Vector3 direction = pcTransform.position - muzzleHand.position + new Vector3(0, fireBallYHeight, 0);
        GameObject a = Instantiate(fireball, muzzleHand.position, Quaternion.identity);

        a.GetComponent<Rigidbody>().velocity = direction * fireballSpeed;
        a.GetComponent<BossFireBall>().damage = fireBallDamage;
    }
    void CallSkeleton()
    {
        AudioManager.ins.PlaySound("callSkeleton");

        canAtk = false;
        Invoke(nameof(ResetAtk), atkSpeedSkeletonCall);

        CanBossTakeDmg(false);
        anim.SetTrigger("skeleton");

        Invoke(nameof(InsSkeleton), 4f);
    }
    void InsSkeleton()
    {
        childTransform.SetLocalPositionAndRotation(new Vector3(0, 0.05f, 0), Quaternion.Euler(0, 5, 0));

        AudioManager.ins.PlaySound("undeadRise");
        AudioManager.ins.PlaySound("undeadRoar");

        for (int i = 0; i < calledEnemyCount; i++)
        {
            Vector3 spawnPos = new(transform.position.x + Random.Range(0, callRange), transform.position.y - 2, transform.position.z + Random.Range(-callRange, callRange));
            //random.range(x=0,range); yapt�k ��nk� x negativde korkuluk var ve iskeletlerin a�a��dan ��kt�klar� g�z�k�yor 

            Instantiate(callCircle, spawnPos + new Vector3(0, 2.1f, 0), Quaternion.Euler(90, 0, 0));
            GameObject skeleton = Instantiate(enemy, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));

            skeleton.GetComponent<NavMeshAgent>().enabled = false; 
            skeleton.GetComponent<EnemyNavMesh>().enabled = false;
            skeleton.GetComponent<EnemyHP>().enabled = false;

            skeleton.transform.DOMoveY(skeleton.transform.position.y + 2, 1.5f);

            StartCoroutine(StartSkeletonSystem(1.5f, skeleton));
            callShield.Stop();
        }
    }
    IEnumerator StartSkeletonSystem(float waitTime, GameObject skeleton)
    {
        yield return new WaitForSeconds(waitTime);
        CanBossTakeDmg(true);

        skeleton.GetComponent<NavMeshAgent>().enabled = true;
        skeleton.GetComponent<EnemyNavMesh>().enabled = true;
        skeleton.GetComponent<EnemyHP>().enabled = true;

        skeleton.GetComponent<EnemyNavMesh>().FollowPlayer();
    }
    void CanBossTakeDmg(bool truee)
    {
        if (truee)
        {
            callShield.Stop();
            bossHP.canTakeDmg = true;
            colliderr.enabled = true;
        }
        else
        {
            callShield.Play();
            bossHP.canTakeDmg = false;
            colliderr.enabled = false;
        }
    }


    public void Die()
    {
        isAlive = false;
        anim.SetTrigger("dead");
        Destroy(gameObject, 2.5f);
    }

    void ResetAtk()
    {
        canAtk = true;
    }
}
