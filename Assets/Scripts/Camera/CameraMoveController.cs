using System;
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
    public bool isMovingByBorderTouch, isMovingByDrag, isRotaring;
    public Vector3 direction;
    public float FarthestZoom;
    public float NearestZoom;
    public float scrollFactor = 2f;
    [Header("Reference")]
    public Transform cameraFocusPoint;
 //   float distanceToFocusPoint;

    public float panoramaSpeedLerpCoefficient;

    Ray rayToGround;
    Vector3 mousPreviousGroundPosition, mousCurrentGroundPosition;
    int groundLayerMask = (1<<9);
    RaycastHit hitGround;

    void Start()
    {
        FarthestZoom = transform.localPosition.magnitude + 8f;
        NearestZoom = 10f;
        panoramaSpeedLerpCoefficient = 0.1f;
        focusPointPosition = cameraFocusPoint.position;
 //       distanceToFocusPoint = Vector3.Distance(transform.position, focusPointPosition);
    }

    void Update()
    {
        MoveByTouchingBorders();
        RotateByDrag();
        MoveByDrag();
        Zoom();
    }

    private void FixedUpdate()
    {
        CulculateGroundPositionsForMove();
    }

    void Zoom()
    {
        var scroll = Input.mouseScrollDelta.y;
        if (scroll > 0 && transform.localPosition.magnitude > NearestZoom)
        {
            this.transform.position += this.transform.rotation * Vector3.forward * scrollFactor;
        }
        else if (scroll < 0 && transform.localPosition.magnitude < FarthestZoom)
        {
            this.transform.position -= this.transform.rotation * Vector3.forward * scrollFactor;
        }
    }

    void MoveByDrag() {
        if (isMovingByDrag)
        {
            focusPointPosition = cameraFocusPoint.position;
            focusPointPosition += mousPreviousGroundPosition - mousCurrentGroundPosition;
            ClampFocusPointPosition(focusPointPosition);
            cameraFocusPoint.position = focusPointPosition;
        }     
    }

    private void CulculateGroundPositionsForMove()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Vector3? hit = GetHitGroundPoint();
            if (hit != null)
            {
                mousPreviousGroundPosition = (Vector3)hit;
                mousCurrentGroundPosition = mousPreviousGroundPosition;
                isMovingByDrag = true;
            }
            else { return; }
        }
        else if (Input.GetMouseButton(2))
        {
            Vector3? hit = GetHitGroundPoint();
            if (hit != null)
            {
                // cameraFocusPoint.position +=  mousPreviousGroundPosition - (Vector3)hit;
                // mousPreviousGroundPosition = (Vector3)hit;
                mousCurrentGroundPosition = (Vector3)hit;
                if (!isMovingByDrag) {
                    mousPreviousGroundPosition = mousCurrentGroundPosition;
                    isMovingByDrag = true;
                }
            }
            else { return; }
        }
        else 
        {
            mousPreviousGroundPosition = mousCurrentGroundPosition;
            isMovingByDrag = false;
        }
    }
    Vector3? GetHitGroundPoint()
    {
        rayToGround = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayToGround, out hitGround, 100f, groundLayerMask))
        {
            return hitGround.point;
        }
        return null;
    }

    void RotateByDrag() {

        if (Input.GetMouseButtonDown(1))
        {
            mousPreviousPosition = Input.mousePosition;
            isRotaring = true;
        }
        else if(Input.GetMouseButton(1))
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
            isMovingByBorderTouch = true;
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
            if (isMovingByBorderTouch)
            {
                if (panoramaSpeedLerpCoefficient > 0.1)
                {
                    panoramaSpeedLerpCoefficient -= Time.deltaTime * 3;
                }
                else
                {
                    isMovingByBorderTouch = false;
                    panoramaSpeedLerpCoefficient = 0.1f;
                }
            }

        }

        if (isMovingByBorderTouch)
        {
            focusPointPosition += (cameraFocusPoint.localRotation * Vector3.Lerp(Vector3.zero, direction, panoramaSpeedLerpCoefficient)) * panoramaSpeed * Time.deltaTime;
            ClampFocusPointPosition(focusPointPosition);

            cameraFocusPoint.position = focusPointPosition;
        }

        
    }

    void ClampFocusPointPosition(Vector3 position)
        {
            focusPointPosition.x = Mathf.Clamp(position.x, -panoramaLimit.x, panoramaLimit.x);
            focusPointPosition.z = Mathf.Clamp(position.z, -panoramaLimit.y, panoramaLimit.y);
        }
}
