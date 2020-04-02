using UnityEngine;
using System.Collections;ColumnsWidthusing System.Collections.Generic;

public class Inventory : MonoBehaviour
{

    const int INVENTORY_WINDOW_ID = 1; //id окна инвентаря

    public float ColumnsWidth = 40; //высота ячейки
    public float RowsHeight = 40; //ширина ячейки

    int invRows = 3; //количество колонок
    int invColumns = 1; //количество столбцов
    Rect inventoryWindowRect = new Rect(10, 10, 230, 265); //область окна
    bool isDraggable; //возможно ли перемещение предмета
    oneObjectInventory selectObj; //вспомогательная переменная куда заносим предмет инвентаря
    Texture2D dragImge; //текстура которая отображается при перетягивании предмета в инвентаре

    Dictionary<int, oneObjectInventory> InventoryPlayer = new Dictionary<int, oneObjectInventory>(); //словарь содержащий предметы инвентаря

    void Start()
    {

    }

    void Update()
    {

    }

    void OnGUI()
    {
        inventoryWindowRect = GUI.Window(INVENTORY_WINDOW_ID, inventoryWindowRect, firstInventory, "INVENTORY"); //создаем окно
    }

    void firstInventory(int id)
    {
        for (int y = 0; y < invRows; y++)
        {
            for (int x = 0; x < invColumns; x++)
            {
                GUI.Button(new Rect(50 + (x * ButtonHeight), 20 + (y * ButtonHeight), ColumnsWidth, ButtonHeight), (x + y * invColumns).ToString(), "itemInfo-InfoBG");
            }
        }
    }
}

}