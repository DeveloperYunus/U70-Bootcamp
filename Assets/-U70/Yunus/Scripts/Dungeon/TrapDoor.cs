using DG.Tweening;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    public Transform trapDoor;
    bool firstTime;


    private void Start()
    {
        firstTime = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && firstTime)
        {
            firstTime = false;

            AudioManager.ins.PlaySound("laugh");
            AudioManager.ins.PlaySound("doorOpen");

            trapDoor.DORotate(new Vector3(0, 13, 0), 0.5f);
        }
    }
}
