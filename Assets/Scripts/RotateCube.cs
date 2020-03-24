using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    private float startX;
    private float startY;
    
    public float speedRotate = 1f;
    public Transform gameCube;
    public Transform camera;
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
        startX =gameCube.rotation.x-Input.mousePosition.y;
        startY = gameCube.rotation.y - Input.mousePosition.x;
    }
    public void OnMouseUp()
    {
        startX = 0;
        startY = 0;
    }
    public void OnMouseDrag()
    {
        if (Input.GetAxis("Jump")>0)
        {
            gameCube.SetPositionAndRotation(gameCube.position, Quaternion.Euler(startX - Input.mousePosition.y, startY - Input.mousePosition.x, gameCube.rotation.z));
            //gameCube.RotateAround();
        }
    }
    
}
