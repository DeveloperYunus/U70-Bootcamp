using UnityEngine;

public class CompassArrow : MonoBehaviour
{
    Transform shipTransform;
    public Transform targetTransform;

    public Transform compass;

    void Start()
    {
        shipTransform = GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        FindAngleFromZ();
    }

    void FindAngleFromZ()
    {
        Vector3 direction = targetTransform.position - shipTransform.position;
        direction.y = 0;

        float angle = Vector3.SignedAngle(shipTransform.right, direction, Vector3.down);

        compass.localEulerAngles = new(compass.localEulerAngles.x, compass.localEulerAngles.y, angle);
    }
}
