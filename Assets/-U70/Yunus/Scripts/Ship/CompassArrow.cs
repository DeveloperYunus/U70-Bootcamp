using UnityEngine;

public class CompassArrow : MonoBehaviour
{
    public Transform mainCamera;
    Transform shipTransform;
    public Transform targetTransform;

    public Transform compass;

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

            float angle = Vector3.SignedAngle(Vector3.right, direction, Vector3.down);

            compass.localEulerAngles = new(0, 0, angle + mainCamera.eulerAngles.y - 90);
        }
    }
}
