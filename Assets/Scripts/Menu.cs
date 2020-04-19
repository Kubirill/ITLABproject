using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Menu: MonoBehaviour
{
    private int lastLevel = 1;
    private int openLevel=1;
    private int stage = 1;
    private int timeLevel=0;
    public GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("levels"))//проверка наличия ключа
        {
            openLevel = PlayerPrefs.GetInt("levels");//загружаем все ключи
            lastLevel = PlayerPrefs.GetInt("lastLevel");
            stage = PlayerPrefs.GetInt("stage");
            timeLevel = PlayerPrefs.GetInt("timeLevel");
            blocks = GameObject.FindGameObjectsWithTag("block");//находим все блоки
            foreach (GameObject block in blocks)
            {
                if (block.name[5] - '0' <= openLevel ) GameObject.Destroy(block);//если в имени блока цифра соответсвует пройденным уровням, то уничтожаем его
            }
        }

    }

    public void Save(int level,int stage)//сохранения уровня и стадии
    {
        PlayerPrefs.SetInt("lastLevel", level);//добавляем ключи
        PlayerPrefs.SetInt("stage",stage);
        if (lastLevel > openLevel)//если сохранеённый уровень больше пройденных, то..
        {
            openLevel = lastLevel;//пройденный уровень приравнивается к последнему
            PlayerPrefs.SetInt("levels", openLevel);
        }
    }
    public void Save( int stage)//сохранить садию
    {
        PlayerPrefs.SetInt("stage", stage);
    }

    public void NewTime(int time)//сохранить время
    {
        timeLevel=time;
        PlayerPrefs.SetInt("timeLevel", time);
    }
    public int GetTime()//получить время
    {
        return PlayerPrefs.GetInt("timeLevel"); ;
    }

    public int GetStage()//получить стадию
    {
       return PlayerPrefs.GetInt("stage");
    }
    public int GetLastLevel()//получить уровень
    {
        return lastLevel;
    }

}
