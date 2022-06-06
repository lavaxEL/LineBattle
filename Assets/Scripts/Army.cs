using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*јбстрактный класс армии*/
public class Army
{
    public int Rows { get; private set; }
    public int Column { get; private set; }
    public Cell[,] cells;
    public Dictionary<string, Sprite> unitIcons;
    public Dictionary<string, Sprite> unitIconsSelect;
    public Army(List<List<Button>> buttons,
        string[] names, List<Sprite> sprites, List<Sprite> spritesSelect, int rows, int column)
    {
        Rows = rows;
        Column = column;
        cells = new Cell[Rows, Column];
        selected = new Selected();
        unitIcons = new Dictionary<string, Sprite>();
        unitIconsSelect = new Dictionary<string, Sprite>();
        for (int i = 0; i < sprites.Count; i++) // заполнение словар€ со спрайтами
            unitIcons.Add(names[i], sprites[i]);
        for (int i = 0; i < spritesSelect.Count; i++) // заполнение словар€ со спрайтами выбранными
            unitIconsSelect.Add(names[i], spritesSelect[i]);
    }
    public void InsertCell(string name, params int[] ind) // вставл€ем нового юнита в €чейку
    {
        cells[ind[0], ind[1]].Free = false;
        cells[ind[0], ind[1]].icon.GetComponent<Image>().sprite = unitIcons[name];
        cells[ind[0], ind[1]].Name = name;
    }
    public void RemoveCell(params int[] ind) // удачл€ем юнита из €чейки
    {
        cells[ind[0], ind[1]].Free = true;
        cells[ind[0], ind[1]].icon.GetComponent<Image>().sprite = unitIcons["dflt"];
    }
    public void SetSelected(params int[] ind) // отмечаем €чейку как выбранную
    {
        selected.isSelected = true;
        selected.ind1 = ind[0];
        selected.ind2 = ind[1];
        cells[ind[0], ind[1]].icon.GetComponent<Image>().sprite = unitIconsSelect[cells[ind[0], ind[1]].Name];
    }
    public void SetDeselected() // отмен€ем выбранность €чейки
    {
        selected.isSelected = false;
        cells[selected.ind1, selected.ind2].icon.GetComponent<Image>().sprite = unitIcons[cells[selected.ind1, selected.ind2].Name];
    }
    public Cell this[int i1, int i2] // индексатор к массиву €чеек
    {
        get => cells[i1, i2];
        set => cells[i1, i2] = value;
    }
    public struct Selected // структура дл€ выделенной €чейки
    {
        public int ind1;
        public int ind2;
        public bool isSelected;
    }
    public Selected selected;
    public struct Cell // структура €чейки армии
    {
        public bool Free { get; set; }
        public string Name { get; set; }
        public Button icon;
        public Cell(string name, Button button, Sprite sprite)
        {
            Free = name != "dflt" ? false : true;
            Name = name;
            icon = button;
            icon.GetComponent<Image>().sprite = sprite;
        }
    }
}
/* ласс армии игрока*/
public class PlayerArmy : Army
{
    public PlayerArmy(List<List<string>> army, List<List<Button>> buttons,
        string[] names, List<Sprite> sprites, List<Sprite> spritesSelect, int rows, int column)
        : base(buttons, names, sprites, spritesSelect, rows, column)
    {
        
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Column; j++)
                cells[i, j] = new Cell(army[i][j], buttons[i][j], unitIcons[army[i][j]]);
    }
    public void SwapCell(params int[] ind)
    {
        Cell temp = cells[ind[0], ind[1]];
        cells[ind[0], ind[1]] = cells[selected.ind1, selected.ind2];
        cells[selected.ind1, selected.ind2] = temp;
        Button tmp = cells[ind[0], ind[1]].icon;
        cells[ind[0], ind[1]].icon = temp.icon;
        cells[selected.ind1, selected.ind2].icon = tmp;
        cells[ind[0], ind[1]].icon.GetComponent<Image>().sprite = unitIcons[cells[ind[0], ind[1]].Name];
        cells[selected.ind1, selected.ind2].icon.GetComponent<Image>().sprite = unitIcons[cells[selected.ind1, selected.ind2].Name];
        SetDeselected();
    }
}
/* ласс армии бота*/
public class EnemyArmy : Army
{
    public EnemyArmy(List<List<Button>> buttons,
        string[] names, List<Sprite> sprites, List<Sprite> spritesSelect, int rows, int column)
         : base(buttons, names, sprites, spritesSelect, rows, column)
    {
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Column; j++)
                cells[i, j] = new Cell("dflt", buttons[i][j], unitIcons["dflt"]);
    }
    public void LoadNewArmy(List<List<string>> army)
    {
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Column; j++)
                RemoveCell(i, j);
        for (int i = 0; i < army.Count; i++)
            for (int c = army[i].Count - 1, j = Column - 1; c >= 0; c--, j--)
                InsertCell(army[i][c], i, j);
    }
}



//public abstract class Army
//{
//    public string name;
//    public Army()
//    {

//    }
//    public void Insert() 
//    {
//        // добавл€ем юнит в €чейку
//    } 
//    public void Remove()
//    {
//        // удал€ем юнит из €чейки
//    }
//}
//public interface IPlayerArmy
//{
//    public void Swap();
//}
//public interface IEnemyArmy
//{
//    public void MakeSomeBad();
//}
//public class PlayerArmy : Army, IPlayerArmy
//{
//    public PlayerArmy()
//    {

//    }
//    public void Swap()
//    {
//        // мен€ем местами €чейки
//    }
//}
//public class EnemyArmy : Army, IEnemyArmy
//{
//    public EnemyArmy()
//    {

//    }
//    public void MakeSomeBad()
//    {

//    }
//}


//public class MainClass
//{
//    public void Main()
//    {
//        PlayerArmy playerArmy = new PlayerArmy();
//        playerArmy[0, 0] = new Army.Cell();
//        /*равносильно*/
//        PlayerArmy playerArmy1 = new PlayerArmy();
//        playerArmy1.cells[0,0] = new Army.Cell();
//    }
//}









