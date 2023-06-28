using Cinemachine;
using DG.Tweening;
using UnityEngine;


public class GameplayChange : MonoBehaviour
{
    public static GameplayChange ins;

    [HideInInspector] public bool isModeWalk;                           //true ise player zeminde pistol ile koþuyor demektir, false ise gemi ile geziyor demektir


    [Header("Player Data")]
    public GameObject playerPack;
    public RectTransform playerPanel;

    [Header("Ship Data")]
    public GameObject shipCm;
    public ShipController shipController;
    public RectTransform shipPanel;

    [Space(10)]
    public CinemachineBrain cmBrain;

    [Header("E Image")]
    public CanvasGroup eImage;
    bool isEKeyActive;


    private void Awake()
    {
        ins = this;
    }
    private void Start()
    {
        isModeWalk = true;
        isEKeyActive = false;

        playerPack.SetActive(true);
        shipCm.SetActive(false);
        shipController.canShipMove = false;

        cmBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
    }
    private void Update()
    {
        if (isEKeyActive && Input.GetKeyDown(KeyCode.E))
        {
            ChangePlayMode();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowEImage(true);
        }   
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ship") && shipController.rb.velocity.magnitude < 1f && !isModeWalk)
        {
            ShowEImage(true);
        }
        else if (!isModeWalk)
        {
            ShowEImage(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ship"))
        {            
            ShowEImage(false);
        }
    }
    void ShowEImage(bool showing)
    {
        if (showing)
        {
            isEKeyActive = true;
            eImage.DOKill();
            eImage.DOFade(1, 0.5f);
        }
        else
        {
            isEKeyActive = false;
            eImage.DOKill();
            eImage.DOFade(0, 1f);
        }
    }



    public void ChangePlayMode()
    {
        if (isModeWalk)     //gemiyi aktif et
        {
            isModeWalk = false;
            ShowShip();
        }
        else                //player'ý aktif et
        {
            isModeWalk = true;
            ShowPlayer();
        }
    }
    void ShowPlayer()
    {
        playerPack.SetActive(true);
        playerPanel.GetComponent<CanvasGroup>().DOFade(1, 2f).SetDelay(1);
        playerPanel.DOScale(1, 0);

        shipCm.SetActive(false);
        shipController.canShipMove = false;
        shipController.AnchorTheShip();

        shipPanel.GetComponent<CanvasGroup>().DOFade(0, 1f);
        shipPanel.DOScale(0, 0).SetDelay(1f);

        cmBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
    }
    void ShowShip()
    {
        playerPack.SetActive(false);
        playerPanel.GetComponent<CanvasGroup>().DOFade(0, 1f);
        playerPanel.DOScale(0, 0).SetDelay(1f);

        shipCm.SetActive(true);
        shipController.canShipMove = true;
        shipPanel.GetComponent<CanvasGroup>().DOFade(1, 2f).SetDelay(1);
        shipPanel.DOScale(1, 0);

        cmBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
    }
}
