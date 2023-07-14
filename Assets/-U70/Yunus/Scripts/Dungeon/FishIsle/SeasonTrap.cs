using DG.Tweening;
using UnityEngine;

public class SeasonTrap : MonoBehaviour
{
    public static SeasonTrap ins;

    [HideInInspector] public int pillarTurn;                       //1, 2, 3, 4
    [HideInInspector] public int pillarType;                       //summer, fall, winter, spring
    bool isGameFinished;

    public Transform playerCapsule;
    public Transform resetPos;
    public float resetGameTime;

    [Tooltip("1 = summer, 2 = fall, 3 = winter, 4 = spring")]
    public ParticleSystem[] pillarFireVFX;
    [Tooltip("1 = summer, 2 = fall, 3 = winter, 4 = spring")]
    public Transform[] spikes;                                     //normal yukseklik -0.01 (z ekseni) zeminden çýkmýþ yükseklik 0.0

    public Transform gate;

    private void Awake()
    {
        ins = this;
    }
    private void Start()
    {
        pillarTurn = 1;
        pillarType = 0;

        isGameFinished = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isGameFinished && pillarType !=0)
        {
            CheckPillar(pillarType);
        }
    }

    void CheckPillar(int pillarType)
    {
        if (pillarType == 4 && pillarTurn == pillarType)
        {
            FireUp(pillarTurn);
            OpenGate();
        }

        if (pillarTurn == pillarType)
        {
            FireUp(pillarTurn);
            pillarTurn++;
        }
        else
        {
            SpikeUp(pillarType - 1);
        }        
    }

    public void SetPillerType(int pillerTypeee)
    {
        pillarType = pillerTypeee;
    }
    void FireUp(int pillarType)
    {
        pillarFireVFX[pillarType - 1].Play();
    }
    void OpenGate()
    {
        isGameFinished = true;

        gate.DOLocalMoveY(-5, 2f);
    }
    void SpikeUp(int pillarType)
    {
        spikes[pillarType].DOLocalMoveZ(0, 0.3f);

        PlayerHP.ins.GetDamageTrap(2000, 0.5f, 20, 4);

        playerCapsule.DOMove(new Vector3(resetPos.position.x, playerCapsule.position.y, resetPos.position.z), 0.01f).SetDelay(resetGameTime - 0.5f);
        Invoke(nameof(ResetSeasonTrap), resetGameTime);
    }
    void ResetSeasonTrap()
    {
        pillarTurn = 1;
        pillarType = 0;

        for (int i = 0; i < spikes.Length; i++)
        {
            pillarFireVFX[i].Stop();
            spikes[i].DOLocalMoveZ(-0.01f, 0);
        }
        
        PlayerHP.ins.Resurrect();
    }
}
