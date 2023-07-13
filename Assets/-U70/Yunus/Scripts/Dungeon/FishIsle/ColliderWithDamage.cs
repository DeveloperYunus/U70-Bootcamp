using DG.Tweening;
using UnityEngine;

public class ColliderWithDamage : MonoBehaviour
{
    float damage;

    [Space(10)]
    public Transform playerCapsule;
    public Transform resetPos;
    public float resetGameTime;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHP>().GetDamageTrap(2000, 0.5f, 10, 5);

            StopBlade();
            ResetBladeTrap();
        }
    }

    void StopBlade()
    {
        GetComponent<Animator>().enabled = false;  
    }
    void ResetBladeTrap()
    {
        playerCapsule.DOMove(new Vector3(resetPos.position.x, playerCapsule.position.y, resetPos.position.z), 0.01f).SetDelay(resetGameTime - 0.5f);
        Invoke(nameof(ResurrectPlayer), resetGameTime);
    }

    void ResurrectPlayer()
    {
        GetComponent<Animator>().enabled = true;
        PlayerHP.ins.Resurrect();
    }
}
