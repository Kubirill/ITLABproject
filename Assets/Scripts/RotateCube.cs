using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    private Vector2 startMouse;
    
    private Vector3 axisOne;
    private Vector3 axisTwo;

    public float speedRotate = 1f;
    public Transform gameCube;
    public Transform cameraPos;
    public Transform cameraVector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown()
    {
        startMouse = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
        axisOne = new Vector3(cameraVector.position.x - cameraPos.position.x, cameraVector.position.y - cameraPos.position.y, cameraVector.position.z - cameraPos.position.z);
        axisTwo = axisOne;
        axisOne = new Vector3(axisOne.z, axisOne.y, -axisOne.x);
        axisTwo = new Vector3( axisTwo.y, -axisTwo.x, axisTwo.z);
    }
    public void OnMouseUp()
    {
        startMouse = new Vector2 (0f,0f);
    }
    public void OnMouseDrag()
    {
        Vector2 nowMouse;
        if (Input.GetAxis("Jump")>0)
        {
            nowMouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (startMouse != nowMouse)
            {
                // gameCube.SetPositionAndRotation(gameCube.position, Quaternion.Euler(startX - Input.mousePosition.y, startY - Input.mousePosition.x, gameCube.rotation.z));
                //gameCube.RotateAround();
                gameCube.RotateAround(gameCube.transform.position, -axisTwo, (startMouse.x - nowMouse.x) * speedRotate);
                gameCube.RotateAround(gameCube.transform.position, -axisOne, (startMouse.y - nowMouse.y)*speedRotate);
                startMouse = nowMouse;
            }
            
        }
    }
    
}
