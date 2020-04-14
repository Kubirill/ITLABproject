using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeLogic : MonoBehaviour
{
    public int xCount; //количество блоков по x
    public int yCount;//количество блоков по y
    public int zCount;//количество блоков по z
    public GameObject[] elements;//элемменты из которых собирается кубик
    public int lvl;// уровень 

    private int[,,] Arr;//массив хранящий текущее состояние куба
    private int zeroCoord; // переменная хранит координаты пустой клетки. f(x,y,x)= x*100+y*10+z. 
    private List<int> randVector = new List<int>(); //хранит возможные изменения координат пустой клетки, для перемешивания кубика

    public void CreateCube() // запуск создания и перемешивания кубика
    {
       
        SetRandList();
        SetUp();
        RandSetUp();
        getArray();
        
    }

    private void SetRandList()//заполнить возможными изменениями координаты пустой клетки
    {
        randVector.Add(100);//смещение может происходить толко ортагонально
        randVector.Add(-100);// поэтому изменяться будет только одна координата
        randVector.Add(10);
        randVector.Add(-10);
        randVector.Add(1);
        randVector.Add(-1);
    }

    private void SetUp()
    {
        Arr = new int[xCount,yCount,zCount];// создание массива равному размеру куба
        zeroCoord = 111;//установка первоночальной координаты пустой клетки
        // 111?
        for (int x=0; x < xCount; x++)
        {
            for ( int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < zCount; z++)
                {
                    Arr[x, y,z] = x * 100 + y*10+z;//для каждой клетки присваевается её координата
                    if (Arr[x, y, z] != zeroCoord)// если клетка не пустая
                    {
                        string nameObj = returnName(Arr[x, y, z]);// получить имя объекта, который должен находится на этом месте
                        foreach (GameObject el in elements)// поиск объекта с таким именем
                        {

                            if (el.name == nameObj)//если объект с таким именем найден
                            {
                                GameObject cube = Instantiate(el, new Vector3((x - (xCount - 1) * 0.5f), (y - (yCount - 1) * 0.5f), (z - (zCount - 1) * 0.5f)), Quaternion.identity, transform);// создать такой объект в мире на положенном месте
                                cube.GetComponent<ElementLogic>().numElement = Arr[x, y, z];// задать целевые координаты этого элемента
                                el.GetComponent<ElementLogic>().parent = gameObject;
                            }
                        }
                    }
                }
            }
        }
        
    }
    

    private string returnName (int num)//получить имя элемента , который должен стоять в определённых координатах
    {
        int newX = num / 100;//извлечь координату x из переменной
        int newY = (num / 10) % 10;//извлечь координату y из переменной
        int newZ = num % 10;//извлечь координату z из переменной
        return returnSym(newX, xCount) + returnSym(newY, yCount) + returnSym(newZ, zCount);//сложить полученный сиволы в искомое имя
    }

    private string returnSym(int num,int max)//получить символ, в зависимости от координаты. 
    {
            if (num == 0) return "0";//если координата 0, то символ в имени "0"
            if (num == max - 1) return "n";//если координата последняя, то символ "n"
            return "x";// в остальных случаях "x"
    }

    private void RandSetUp()//перемешивания кубика
    {
        for (int i = 0; i < xCount*yCount*zCount*10; i++)//чем больше куб, тем больше итераций
        {


            int vr = randVector[Random.Range(0,randVector.Count)];// получить случайное изменение координаты
            int pointX = zeroCoord / 100 + vr / 100;// получить новую координату для x
            int pointY = (zeroCoord / 10) %10 + (vr / 10) % 10;// получить новую координату для y
            int pointZ = zeroCoord % 10 + vr % 10;// получить новую координату для z
            pointX = Mathf.Min(Mathf.Max(0, pointX), xCount-1);//ограничить значения размерами массива
            pointY = Mathf.Min(Mathf.Max(0, pointY), yCount-1);
            pointZ = Mathf.Min(Mathf.Max(0, pointZ), zCount-1);
            Vector3 trash = Shift(Arr[pointX,pointY,pointZ]);//вызов функция, для смещения элементов в кубе

        }
        GameObject[] elementsOnscene = GameObject.FindGameObjectsWithTag("element");//найти все элементы кубика
        for (int i = 0; i < elementsOnscene.Length; i++)//для каждого элемента
        {
            elementsOnscene[i].GetComponent<ElementLogic>().SetPos();//вызвать функцию получения позиции
        }
    }

    public Vector3 getStart(int elementId)//функция выдающая текущие координаты, для определённого элемена
    {
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < zCount; z++)
                {
                    if (Arr[x, y, z] == elementId) return new Vector3(x - (xCount - 1) * 0.5f, y - (yCount - 1) * 0.5f, z - (zCount - 1) * 0.5f);//когда место запрашеемого элемента в массиве найденно, вернуть координаты в пространстве
                }
            }
        }
        return new Vector3(0,0,0);
    }

    public Vector3 Shift(int elementId)//функция сдвига
    {
        int newX;//переменные новых координат
        int newY;
        int newZ;
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < zCount; z++)
                {
                    if (Arr[x, y, z] == elementId)
                    {
                        
                        if (Mathf.Abs(zeroCoord - (x * 100 + y * 10 + z)) == 10 || Mathf.Abs(zeroCoord - (x * 100 + y * 10 + z)) == 1 || Mathf.Abs(zeroCoord - (x * 100 + y * 10 + z)) == 100)//проверяется что запрашиваемая клетка соседня с пустой и ортогональна к ней, т.е различается толко одной координатой на 1
                        {

                            newX = zeroCoord/100;//разложить координаты пустой клетки
                            newY = (zeroCoord / 10) % 10;
                            newZ = zeroCoord%10;
                            Arr[newX, newY, newZ] = elementId;//записать в пустую клетку текущую
                            Arr[x, y, z] = 111;//записать в текущую клетку координаты 0ю 111?
                            zeroCoord = x * 100 + y * 10 + z;//зафиксировать новые координаты пустой клетки
                            CheckCube();//проверить законченна ли игра
                            return new Vector3(newX-(xCount-1)*0.5f, newY - (yCount - 1) * 0.5f, newZ - (zCount - 1) * 0.5f);//вернуть новое положение для нажатого элемента
                        }
                        else return new Vector3(x - (xCount - 1) * 0.5f, y - (yCount - 1) * 0.5f, z - (zCount - 1) * 0.5f);//вернуть текущее положение нажатого элемента, если блок не рядом с пустой клеткой
                    }
                }
            }
        }
        return new Vector3(0,0,0);
        
    }
    private void CheckCube()
    {
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (cam.GetComponent<RotateCube>().targetLook == transform)
        {
            bool correct = true;//предпологается изначально что куб собран правильно
            for (int x = 0; x < xCount; x++)
            {
                for (int y = 0; y < yCount; y++)
                {
                    for (int z = 0; z < zCount; z++)
                    {
                        int targetX = Arr[x, y, z] / 100;//разложить координаты хранящевося элемента
                        int targetY = (Arr[x, y, z] / 10) % 10;
                        int targetZ = Arr[x, y, z] % 10;
                        targetX = targetX * 2 / (xCount - 1);// после этого   targetX будет  следующим образом превращен 0->0, max->2, остальное->1;
                        targetY = targetY * 2 / (yCount - 1);
                        targetZ = targetZ * 2 / (zCount - 1);
                        //Debug.Log(targetX+ " " + targetY+ " "+ targetZ+" "+ Arr[x, y, z]);
                        if (!((x * 2 / (xCount - 1) == targetX) && (y * 2 / (yCount - 1) == targetY) && (z * 2 / (zCount - 1) == targetZ))) correct = false;// если хоть одна координата не совпадает, то куб собран не правильно
                        if (!correct) break;// если куб не правильный то выйти из цикла
                    }
                    if (!correct) break;// если куб не правильный то выйти из цикла
                }
                if (!correct) break;// если куб не правильный то выйти из цикла
            }
            if (correct)//еслли ошибок не найдено то перейти на следующий уровень
            {
                NextLevel();
            }
        }
        

    }
    public void NextLevel()
    {
        Debug.Log("next");
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        lvl++;
        cam.GetComponent<Menu>().Save(lvl, 1);
        SceneManager.LoadScene("Level"+lvl, LoadSceneMode.Single);
    }

    public void getArray()
    {
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < zCount; z++)
                {
                    Debug.Log(x+" "+y+" " + z +" " + Arr[x, y, z]);
                }
            }
        }
    }
   
}
