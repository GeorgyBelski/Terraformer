using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    public Vector2 panoramaLimit = new Vector2(20,20);
    [Range(0, 20)]
    public float panoramaSpeed = 10f;

    public float borderThickness = 10;
    
    Vector3 focusPointPosition;
    public Vector2 mousPosition , mousPreviousPosition;
    public bool isMoving, isRotaring;
    public Vector3 direction;

    [Header("Reference")]
    public Transform cameraFocusPoint;
 //   float distanceToFocusPoint;

    public float panoramaSpeedLerpCoefficient;

    void Start()
    {
        panoramaSpeedLerpCoefficient = 0.1f;
        focusPointPosition = cameraFocusPoint.position;
 //       distanceToFocusPoint = Vector3.Distance(transform.position, focusPointPosition);
    }

    void Update()
    {
        MoveByTouchingBorders();
        RotateByDrag();

    }
    void RotateByDrag() {

        if (Input.GetMouseButtonDown(0))
        {
            mousPreviousPosition = Input.mousePosition;
            isRotaring = true;
        }
        else if(Input.GetMouseButton(0))
        {
                float screenXDistance = Input.mousePosition.x - mousPreviousPosition.x;
                float rotateY = screenXDistance / (Screen.width - borderThickness*2) * 360;
                cameraFocusPoint.Rotate(0, rotateY, 0);
                mousPreviousPosition = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0)){
            isRotaring = false;
        }

    }
    void MoveByTouchingBorders()
    {

        mousPosition = Input.mousePosition;

        if (Input.GetKey(KeyCode.LeftAlt)||(mousPosition.y >= Screen.height - borderThickness && mousPosition.y < Screen.height + borderThickness*3)
            || (mousPosition.y < borderThickness && mousPosition.y > -borderThickness*3)
            || (mousPosition.x >= Screen.width - borderThickness && mousPosition.x < Screen.width + borderThickness*3)
            || (mousPosition.x < borderThickness && mousPosition.x > -borderThickness*3))
        {
            isMoving = true;
            direction.x = mousPosition.x - Screen.width / 2;
            direction.z = mousPosition.y - Screen.height / 2;
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                direction.x *= (2f/Screen.width);
                direction.z *= (2f/Screen.height);
            }
            else {
                direction.Normalize();
            }
            
            //   cameraFocusPoint.position += (cameraFocusPoint.localRotation * Vector3.forward) * panoramaSpeed * Time.deltaTime;

            // Vector3.Lerp();

            panoramaSpeedLerpCoefficient += Time.deltaTime * 2;
            panoramaSpeedLerpCoefficient = Mathf.Min(1, panoramaSpeedLerpCoefficient);
        }
        else
        {
            //   direction = Vector3.zero;
            //    panoramaSpeedLerpCoefficient = 0.1f;
            if (isMoving)
            {
                if (panoramaSpeedLerpCoefficient > 0.1)
                {
                    panoramaSpeedLerpCoefficient -= Time.deltaTime * 3;
                }
                else
                {
                    isMoving = false;
                    panoramaSpeedLerpCoefficient = 0.1f;
                }
            }

        }

        if (isMoving)
        {
            focusPointPosition += (cameraFocusPoint.localRotation * Vector3.Lerp(Vector3.zero, direction, panoramaSpeedLerpCoefficient)) * panoramaSpeed * Time.deltaTime;
            focusPointPosition.x = Mathf.Clamp(focusPointPosition.x, -panoramaLimit.x, panoramaLimit.x);
            focusPointPosition.z = Mathf.Clamp(focusPointPosition.z, -panoramaLimit.y, panoramaLimit.y);

            cameraFocusPoint.position = focusPointPosition;
        }
    }
}
