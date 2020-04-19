using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pistonLevel : MonoBehaviour
{
    public char[,] Arr;//массив ситуации в игре
    public GameObject floor;//префабы пола
    public GameObject finish;//префаб блока финиша
    public GameObject wall;//префаб стены
    public GameObject piston;//префаб поршня
    public GameObject gameCube;//кубик
    public int level=1;//уровень
    public Transform parent;//трансформ этого объекта
    public Text second;//текст для второй стадии
    public Text third;//текст для третей стадии

    private int xCount;//длина в клетках по x
    private int yCount;//длина в клетках по y
    private List<GameObject> movingObjects = new List<GameObject>();//двигающиеся объекты

    // → ↓← ↑ 1234 направление поршней
    //  → ↓← ↑qwer направление выдвенутых поршней
    //□ 0 - пустая клетка
    // F 7- финиш
    // X  5 Объект
    // ■ 8

    private void Start()
    {
        setLevel(level);//процедура генерации определённого уровня
        int maxCountBlock =Mathf.Max( gameCube.GetComponent<CubeLogic>().xCount, gameCube.GetComponent<CubeLogic>().yCount, gameCube.GetComponent<CubeLogic>().zCount);//получить максимальную длину кубика по одной из осей
        gameCube.transform.localScale = new Vector3(0.95f / maxCountBlock, 0.95f / maxCountBlock, 0.95f / maxCountBlock);//подогнать размер куба 
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");//получить камеру
        if (cam.GetComponent<Menu>().GetStage() > 2) Win();//если в сохранение эта стадия пройденна, то перейти к следующей
    }

    public string  getLevel(int lvl)//получить код генерации уровня
    {
        if (lvl == 1) return "8482228888100051378000000888888888";//первые две цифры это размер поля, далее каждая цифра обозначает объект на поле
        if (lvl == 2) return "558722880453100088040388448";
        if (lvl == 3) return "558822880203100088025387848";
        if (lvl == 4) return "558227815408800031040884888";
        if (lvl == 5) return "778828888802000880120088005328810010880000078888888";
        else return "";
    }
    public void setLevel (int lvl)// собрать уровень
    {
        string seed = getLevel(lvl);//получить код генерации уровня
        xCount =int.Parse( seed.Substring(0, 1));//получить длину по x 
        yCount = int.Parse(seed.Substring(1,1));//получить длину по y
        seed = seed.Substring(2, seed.Length-2);//убрать из кода генерации певые две цифры, отвечающие за размер поля
        Arr = new char[xCount, yCount];//создать массив по размеру поля
        for (int y = 0; y < yCount; y++) 
        {
            for (int x = 0; x < xCount; x++)
            {
                Arr[x, y] = seed[y*xCount+x]; //заполнять массив
                spawnBox(Arr[x, y]-'0', x, y);//создавать объект на определённом месте
            }
            
        }
        
    }

    void spawnBox(int seedCode,int x, int y)//процедура создания объекта
    {
        if ((seedCode >= 1) && (seedCode <= 4))//если это код поршня, то...
        {
            GameObject newObj = Instantiate(piston, new Vector3(-(x - (xCount - 1) * 0.5f), 1, (y - (yCount - 1) * 0.5f)), Quaternion.Euler(0, seedCode * 90, 0), parent);//создать поршень с определённым поворотом
            newObj.GetComponent<MovingObjectPistonLevel>().Xcoor = x;//задать координаты объекту
            newObj.GetComponent<MovingObjectPistonLevel>().Ycoor = y;
            newObj.GetComponent<PistonLogic>().level = gameObject;//заполнить переменные в объекте
            movingObjects.Add(newObj);//добавить в лист двигующихся объектов
        }
        if (seedCode == 5)//если это кубик, то...
        {
            gameCube.transform.localPosition = new Vector3(-(x - (xCount - 1) * 0.5f), 1, (y - (yCount - 1) * 0.5f));//переместить кубик
            gameCube.GetComponent<MovingObjectPistonLevel>().Xcoor = x;//задать его переменные
            gameCube.GetComponent<MovingObjectPistonLevel>().Ycoor = y;
            movingObjects.Add(gameCube);
        }
        if (seedCode == 8) Instantiate(wall, new Vector3(-(x - (xCount - 1) * 0.5f), 1, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);//если код стены, то создать её
        if (seedCode == 7) Instantiate(finish, new Vector3(-(x - (xCount - 1) * 0.5f), 0, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);// если код финиша, то создать блок финиша уровнем ниже
        else//иначе уровнем ниже будут создаваться блоки пола и стен
        {
            if ((x > 0) && (x < xCount - 1) && (y > 0) && (y < yCount - 1))//если блок не с краю,то..
            {
                Instantiate(floor, new Vector3(-(x - (xCount - 1) * 0.5f), 0, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);//создаём пол
                
            }
            else
            {
                Instantiate(wall, new Vector3(-(x - (xCount - 1) * 0.5f), 0, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);//иначе стену
            }
        }
    }

    public void OperationPiston(int xCoor, int yCoor)//процедура работы поршня
    {
        
        
            if (Arr[xCoor, yCoor] =='1')//если поршень смотрит → , то
        {
                if (Arr[xCoor + 1, yCoor] != '0')// если следующая клетка в этом направление не пустая, то..
                {
                    
                    for (int i = xCoor + 1; i < xCount; i++)//идёт пересчёт всех клеток далее в этом напрвление
                    {
                        if (Arr[i, yCoor] == '8') break;// если стена, то сдвиг не возможен и сразу выход из цикла
                        if ((Arr[i, yCoor] == '0') || (Arr[i, yCoor] == '7'))//если пустая клетка или финиш(что тоже саиое),то
                    {
                            Shift(i, yCoor, 1, xCoor + 2);//организовать сдвиг (координата пусой клетки по x;координата пусой клетки по y;направление сдвига; клетка перед поршнем)

                        changePiston(xCoor, yCoor);//изменить состояние поршня
                            break;
                        }
                    }
                }
                else changePiston(xCoor, yCoor, 1);//если же клетка перед поршнем пустая, то изменить состояние поршня и передать заодно его направление

            }

            if (Arr[xCoor, yCoor] =='2')//аналогично но в другом направление
            {
                if (Arr[xCoor, yCoor + 1] != '0')
                {
                    for (int i = yCoor + 1; i < yCount; i++)
                    {
                        if (Arr[xCoor, i] == '8') break;
                        if ((Arr[xCoor, i] == '0') || (Arr[xCoor, i] == '7'))
                    {
                            Shift(xCoor, i,2, yCoor + 2);
                            
                            changePiston(xCoor, yCoor);
                            break;
                        }
                    }
                }
                else changePiston(xCoor, yCoor,2);
                
            }

            if (Arr[xCoor, yCoor] =='3')//аналогично но в другом направление
        {
                if (Arr[xCoor - 1, yCoor] != '0')
                {
                    
                    for (int i = xCoor - 1; i >= 0; i--)
                    {
                        if (Arr[i, yCoor] == '8') break;
                        if ((Arr[i, yCoor] == '0') || (Arr[i, yCoor] == '7'))
                        {
                            Shift(i, yCoor, 3, xCoor - 2);
                            
                            changePiston(xCoor, yCoor);
                            break;
                        }
                    }
                }
                else changePiston(xCoor, yCoor,3);
                

            }

            if (Arr[xCoor, yCoor] =='4')//аналогично но в другом направление
        {
                if (Arr[xCoor, yCoor - 1] != '0')
                {

                    for (int i = yCoor - 1; i >= 0; i--)
                    {
                        if (Arr[xCoor, i] == '8') break;
                        if ((Arr[xCoor, i] == '0')|| (Arr[xCoor, i] == '7'))
                        {
                            Shift(xCoor, i, 4, yCoor - 2);
                            
                            changePiston(xCoor, yCoor);
                            break;
                        }
                    }

                }
                else changePiston(xCoor, yCoor,4);
                
            }
        
        

    }
    private void Shift (int xCoor, int yCoor, int dir, int coorEnd)//сдвиг блоков
    {
        if (dir == 1)//если направление поршня →
        {
            for (int i = xCoor; i>= coorEnd; i--)//то начиная с пустой клетки и до клетки перед поршнем
            {
                changePosition(i - 1, yCoor,i, yCoor);//изменять позицию (координата сдвигаемого блок по x;координата сдвигаемого блок по y;конечная координата по x, конечная координата по y)
            }
        }

        if (dir == 2)//аналогично но в другом направление
        {
            for (int i = yCoor; i >= coorEnd; i--)
            {
                changePosition(xCoor,i - 1, xCoor, i);
            }
        }

        if (dir == 3)//аналогично но в другом направление
        {
            for (int i = xCoor; i <= coorEnd; i++)
            {
                changePosition(i + 1, yCoor, i, yCoor);
            }
        }

        if (dir == 4)//аналогично но в другом направление
        {
            for (int i = yCoor; i <= coorEnd; i++)
            {
                changePosition(xCoor,i + 1, xCoor, i );
            }
        }
    }
    public void changePosition(int startX,int startY,int endX,int endY)//смнить позицию
    {
        char checkFinish = Arr[endX, endY];//запомнить значение конечной координаты
        Arr[endX, endY] = Arr[startX, startY];//присвоить конечной клетке значение начальной
        foreach (GameObject mObj in movingObjects)//поиск по листу с двигующимися объектами
        {
            if ((mObj.GetComponent<MovingObjectPistonLevel>().Xcoor==startX) &&(mObj.GetComponent<MovingObjectPistonLevel>().Ycoor == startY))//если координаты совпадают,то..
            {
                mObj.GetComponent<MovingObjectPistonLevel>().ChangePosition(endX, endY,xCount,yCount);//вызвать функцию смены позиции
                break;
            }
            
        }
        
        Arr[startX, startY] = '0';//сделать начальную клетку пустой
        if (checkFinish == '7')//если конечная клетка была финишем,то..
        {
            if (Arr[endX, endY] == '5') Win();//если мы перемещали кубик, то это победа
            else Lose();//иначе проигрыш
        }
    }
    private void changePiston(int xPiston, int yPiston)//изменить состояние поршня (анимация)
    {
        foreach (GameObject mObj in movingObjects)//поиск по двигающимся объектам
        {
            if ((mObj.GetComponent<MovingObjectPistonLevel>().Xcoor == xPiston) && (mObj.GetComponent<MovingObjectPistonLevel>().Ycoor == yPiston))//нашли по координатам
            {
                mObj.GetComponent<PistonLogic>().Open();//вызвать функцию работы поршня
                break;
            }

        }
    }
    private void changePiston(int xPiston, int yPiston,int direct)//изменить состояние поршня (анимация) и взможно притянуть блок
    {
        foreach (GameObject mObj in movingObjects)//поиск по двигающимся объектам
        {
            if ((mObj.GetComponent<MovingObjectPistonLevel>().Xcoor == xPiston) && (mObj.GetComponent<MovingObjectPistonLevel>().Ycoor == yPiston))//нашли по координатам

            {
                mObj.GetComponent<PistonLogic>().Open(direct);//вызвать функцию работы поршня с направлением
                break;
            }

        }
    }

    private void Win()//процедура победы
    {
        Destroy(second);//уничтожить текст подсказки
        third.rectTransform.localPosition = third.rectTransform.localPosition - new Vector3(400, 0, 0);//переместить на экран подсказку из третьего этапа
        gameCube.transform.parent = null;//кубик вытащить из объекта уровня
        gameObject.AddComponent<Rigidbody>();//добавляет компонент для того чтобы поле упало
        transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);//немного сдивгаем вниз поле
        gameObject.GetComponent<Rigidbody>().mass = 100;//изменяем массу поля
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");//нацти камеру
        cam.GetComponent<Menu>().Save(level, 3);//сохранить этап
    }

    private void Lose()//процедура проигыша
    {
        //Debug.Log("lose");
        SceneManager.LoadScene("menu", LoadSceneMode.Single);//выйти в меню
    }

    private bool findTarget;//переменая становится true если нашли новую цель

    public void Update()
    {
        if ((transform.position.y < -5)&&(!findTarget))//если цель не найдена
        {
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");//то находим камеру
            cam.GetComponent<RotateCube>().newTarget(gameCube.transform);//и делаем кубик целью
            findTarget = true;//цель найденна
        }
        if (transform.position.y < -50) Destroy(gameObject);//если уровень далеко то его уничтожаем
        if (Input.GetKeyDown(KeyCode.R)) Lose();// r -для выхода в меню
    }
}
