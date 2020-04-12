using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Menu: MonoBehaviour
{

    private int compliteLevel=0;
    private int stage = 1;
    public GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("levels"))
        {
            compliteLevel = PlayerPrefs.GetInt("levels");
            stage = PlayerPrefs.GetInt("stage");
            blocks = GameObject.FindGameObjectsWithTag("block");
            foreach (GameObject block in blocks)
            {
                if (block.name[5] - '0' <= compliteLevel + 1) GameObject.Destroy(block);
            }
        }

    }

    public void Save(int level,int stage)
    {
        PlayerPrefs.SetInt("levels",level);
        PlayerPrefs.SetInt("stage",stage);
    }
    public void Save( int stage)
    {
        PlayerPrefs.SetInt("stage", stage);
    }

    public int GetStage()
    {
       return PlayerPrefs.GetInt("stage");
    }
    public int GetLevel()
    {
        return PlayerPrefs.GetInt("levels");
    }

}
