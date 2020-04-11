using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct elementPosition
{
    public GameObject gObj;
    public bool flipped;
    public float angle;
    public int id;
    public static bool operator ==(elementPosition elsOne, elementPosition elsAns)
    {
        //Debug.Log(elsOne.angle + " " + elsAns.angle + " " + elsOne.flipped + " " + elsAns.flipped + " " + elsOne.id + " " + elsAns.id);
        if ((elsOne.id== elsAns.id)&& (elsOne.flipped == elsAns.flipped))
            {
            int partAngle;
            if (elsOne.gObj.GetComponent<traceryLogic>() != null) partAngle = elsOne.gObj.GetComponent<traceryLogic>().angleSimetry;
            else partAngle = elsAns.gObj.GetComponent<traceryLogic>().angleSimetry;
            float angleOne = (elsOne.angle + 360) % partAngle;
            float angleAns = (elsAns.angle + 360) % partAngle;
            if ((Mathf.Abs(angleOne - angleAns) < 10)||(Mathf.Abs(angleOne - angleAns+partAngle) < 10) || (Mathf.Abs(angleOne - angleAns - partAngle) < 10)) return true;
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
    public GameObject[] answersElements;
    public GameObject cam;
    public Transform answerBlock;
    public GameObject doorDown;
    public GameObject doorUp;
    private bool finish;
    //public GameObject[] elementsOnScene = new GameObject[3];
    public float speedRotate=1;
    elementPosition[] elementsOnScene = new elementPosition[3];
    elementPosition[] answer = new elementPosition[3];
    private int index;
    public int level=1;

    private void Start()
    {
        setAnswerTraceryPazzle();
        CreateVisualAnswer();
    }

    private void setAnswerTraceryPazzle()
    {
        answer[0].gObj = answersElements[0];
        answer[0].angle = Random.Range(0, level + 1) * 60 / (level + 1);
        answer[1].gObj = answersElements[1];
        answer[1].angle = Random.Range(0, level + 1) * 90 / (level + 1);
        answer[2].gObj = answersElements[2];
        answer[2].angle = Random.Range(0, level + 1) * 90 / (level + 1);
        bool fliping;
        if (Random.Range(0, 2) >= 1) fliping = true;
        else fliping = false;
        for (int i = 0; i < 3; i++)
        {
            answer[i].flipped = fliping;
            answer[i].id = i;
        }
        for (int first = 0; first < 3; first++)
        {

            for (int second = 0; second < 3; second++)
            {
                if (Random.Range(0, 2) >= 1)
                {
                    elementPosition trush = answer[first];
                    answer[first] = answer[second];
                    answer[second] = trush;
                }
            }
        }
    }

    private void CreateVisualAnswer()
    {
        for (int i = 0; i < 3; i++)
        {
            int angle;
            if (answer[i].flipped) angle = 1;
            else angle = 0;
            Instantiate(answer[i].gObj, answerBlock.position + new Vector3(0, 0, -0.06f - 0.01f * i+0.01f*angle), Quaternion.Euler(0, angle*180, answer[i].angle), answerBlock);
        }
    }

    private void OnMouseDown()
    {
        if (!finish)
        {

            if (cam.GetComponent<Inventory>().isDraggable)
            {
                string nameEl = cam.GetComponent<Inventory>().UseElement();
                foreach (GameObject element in elements)
                {
                    if (element.name == nameEl)
                    {
                        elementsOnScene[index].gObj = Instantiate(element, transform.position - new Vector3(0, 0, index * 0.1f + 0.2f), Quaternion.identity);
                        elementsOnScene[index].gObj.GetComponent<traceryLogic>().cam = cam;
                        elementsOnScene[index].gObj.GetComponent<BoxCollider>().enabled = false;
                        elementsOnScene[index].flipped = false;
                        elementsOnScene[index].id = elementsOnScene[index].gObj.GetComponent<traceryLogic>().id;
                        elementsOnScene[index].angle = elementsOnScene[index].gObj.transform.rotation.z;
                        index++;
                    }

                }


            }
            else
            {
                if (index > 0)
                {
                    index = index - 1;
                    cam.GetComponent<Inventory>().AddElement(elementsOnScene[index].gObj.GetComponent<traceryLogic>().id);
                    Destroy(elementsOnScene[index].gObj);
                    elementsOnScene[index].gObj = null;

                }
            }
        }
    }

    private void Update()
    {
        if (!finish)
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
                    elementsOnScene[index - 1].angle = elementsOnScene[index - 1].gObj.transform.localEulerAngles.z;

                }
                if (Input.GetKey(KeyCode.E))
                {

                    if (!elementsOnScene[index - 1].flipped) elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 0, 1), -speedRotate);
                    else elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 0, 1), speedRotate);
                    elementsOnScene[index - 1].angle = elementsOnScene[index - 1].gObj.transform.localEulerAngles.z;
                }
            }
            if (index == 3)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    int correctCount = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (elementsOnScene[i] == answer[i]) correctCount++;
                    }
                    if (correctCount >= 3)
                    {
                        finish = true;
                        cam.GetComponent<RotateCube>().enabled = true;
                        cam.GetComponent<Inventory>().enabled = false;
                        GameObject cube = GameObject.FindGameObjectWithTag("Cube");
                        cube.GetComponent<CubeLogic>().CreateCube();
                        for (int i = 0; i < 3; i++) elementsOnScene[i].gObj.transform.parent = transform;
                    }
                }
            }
        }
        else
        {
            doorDown.transform.position = doorDown.transform.position - new Vector3(0, speedRotate*5, 0)* Time.deltaTime;
            doorUp.transform.position = doorUp.transform.position + new Vector3(0, speedRotate*5, 0)*Time.deltaTime;
            if (doorDown.transform.position.y < -20)
            {
                
                Destroy(doorDown);
                Destroy(doorUp);
            }
            
            
        }
    }
    
        
}
