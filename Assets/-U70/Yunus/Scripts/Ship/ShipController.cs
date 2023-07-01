using DG.Tweening;
using TMPro;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public static ShipController ins;
    public bool canShipMove;
    int shipSpeedLvl;       //ship level -1, 0, 1, 2

    public Rigidbody rb;

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

        SetShipSail();
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

                SetSailScale(sailFront, false);
                SetSailScale(sailMiddle, true);
                SetSailScale(sailBack, false);
                break;


            case 2:
                _moveSpeed = moveSpeed * 2;

                SetSailScale(sailFront, true);
                SetSailScale(sailBack, true);
                break;


            case 0:
                _moveSpeed = 0;

                SetSailScale(sailFront, false);
                SetSailScale(sailMiddle, false);
                SetSailScale(sailBack, false);
                break;

            case -1:
                _moveSpeed = moveSpeed * (-0.5f);

                SetSailScale(sailFront, false);
                SetSailScale(sailMiddle, false);
                SetSailScale(sailBack, false);
                break;
        }
    }

    void SetSailScale(Transform sail, bool up)      //up true ise yelkenler a��l�r
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
}