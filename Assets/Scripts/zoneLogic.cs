using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct elementPosition //структура снежинок в игре
{
    public GameObject gObj;//сам объект
    public bool flipped;//превёрнут элемент или нет
    public float angle;//угол
    public int id;//номер элемента
    
    public static bool operator ==(elementPosition elsOne, elementPosition elsAns)//переопределение оператора равно
    {
        //Debug.Log(elsOne.angle + " " + elsAns.angle + " " + elsOne.flipped + " " + elsAns.flipped + " " + elsOne.id + " " + elsAns.id);
        if ((elsOne.id== elsAns.id)&& (elsOne.flipped == elsAns.flipped))//если номера элементов равны и сторона равна
            {
            int partAngle;//угол симметрии
            if (elsOne.gObj.GetComponent<traceryLogic>() != null) partAngle = elsOne.gObj.GetComponent<traceryLogic>().angleSimetry;//если у оодного объекта есть компонент то получаем его угол симетрии
            else partAngle = elsAns.gObj.GetComponent<traceryLogic>().angleSimetry;//иначе берём у другого объекта угол симметрии
            float angleOne = (elsOne.angle + 360) % partAngle;//получить остаток от деления на угол симметрии
            float angleAns = (elsAns.angle + 360) % partAngle;//получить остаток от деления на угол симметрии
            if ((Mathf.Abs(angleOne - angleAns) < 5)||(Mathf.Abs(angleOne - angleAns+partAngle) < 5) || (Mathf.Abs(angleOne - angleAns - partAngle) < 5)) return true;//еси разница между углами не большая, то вернуть true
            else return false;
            }
        else return false;
    }
    public static bool operator !=(elementPosition elPos, elementPosition elsAns)//переопрделение оператора неравенства
    {
        return !(elPos == elsAns);//вернуть не оператор равенства
    }
}
public class zoneLogic : MonoBehaviour
{
    public GameObject[] elements;//элементы узоров
    public GameObject[] answersElements;//элементы узоров на ответе
    public GameObject cam;//камера
    public Transform answerBlock;//блок правильного узора
    public GameObject doorDown;//нижняя часть двери
    public GameObject doorUp;//верхняя часть двери
    public Text first;//текст первой стадии
    public Text second;//текст второй части
    private bool finish;//переменая победы
    //public GameObject[] elementsOnScene = new GameObject[3];
    public float speedRotate=1;//скорость вращения

    elementPosition[] elementsOnScene = new elementPosition[3];//элементы на сцене
    elementPosition[] answer = new elementPosition[3];//элементы ответа
    private int index;// номер текущего изменяемого элемента
    public int level=1;//уровень

    private void Start()
    {
        setAnswerTraceryPazzle();//установить узор на ответ
        CreateVisualAnswer();// создать визуальный правильный узор

        if (cam.GetComponent<Menu>().GetStage() > 1)//если стадия пройденна
        {
            finish = true;//уровень пройденн
            cam.GetComponent<RotateCube>().enabled = true;//сделать активным скрипт вращения
            cam.GetComponent<Inventory>().enabled = false;//сделать неактивным инвентарь
            GameObject cube = GameObject.FindGameObjectWithTag("Cube");//найти куб
            GameObject.Destroy(first);//уничтожить текст перфой стадии
            second.rectTransform.localPosition = second.rectTransform.localPosition - new Vector3(400, 0, 0);//переместить текст второй части
            cube.GetComponent<CubeLogic>().CreateCube();//сгенерировать куб
            
            
        }
        
    }

    private void setAnswerTraceryPazzle()//задать узор
    {
        answer[0].gObj = answersElements[0];//установить для каждого элемента по порядку. Добавить объект в структуру
        answer[0].angle = Random.Range(0, level + 1) * 60 / (level + 1);//зарандомить угол в зависимости от уровня
        answer[1].gObj = answersElements[1];
        answer[1].angle = Random.Range(0, level + 1) * 90 / (level + 1);
        answer[2].gObj = answersElements[2];
        answer[2].angle = Random.Range(0, level + 1) * 90 / (level + 1);
        bool fliping;
        if (Random.Range(0, 2) >= 1) fliping = true;//зарандомить сторону
        else fliping = false;
        for (int i = 0; i < 3; i++)//для каждого элемента присвоить номеру и сторону
        {
            answer[i].flipped = fliping;
            answer[i].id = i;
        }
        for (int first = 0; first < 3; first++)
        {

            for (int second = 0; second < 3; second++)//перебор двух позиций в массиве
            {
                if (Random.Range(0, 2) >= 1)//с вероятностью 50% будут меняться местами два элемента в массиве
                {
                    elementPosition trush = answer[first];
                    answer[first] = answer[second];
                    answer[second] = trush;
                }
            }
        }
    }

    private void CreateVisualAnswer()//создать визуальныйцелевой узор
    {
        for (int i = 0; i < 3; i++)
        {
            int angle;//переменная для измеения координат в зависимости от стороны элемента
            if (answer[i].flipped) angle = 1;
            else angle = 0;
            Instantiate(answer[i].gObj, answerBlock.position + new Vector3(0, 0, -0.06f - 0.01f * i+0.01f*angle), Quaternion.Euler(0, angle*180, answer[i].angle), answerBlock);//создать элемент на блоке для ответа под определённым углом
        }
    }

