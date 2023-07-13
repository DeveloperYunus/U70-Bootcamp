using DG.Tweening;
using UnityEngine;

public class CheckPointSystem : MonoBehaviour
{
    public static CheckPointSystem ins;

    public Transform checkPoint;
    Vector3 startPos;
    float startRot;

    [Header("--- Values ---")]
    public float moveTime;


    private void Awake()
    {
        ins = this;
    }
    private void Start()
    {
        startRot = transform.rotation.y;
        startPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckP"))     //checkPoint
        {
            checkPoint = other.transform;
        }
    }

    public void PlayerGoCheckPoint(float waitTime)
    {
        Invoke(nameof(InvokePlayerGoCheckPos), waitTime);
    }
    void InvokePlayerGoCheckPos()
    {
        if (checkPoint == null)
        {
            transform.DOMove(startPos, moveTime);
            transform.DORotate(new Vector3(0, startRot, 0), moveTime);
        }
        else
        {
            transform.DOMove(checkPoint.position, moveTime);
            transform.rotation = checkPoint.rotation;
        }
    }
}
