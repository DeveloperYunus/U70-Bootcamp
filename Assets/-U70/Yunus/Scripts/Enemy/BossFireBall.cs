using UnityEngine;

public class BossFireBall : MonoBehaviour
{
    public Collider coll;
    public ParticleSystem fireBallVFX;
    public ParticleSystem fireBallExpVFX;
    
    public float damage;
    public float dieTime;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.name == "Capsule")
        {
            print(other.tag);

            other.GetComponentInParent<PlayerHP>().GetDamage(damage, 0.3f, 6);

            StopFireball();
        }

        if (other.CompareTag("Untagged") || other.CompareTag("Ground"))
        {
            print(other.tag);

            StopFireball();
        }
    }

    void StopFireball()
    {
        if (Random.Range(0, 2) == 0)
            AudioManager.ins.PlaySound("fireBallExp1");
        else
            AudioManager.ins.PlaySound("fireBallExp2");

        coll.enabled = false;
        GetComponent<Rigidbody>().velocity = -GetComponent<Rigidbody>().velocity;
        fireBallVFX.Stop();
        fireBallExpVFX.Play();

        Invoke(nameof(StopSpeed), 0.02f);
        Destroy(gameObject, dieTime);
    }

    void StopSpeed()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

    }
}