    private void OnMouseDown()
    {
        if (!finish)//если уровень не завершён
        {

            if (cam.GetComponent<Inventory>().isDraggable) //если в инвентаре перетаскивается объект
            {
                string nameEl = cam.GetComponent<Inventory>().UseElement();//получить имя перетаскиваемого элемента
                foreach (GameObject element in elements)//найти элемент по имени
                {
                    if (element.name == nameEl)
                    {
                        elementsOnScene[index].gObj = Instantiate(element, transform.position - new Vector3(0, 0, index * 0.1f + 0.2f), Quaternion.identity);//добавить в зону перетаскиваемый элемент
                        elementsOnScene[index].gObj.GetComponent<traceryLogic>().cam = cam;// заполнить переменные
                        elementsOnScene[index].gObj.GetComponent<BoxCollider>().enabled = false;
                        elementsOnScene[index].flipped = false;
                        elementsOnScene[index].id = elementsOnScene[index].gObj.GetComponent<traceryLogic>().id;
                        elementsOnScene[index].angle = elementsOnScene[index].gObj.transform.rotation.z;
                        index++;//переместить активный элемент
                    }

                }


            }
            else //если же  элемент из массива не перетаскивается то по нажатию
            {
                if (index > 0)//если на полеесть элементы
                {
                    index = index - 1;//изменить активный элемент
                    cam.GetComponent<Inventory>().AddElement(elementsOnScene[index].gObj.GetComponent<traceryLogic>().id);//добавить активный элемент в массив
                    Destroy(elementsOnScene[index].gObj);//убрать объект из зоны
                    elementsOnScene[index].gObj = null;//обнулить информацию на объекте

                }
            }
        }
    }

    private void Update()
    {
        if (!finish)//если игра не законченна
        {
            if (index > 0)//на поле есть элементы
            {
                if (Input.GetKeyDown(KeyCode.F))// при нажатие на f
                {
                    elementsOnScene[index - 1].flipped = !elementsOnScene[index - 1].flipped;//изменит состояние тороны элемента
                    if (elementsOnScene[index - 1].flipped) elementsOnScene[index - 1].gObj.transform.position = elementsOnScene[index - 1].gObj.transform.position + new Vector3(0, 0, 0.1f);//сменистить объект из-за того что pivot не в центре
                    else elementsOnScene[index - 1].gObj.transform.position = elementsOnScene[index - 1].gObj.transform.position + new Vector3(0, 0, -0.1f);
                    elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 1, 0), 180);//повернуть элемент на 180градусов

                }
                if (Input.GetKey(KeyCode.Q))// при нажатии на qили e вращать элемент
                {
                    if (!elementsOnScene[index - 1].flipped) elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 0, 1), speedRotate);// угол в зависимости от того какой стороной повёрнута
                    else elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 0, 1), -speedRotate);
                    elementsOnScene[index - 1].angle = elementsOnScene[index - 1].gObj.transform.localEulerAngles.z;//запоминать угол

                }
                if (Input.GetKey(KeyCode.E))
                {

                    if (!elementsOnScene[index - 1].flipped) elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 0, 1), -speedRotate);
                    else elementsOnScene[index - 1].gObj.transform.Rotate(new Vector3(0, 0, 1), speedRotate);
                    elementsOnScene[index - 1].angle = elementsOnScene[index - 1].gObj.transform.localEulerAngles.z;
                }
            }
            if (index == 3)//если на поле все элементы
            {
                if (Input.GetKeyDown(KeyCode.Space))//по нажатию на пробел
                {
                    int correctCount = 0;//переменная количества правильных ответов
                    for (int i = 0; i < 3; i++)//идёт просмотр всех элементов
                    {
                        if (elementsOnScene[i] == answer[i]) correctCount++;//если структура на поле и вответе равна, то добавить 1 к количеству правильных ответов
                    }
                    if (correctCount >= 3)// если количество правильных ответов 3, то 
                    {
                        finish = true;//активировать переменную конца игры
                        cam.GetComponent<RotateCube>().enabled = true;//сделать активным скрипт вращения
                        cam.GetComponent<Inventory>().enabled = false;//сделать неактивным инвентарь
                        GameObject cube = GameObject.FindGameObjectWithTag("Cube");//найти куб
                        cube.GetComponent<CubeLogic>().CreateCube();//сгенерировать куб
                        GameObject.Destroy(first);//уничтожить текст перфой стадии
                        second.rectTransform.localPosition = second.rectTransform.localPosition - new Vector3(400, 0, 0);//переместить текст второй части
                        for (int i = 0; i < 3; i++) elementsOnScene[i].gObj.transform.parent = transform;//прикрепить объекты на сцене к двери
                        cam.GetComponent<Menu>().Save(level, 2);//сохранить стадию
                    }
                }
            }
        }
        else//если стадия пройденна
        {
            doorDown.transform.position = doorDown.transform.position - new Vector3(0, speedRotate*5, 0)* Time.deltaTime;//раздвигать двери
            doorUp.transform.position = doorUp.transform.position + new Vector3(0, speedRotate*5, 0)*Time.deltaTime;
            if (doorDown.transform.position.y < -20)//если двери далеко то они уничтожвются
            {
                GameObject.Destroy(doorDown);
                GameObject.Destroy(doorUp);
                for (int i = 0; i < 3; i++) Destroy(elementsOnScene[i].gObj);
            }
            
            
        }
    }
    
        
}
