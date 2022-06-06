using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Главный класс для юнита*/
public class Unit
{
    public GameObject unit;
    public Transform transform;
    public Vector2 targetPos;
    public string Name { private set; get; }
    public int Hp { get; set; }
    public int Damage { get; set; }

    public UnitList ally; // все союзники
    public UnitList rival; // все соперники
    public Unit(int hp, int damage, GameObject obj, Vector2 startPos, string name)
    {
        Hp = hp;
        Damage = damage;
        targetPos = Camera.main.ScreenToWorldPoint(startPos);
        unit = obj;
        Name = name;
    }
    virtual public bool Move(float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime*speed);
        if (transform.position.x == targetPos.x && transform.position.y == targetPos.y)
        {
            Debug.Log(Name + " пришел");
            return false;
        } 
        return true;

    }
    virtual public void Dead()
    {

    }
}
/*Класс работы со всей армией*/
public class UnitList : List<List<Unit>>
{
    public int DistBetweenUnits { set; get; }
    public int Hp { private set; get; }
    public enum Status { moveComplete, moving }
    public Status status;
    public UnitList(Army.Cell[,] army, int rows, int column, Dictionary<string, GameObject> prefabs,
        CreatureStack creatureStack, int distBetweenUnits)
    {
        DistBetweenUnits = distBetweenUnits;
        status = Status.moving;
        int xStart = Screen.width / 2 + distBetweenUnits / 2 + distBetweenUnits*10;
        int x = xStart;
        int y = Screen.height - Screen.height / 4;
        for (int i=0; i<rows; i++)
        {
            List<Unit> line = new List<Unit>(); 
            for (int j=0; j<column; j++)
            {
                Unit unit = ChooseType(army[i, j].Name, creatureStack, prefabs, new Vector2(x, y));
                if (unit == null)
                    continue;
                line.Add(unit);
                x += distBetweenUnits;
            }
            y -= Screen.height / 6;
            x = xStart;
            this.Add(line);
        }
        SetHp();
    }
    private Unit ChooseType(string name, CreatureStack creatureStack, 
        Dictionary<string, GameObject> prefabs, Vector2 spawnPos)
    {
        switch (name)
        {
            case "skeleton":
                return new Skeleton(creatureStack[name].Hp, creatureStack[name].Damage, 
                    prefabs[name], spawnPos, name);
            case "zombie":
                return new Zombie(creatureStack[name].Hp, creatureStack[name].Damage, 
                    prefabs[name], spawnPos, name);
            case "swordman":
                return new Swordman(creatureStack[name].Hp, creatureStack[name].Damage, 
                    prefabs[name], spawnPos, name);
            case "archer":
                return new Archer(creatureStack[name].Hp, creatureStack[name].Damage,
                    prefabs[name], spawnPos, name);
            default:
                return null;
        }
    }
    private void SetHp()
    {
        Hp = 0;
        for (int i = 0; i < this.Count; i++)
            for (int j = 0; j < this[i].Count; j++)
                Hp += this[i][j].Hp;
    }
    public void Move(float speed)
    {
        bool move = false;
        for (int i = 0; i < this.Count; i++)
            for (int j = 0; j < this[i].Count; j++)
                if (this[i][j].Move(speed))
                        move = true;
        if (!move)
            status = Status.moveComplete;
    }
    public void Attack()
    {

    }
    public float GetArmyHp()
    {
        float hp = 0;
        for (int i = 0; i < this.Count; i++)
            for (int j = 0; j < this[i].Count; j++)
                hp += this[i][j].Hp;
        return hp;
    }
}
