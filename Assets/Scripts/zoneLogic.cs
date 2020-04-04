using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct elementPosition
{
    public GameObject gObj;
    public bool flipped;
    public float angle;
    public static bool operator ==(elementPosition elsOne, elementPosition elsAns)
    {
        if ((elsOne.gObj.name== elsAns.gObj.name)&& (elsOne.flipped == elsAns.flipped))
            {
            int partAngle;
            if (elsOne.gObj.name == "figure1") partAngle = 60;
            else partAngle = 90;
            float angleOne = (elsOne.angle + 360) % partAngle;
            float angleAns = (elsAns.angle + 360) % partAngle;
            if (Mathf.Abs(angleOne - angleAns) < 10) return true;
            else return false;
            }
        else return false;
    }
    public static bool operator !=(elementPosition elPos, elementPosition elsAns)
    {
        return !(elPos == elsAns);
    }
}
public class zoneLogic : MonoBehaviour
{
    public GameObject[] elements;
    public GameObject cam;
    //public GameObject[] elementsOnScene = new GameObject[3];
    public float speedRotate=1;
    elementPosition[] elementsOnScene = new elementPosition[3];
    elementPosition[] answer = new elementPosition[3];
    private int index;

    private void Start()
    {
        
    }

    private void OnMouseDown()
    {
        if (cam.GetComponent<Inventory>().isDraggable)
        {
            string nameEl = cam.GetComponent<Inventory>().UseElement() ;
            foreach (GameObject element in elements)
            {
                if (element.name == nameEl)
                {
                    elementsOnScene[index].gObj = Instantiate(element, transform.position- new Vector3(0,0,index*0.1f+0.2f), Quaternion.identity);
                    elementsOnScene[index].gObj.GetComponent<traceryLogic>().cam = cam;
                    elementsOnScene[index].gObj.GetComponent<BoxCollider>().enabled = false;
                    elementsOnScene[index].flipped = false;
                    elementsOnScene[index].angle = elementsOnScene[index].gObj.transform.rotation.z;
                    index++;
                }
                
            }
           
            
        }
        else
        {
            if (index > 0)
            {
                index =index-1;
                cam.GetComponent<Inventory>().AddElement(elementsOnScene[index].gObj.GetComponent<traceryLogic>().id);
                Destroy(elementsOnScene[index].gObj);
                elementsOnScene[index].gObj = null;
                
            }
        }
    }
    private void Update()
    {
        if (index > 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                elementsOnScene[index - 1].flipped = !elementsOnScene[index - 1].flipped;
                if (elementsOnScene[index - 1].flipped) elementsOnScene[index - 1].gObj.transform.position = elementsOnScene[index - 1].gObj.transform.position + new Vector3(0, 0, 0.1f);
                else elementsOnScene[index - 1].gObj.transform.position = elementsOnScene[index - 1].gObj.transform.position + new Vector3(0, 0, -0.1f);
                elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 1, 0), 180);

            }
            if (Input.GetKey(KeyCode.Q))
            {
                if (!elementsOnScene[index - 1].flipped) elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 0, 1), speedRotate);
                else elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 0, 1), -speedRotate);
                elementsOnScene[index-1].angle = elementsOnScene[index].gObj.transform.rotation.z;
            }
            if (Input.GetKey(KeyCode.E))
            {

                if (!elementsOnScene[index - 1].flipped)  elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 0, 1), -speedRotate);
                else elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 0, 1), speedRotate);
                elementsOnScene[index-1].angle = elementsOnScene[index-1].gObj.transform.rotation.z;
            }
        }  
        if (index == 3)
        {

        }
    }
}
