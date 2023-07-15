using UnityEngine;

public class GameplaySoundChanger : MonoBehaviour
{
    public float soundTransTime;

    void Start()
    {
        AudioManager.ins.PlaySound("forestBG");
    }

    public void ChangeBGTypeForGameplay(bool inShip)
    {
        AudioManager.onOceon = inShip;
        AudioManager.ins.forestOrOceanTimer = soundTransTime;
    }
}
