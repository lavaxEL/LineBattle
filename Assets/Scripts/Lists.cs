using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStack
{
    public Dictionary<string, Creature> creatures;
    public CreatureStack()
    {
        creatures = new Dictionary<string, Creature>();
        LoadCreature("PlayerCreatures");
        LoadCreature("EnemyCreatures");
    }
    private void LoadCreature(string path)
    {
        TextAsset file = Resources.Load(path) as TextAsset;
        string content = file.text;
        string[] deviders1 = new string[] { "----------" };
        string[] deviders2 = new string[] { "\n" };
        string[] deviders3 = new string[] { ", " };
        string[] units = content.Split(deviders1, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < units.Length; i++)
        {
            string[] parameters = units[i].Split(deviders2, StringSplitOptions.RemoveEmptyEntries);

            string[] names = parameters[1].Split(deviders3, StringSplitOptions.RemoveEmptyEntries);
            string nameEng = names[0].Trim();
            string nameRus = names[1].Trim();

            string[] stat = parameters[2].Split(deviders3, StringSplitOptions.RemoveEmptyEntries);
            int purchasePrice = Int32.Parse(stat[0].Trim());
            int stars = Int32.Parse(stat[1].Trim());
            int sellingPrice = Int32.Parse(stat[2].Trim());
            int attack = Int32.Parse(stat[3].Trim());
            int hp = Int32.Parse(stat[4].Trim());

            string descEng = parameters[3].Trim();
            string descRus = parameters[4].Trim();

            creatures.Add(nameEng, new Creature(attack, hp, purchasePrice, sellingPrice, stars, nameRus, nameEng, descRus, descEng));
        }
    }
    public Creature this[string index]
    {
        get => creatures[index];
        set => creatures[index] = value;
    }
}

public struct Creature
{
    private string nameRus;
    private string nameEng;
    private string descriptionRus;
    private string descriptionEng;
    public string Name
    {
        get
        {
            if (Main.settings.language == Settings.Language.rus)
                return nameRus;
            else
                return nameEng;
        }
    }
    public string Description
    {
        get
        {
            if (Main.settings.language == Settings.Language.rus)
                return descriptionRus;
            else
                return descriptionEng;
        }
    }
    public int PurchasePrice { private set; get; }
    public int SellingPrice { private set; get; }
    public int Damage { private set; get; }
    public int Hp { private set; get; }
    public int Stars { private set; get; }
    public Creature(int damage, int hp, int purchasePrice, int sellingPrice, int stars, params string[] str)
    {
        PurchasePrice = purchasePrice;
        SellingPrice = sellingPrice;
        Damage = damage;
        Hp = hp;
        Stars = stars;
        nameRus = str[0];
        nameEng = str[1];
        descriptionRus = str[2];
        descriptionEng = str[3];
    }
}

public class Levels
{
    public Level[] data;
    public Level this[int index]
    {
        get => data[index];
        set => data[index] = value;
    }
}
public struct Level
{
    private string nameRus;
    private string nameEng;
    public string[,] units;
    public string Name
    {
        get
        {
            if (Main.settings.language == Settings.Language.rus)
                return nameRus;
            else
                return nameEng;
        }
    }
}