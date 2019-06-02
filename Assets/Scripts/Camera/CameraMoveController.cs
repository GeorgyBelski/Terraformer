using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{

    public float panoramaSpeed = 10f;
   // public float panoramaSpeed = 10f;
    public float borderThickness = 10;
    public Transform cameraFocusPoint;
    public Vector3 mousPosition;

    public bool isMoving;
    Vector3 direction;

    public float panoramaSpeedLerpCoefficient;

    void Start()
    {
        panoramaSpeedLerpCoefficient = 0.1f;
    }

    void Update()
    {
        MoveByTouchingBorders();
    }

    void MoveByTouchingBorders()
    {

        mousPosition = Input.mousePosition;

        if (mousPosition.y >= Screen.height - borderThickness
            || mousPosition.y < borderThickness
            || mousPosition.x >= Screen.width - borderThickness
            || mousPosition.x < borderThickness)
        {
            isMoving = true;
            direction.x = mousPosition.x - Screen.width / 2;
            direction.z = mousPosition.y - Screen.height / 2;
            direction.Normalize();
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
            cameraFocusPoint.position += (cameraFocusPoint.localRotation * Vector3.Lerp(Vector3.zero, direction, panoramaSpeedLerpCoefficient)) * panoramaSpeed * Time.deltaTime;
        }
    }
}
