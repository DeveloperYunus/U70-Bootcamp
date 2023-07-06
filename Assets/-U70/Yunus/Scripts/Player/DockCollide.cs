using UnityEngine;

public class DockCollide : MonoBehaviour
{
    GameplayChange gc;

    public Transform playerStartPos;
    public CanvasGroup eImage;

    private void Start()
    {
        gc = GameplayChange.ins;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowEImageForDock(true);
        }

        if (other.CompareTag("Player") || other.CompareTag("Ship"))
        {
            SendData();
            gc.SetEImageScale(GameplayChange.isModeWalk ? gc.playerScale : gc.shipScale);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ship") && gc.shipController.rb.velocity.magnitude < 1f && !GameplayChange.isModeWalk)
        {
            ShowEImageForDock(true);
        }
        else if (other.CompareTag("Ship") && !GameplayChange.isModeWalk)
        {
            ShowEImageForDock(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ship"))
        {
            ShowEImageForDock(false);
        }
    }





    /// <summary> playerStartPos ve eImage verilerini GameplayChange'e gönderir </summary>
    void SendData()
    {
        GameplayChange.playerStartPos = playerStartPos;
        GameplayChange.eImage = eImage;
    }
    void ShowEImageForDock(bool showing)
    {
        SendData();

        gc.ShowEImage(showing);
    }
}
