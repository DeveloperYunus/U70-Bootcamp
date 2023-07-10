using UnityEngine;

public class EnemyProducer : MonoBehaviour
{
    public GameObject enemy;

    [Header("--- Skeleton Stats ---")]
    public float health;
    public float armour;
    public float damage;

    [Header("--- Skeleton Spawn ---")]
    public int skeletonAmount;
    public float range;

    void Start()
    {
        ProduceEnemy(skeletonAmount);
    }
    void ProduceEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject a = Instantiate(enemy, transform.position, Quaternion.identity, transform);
            a.transform.position = new Vector3(transform.position.x + Random.Range(-range, range), transform.position.y, transform.position.z + Random.Range(-range, range));

            a.GetComponent<EnemyHP>().SetSkeletonStats(health,armour,damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<EnemyNavMesh>().FollowPlayer();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(range, 0, range));
    }
}
