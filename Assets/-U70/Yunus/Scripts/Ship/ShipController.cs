using DG.Tweening;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public static ShipController ins;
    public bool canShipMove;
    int shipSpeedLvl;       //ship level -1, 0, 1, 2

    [HideInInspector] public Rigidbody rb;

    [Header("Ship Controller")]
    public float moveSpeed;
    public float speedChangeTime;
    public float rotateSpeed;

    float _moveSpeed;

    [Header("Ship Sail")]
    public Transform sailFront;
    public Transform sailMiddle;
    public Transform sailBack;

    public float sailUpTime;
    public float sailDownTime;

    [Header("Ship VFX")]
    public ParticleSystem shipFoamBody;


    private void Awake()
    {
        ins = this;
    }
    void Start()
    {
        canShipMove = false;
        rb = GetComponent<Rigidbody>();

        shipSpeedLvl = 0;

        SetSailScale(sailBack, false);
        SetSailScale(sailMiddle, false);
        SetSailScale(sailFront, false);
    }
    void Update()
    {
        if (canShipMove)
        {
            ControlShip();
        }

        SetSpeed();
    }


    void ControlShip()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetShipSpeedLevel(1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SetShipSpeedLevel(-1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
        }    
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(rotateSpeed * Time.deltaTime * -Vector3.up);
        }
    }
    void SetShipSpeedLevel(int value)
    {
        shipSpeedLvl += value;

        if (shipSpeedLvl > 2) 
        {
            shipSpeedLvl = 2;
        }
        else if (shipSpeedLvl < -1)
        {
            shipSpeedLvl = -1;
        }
        else
        {
            SetShipSail();
        }
    }
    void SetShipSail()
    {
        sailFront.DOKill();
        sailMiddle.DOKill();
        sailBack.DOKill();

        switch (shipSpeedLvl) 
        {
            case 1:
                _moveSpeed = moveSpeed;
                AudioManager.ins.PlaySound("sail");

                SetSailScale(sailFront, false);
                SetSailScale(sailMiddle, true);
                SetSailScale(sailBack, false);

                SetShipFoam(true);
                break;


            case 2:
                _moveSpeed = moveSpeed * 2;
                AudioManager.ins.PlaySound("sail");

                SetSailScale(sailFront, true);
                SetSailScale(sailMiddle, true);
                SetSailScale(sailBack, true);

                SetShipFoam(true);
                break;


            case 0:
                _moveSpeed = 0;
                AudioManager.ins.PlaySound("sail");

                SetSailScale(sailFront, false);
                SetSailScale(sailMiddle, false);
                SetSailScale(sailBack, false);

                SetShipFoam(false);
                break;

            case -1:
                _moveSpeed = moveSpeed * (-0.5f);

                SetSailScale(sailFront, false);
                SetSailScale(sailMiddle, false);
                SetSailScale(sailBack, false);

                SetShipFoam(false);
                break;
        }
    }

    void SetSailScale(Transform sail, bool up)      //up true ise yelkenler açýlýr
    {
        if (up)
        {
            sail.DOScaleY(1f, sailDownTime);
            sail.DOScaleZ(1f, sailDownTime);
        }
        else
        {
            sail.DOScaleY(0.1f, sailUpTime);
            sail.DOScaleZ(0.3f, sailUpTime);
        }
    }
    void SetSpeed()
    {
        rb.velocity = Vector3.Lerp(rb.velocity, transform.right * _moveSpeed, speedChangeTime);
    }

    public void AnchorTheShip()
    {
        rb.velocity = Vector3.zero;
    }

    void SetShipFoam(bool active)
    {
        if (active)
            shipFoamBody.Play();
        else
            shipFoamBody.Stop();
    }
}
