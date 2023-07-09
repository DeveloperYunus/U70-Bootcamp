using TMPro;
using UnityEngine;

public class CompassArrow : MonoBehaviour
{
    public static CompassArrow ins;

    Transform shipTransform;
    public Transform mainCamera;
    public Transform targetTransform;

   
    [Header("--- For Compass ---")]
    public Transform compass;
    public TextMeshProUGUI distanceTxt;

    public Transform[] targets;


    private void Awake()
    {
        ins = this;
    }
    void Start()
    {
        //mainCamera = Camera.main.transform;
        shipTransform = GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        FindAngleFromZ();
    }

    void FindAngleFromZ()
    {
        if (targetTransform)
        {
            Vector3 direction = targetTransform.position - shipTransform.position;
            direction.y = 0;

            distanceTxt.text = direction.magnitude.ToString("#");

            float angle = Vector3.SignedAngle(Vector3.right, direction, Vector3.down);

            compass.localEulerAngles = new(0, 0, angle + mainCamera.eulerAngles.y - 90);
        }
    }

    /// <summary>
    /// Eðer distance text'inde 0 gözüksün istiyorsak liste elemanýn dýþýnda bir deðer veririz
    /// </summary>
    /// <param name="targetNumber"></param>
    public void SetTarget(int targetNumber)
    {
        if (targets.Length < targetNumber + 1)
        {
            targetTransform = null;
            distanceTxt.text = "No target";
        }
        else
        {
            targetTransform = targets[targetNumber];
        }
    }
}
