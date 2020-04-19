using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Menu: MonoBehaviour
{
    public GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("levels"))//проверка наличия ключа
        {
            blocks = GameObject.FindGameObjectsWithTag("block");//находим все блоки
            foreach (GameObject block in blocks)
            {
                if (block.name[5] - '0' <= PlayerPrefs.GetInt("levels")) GameObject.Destroy(block);//если в имени блока цифра соответсвует пройденным уровням, то уничтожаем его
            }
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteKey("levels");
            PlayerPrefs.DeleteKey("lastLevel");
            PlayerPrefs.DeleteKey("stage");
            PlayerPrefs.DeleteKey("timeLevel");
        }
    }

    public void Save(int level,int stage)//сохранения уровня и стадии
    {
        PlayerPrefs.SetInt("lastLevel", level);//добавляем ключи
        PlayerPrefs.SetInt("stage",stage);
        if (PlayerPrefs.GetInt("lastLevel") > PlayerPrefs.GetInt("levels"))//если сохранеённый уровень больше пройденных, то..
        {
            PlayerPrefs.SetInt("levels", level);
        }
    }
    public void Save( int stage)//сохранить садию
    {
        PlayerPrefs.SetInt("stage", stage);
    }

    public void NewTime(int time)//сохранить время
    {
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
        return PlayerPrefs.GetInt("lastLevel");
    }

}
