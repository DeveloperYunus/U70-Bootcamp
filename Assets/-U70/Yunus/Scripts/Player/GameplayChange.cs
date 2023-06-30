using Cinemachine;
using DG.Tweening;
using StarterAssets;
using UnityEngine;
using UnityEngine.UIElements;


public class GameplayChange : MonoBehaviour
{
    public static GameplayChange ins;

    [HideInInspector] public bool isModeWalk;                           //true ise player zeminde pistol ile koþuyor demektir, false ise gemi ile geziyor demektir


    [Header("--- Player Data ---")]
    public GameObject playerPistol;
    public GameObject playerCm;
    public FirstPersonController playerCapsule;
    public RectTransform playerPanel;

    [Space(10)]
    public Transform playerStartPos;
    public Transform pistolForShipMove;
    public float downPistolDuration;


    [Header("--- Ship Data ---")]
    public GameObject shipCm;
    public ShipController shipController;
    public RectTransform shipPanel;

    [Space(10)]
    public CinemachineBrain cmBrain;

    [Header("--- E Image ---")]
    public CanvasGroup eImage;
    bool isEKeyActive;
    bool eKeyTimer;                                     // e key'i cinemachine'in bir kameradan diðerine geçene kadar tekrar aktif olmaz


    private void Awake()
    {
        ins = this;
    }
    private void Start()
    {
        isModeWalk = true;
        isEKeyActive = false;
        eKeyTimer = true;

        shipCm.SetActive(false);
        shipController.canShipMove = false;

        cmBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
    }
    private void Update()
    {
        if (isEKeyActive && Input.GetKeyDown(KeyCode.E) && eKeyTimer)
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

        eKeyTimer = false;
        Invoke(nameof(TurnTrueEKey), cmBrain.m_DefaultBlend.m_Time);
    }
    void TurnTrueEKey()
    {
        eKeyTimer = true;
    }

    void ShowShip()
    {
        pistolForShipMove.DOLocalRotate(new Vector3(54, 0, 0), downPistolDuration);
        playerCapsule.StopSpeed();

        Invoke(nameof(ShowShipInvoke), 1);
    }
    void ShowShipInvoke()
    {
        SetEKeyScale(7);

        playerCm.SetActive(false);
        playerPistol.SetActive(false);

        shipCm.SetActive(true);
        shipController.canShipMove = true;


        playerPanel.GetComponent<CanvasGroup>().DOFade(0, 1f);
        playerPanel.DOScale(0, 0).SetDelay(1f);

        shipPanel.GetComponent<CanvasGroup>().DOFade(1, 2f).SetDelay(1);
        shipPanel.DOScale(1, 0);

        cmBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
    }

    void ShowPlayer()
    {
        SetEKeyScale(1);
        SetPlayerLocation();

        playerCm.SetActive(true);
        playerCapsule.StopSpeed();

        shipCm.SetActive(false);
        shipController.canShipMove = false;
        shipController.AnchorTheShip();


        playerPanel.GetComponent<CanvasGroup>().DOFade(1, 2f).SetDelay(1);
        playerPanel.DOScale(1, 0);

        shipPanel.GetComponent<CanvasGroup>().DOFade(0, 1f);
        shipPanel.DOScale(0, 0).SetDelay(1f);

        cmBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;

        Invoke(nameof(ShowPlayerInvoke), cmBrain.m_DefaultBlend.m_Time);
    }
    void ShowPlayerInvoke()
    {
        playerPistol.SetActive(true);
        pistolForShipMove.DOLocalRotate(new Vector3(0, 0, 0), downPistolDuration);
        Invoke(nameof(LetPlayerMove), 1);
    }
    void LetPlayerMove()
    {
        playerCapsule.isAlive = true;
    }


    void SetPlayerLocation()
    {
        Vector3 posXZ = new(playerStartPos.position.x, playerCapsule.transform.position.y, playerStartPos.position.z);

        playerCapsule.transform.DOMove(posXZ, cmBrain.m_DefaultBlend.m_Time);
    }
    void SetEKeyScale(float scaleVal)
    {       
        eImage.GetComponent<RectTransform>().DOScale(scaleVal, cmBrain.m_DefaultBlend.m_Time);
    }
}
