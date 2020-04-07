using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    private Vector2 startMouse;
    public float maxDistance;
    public float minDistance;
    public float speedWheel;
    private float nowDistance;
    private Vector3 axisOne;
    private Vector3 axisTwo;

    public float speedRotate = 1f;
    public Transform targetLook;
    public Transform cameraPos;
    public Transform cameraVector;
    public float startX = 0;
    public float startY = 0;
    public bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        nowDistance = (maxDistance + minDistance ) / 2;
        targetLook.position = new Vector3(targetLook.localPosition.x, targetLook.localPosition.y, nowDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            nowDistance = nowDistance + Input.GetAxis("Mouse ScrollWheel") * speedWheel;
            nowDistance = Mathf.Min(Mathf.Max(minDistance, nowDistance), maxDistance);
            targetLook.localPosition = new Vector3(targetLook.localPosition.x, targetLook.localPosition.y, nowDistance);

        }
        else
        {
            if ((targetLook.localPosition != new Vector3(startX, startY, nowDistance)))
            {
                targetLook.localPosition = Vector3.Lerp(targetLook.localPosition, new Vector3(startX, startY, nowDistance), 0.05f);
                targetLook.localScale = Vector3.Lerp(targetLook.localScale, new Vector3(1, 1, 1), 0.05f);
            }
            if (Vector3.Distance(targetLook.localPosition, new Vector3(startX, startY, nowDistance))<1)
            {
                targetLook.localPosition = new Vector3(startX, startY, nowDistance);
                active = false;

            }
        }
    }



    public void OnMouseDown()
    {
        startMouse = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
        axisOne = new Vector3(cameraVector.position.x - cameraPos.position.x, cameraVector.position.y - cameraPos.position.y, cameraVector.position.z - cameraPos.position.z);
        axisTwo = axisOne;
        axisOne = new Vector3(axisOne.z, axisOne.y, -axisOne.x);
        axisTwo = new Vector3( axisOne.y, -axisOne.x, axisOne.z);
    }
    public void OnMouseUp()
    {
        startMouse = new Vector2 (0f,0f);
    }
    public void OnMouseDrag()
    {
        Vector2 nowMouse;
        
        
            nowMouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (startMouse != nowMouse)
            {
                // targetLook.SetPositionAndRotation(targetLook.position, Quaternion.Euler(startX - Input.mousePosition.y, startY - Input.mousePosition.x, targetLook.rotation.z));
                //targetLook.RotateAround();
                targetLook.RotateAround(targetLook.transform.position, -axisTwo, (startMouse.x - nowMouse.x) * speedRotate);
                targetLook.RotateAround(targetLook.transform.position, -axisOne, (startMouse.y - nowMouse.y) * speedRotate);
                startMouse = nowMouse;
            }
            
        
    }
     
    public void newTarget(Transform target)
    {
        minDistance = 2;
        targetLook = target;
       // transform.LookAt(targetLook);
        targetLook.parent=transform;
        nowDistance = (maxDistance + minDistance) / 2;
        active = true;
    }
}
