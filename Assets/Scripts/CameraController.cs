using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float distance = 5;

    [SerializeField] float minVerticalAngle = -45;
    [SerializeField] float maxVerticalAngle = 45;

    [SerializeField] Vector2 framingOffset;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    float rotationX;
    float rotationY;

    float invertXVal;
    float invertYVal;

    float focusDistance = 1.5f;
    float baseDistance = 0;

    private void Start()
    {
        baseDistance = distance;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;

        rotationX += Input.GetAxis("Mouse Y") * invertYVal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        rotationY += Input.GetAxis("Mouse X") * invertXVal * rotationSpeed;

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        var focusPotion = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

        transform.position = focusPotion - targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);

    public void focusCamera(bool action)
    {
        if (action)
        {
            if(distance > focusDistance)
            {
                distance -= 0.1f;
            }
        }
        else
        {
            if (distance < baseDistance)
            {
                distance += 0.1f;
            }
        }
    }

}
