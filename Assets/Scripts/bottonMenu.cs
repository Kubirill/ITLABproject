using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bottonMenu : MonoBehaviour
{
    public bool contin;
    public float speed=1;
    private int direction=0;

    private void OnMouseDown()
    {
        direction = 1;
    }

    private void Update()
    {
        transform.position = transform.position + new Vector3(0, 0, speed * direction*Time.deltaTime);
        if (transform.position.z > -4.9) direction = -1;
        
        if ((transform.position.z < -5) && (direction == -1) && (contin))
        {
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            SceneManager.LoadScene("Level"+(cam.GetComponent<Menu>().GetLastLevel()), LoadSceneMode.Single);
        }
        if ((transform.position.z < -5) && (direction == -1) && (!contin))
        {
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            SceneManager.LoadScene(gameObject.name, LoadSceneMode.Single);
            cam.GetComponent<Menu>().Save(1);
            cam.GetComponent<Menu>().NewTime(0);
        }
    }
}
