using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public GameObject armyInterfaceBlock;

    public List<Button> buttonsRow0;
    public List<Button> buttonsRow1;
    public List<Button> buttonsRow2;
    public List<Button> buttonsRow3;

    public List<Button> e_buttonsRow0;
    public List<Button> e_buttonsRow1;
    public List<Button> e_buttonsRow2;
    public List<Button> e_buttonsRow3;

    public Text levelNameBanner;

    public List<Sprite> playerArmySprites;
    public List<Sprite> playerArmySpritesSelect;
    public List<Sprite> enemyArmySprites;
    public List<Sprite> enemyArmySpritesSelect;
    readonly string[] playerArmyNames = {"swordman", "archer", "dflt" };
    readonly string[] enemyArmyNames = {"skeleton", "zombie", "dflt" };

    public BattleManager battleManager; // ссылка на скрипт управлени€ боем
    public MenuManager menuManager; // ссылка на скрипт управлени€ меню
    public UnitInfoControl unitInfoControl; // ссылка на скрипт контрол€ вывод информации о юните

    private PlayerArmy playerArmy;
    private EnemyArmy enemyArmy;
    private CreatureStack creatureStack;
    public static Settings settings;

    private delegate void menuBack();
    private Stack<menuBack> menuBacks = new Stack<menuBack>();
    private void Start()
    {
        /*¬ременно*/
        
        List<List<string>> load_data = new List<List<string>>();

        List<string> row1 = new List<string>() { "dflt", "swordman", "dflt", "dflt", "swordman", "swordman" };
        List<string> row2 = new List<string>() { "swordman", "swordman", "archer", "dflt", "swordman", "archer"};
        List<string> row3 = new List<string>() { "dflt", "swordman", "swordman", "swordman", "swordman", "dflt" };
        List<string> row4 = new List<string>() { "archer", "swordman", "dflt", "dflt", "archer", "dflt" };
        load_data.Add(row1);
        load_data.Add(row2);
        load_data.Add(row3);
        load_data.Add(row4);
        /*временно*/
        playerArmy = new PlayerArmy(load_data, MakeButtonStack(), playerArmyNames, playerArmySprites, playerArmySpritesSelect, 4, 6);
        enemyArmy = new EnemyArmy(MakeEButtonStack(), enemyArmyNames, enemyArmySprites, enemyArmySpritesSelect, 4, 8);
        Settings set = new Settings();
        settings = set;
        Main.settings.language = Settings.Language.rus;
        creatureStack = new CreatureStack();
    }
    private void LoadData()
    {

    }
    public void LoadLevel(int level)
    {
        try
        {
            TextAsset file = Resources.Load("levels_" + ((level/10)*10).ToString() + "_" + ((level / 10 + 1) * 10).ToString()) as TextAsset; // levels_0_10
            string content = file.text;
            string levelData = content.Split('-')[level];
            string[] deviders1 = new string[] { "\n" };
            string[] deviders2 = new string[] { ", " };
            string[] levelDataMass = levelData.Split(deviders1, StringSplitOptions.None); // строки уровн€
            string[] levelName = levelDataMass[1].Split(deviders2, StringSplitOptions.None); // имена уровн€
            List<List<string>> levelArmy = new List<List<string>>();
            for (int i = 2; i< levelDataMass.Length-1; i++)
            {
                List<string> line = new List<string>();
                string[] creatures = levelDataMass[i].Split(deviders2, StringSplitOptions.RemoveEmptyEntries);
                foreach (string creature in creatures)
                    line.Add(creature.Trim());
                levelArmy.Add(line);
            }
            enemyArmy.LoadNewArmy(levelArmy);
            levelNameBanner.text = levelName[0];
            
        }
        catch(Exception e)
        {
            Debug.Log("Exception: " + e.Message);
        }
        finally
        {
            menuManager.MenuBlockSetNoActive();
        }

    }
    private List<List<Button>> MakeButtonStack()
    {
        List<List<Button>> res = new List<List<Button>>();
        res.Add(buttonsRow0);
        res.Add(buttonsRow1);
        res.Add(buttonsRow2);
        res.Add(buttonsRow3);
        return res;
    }
    private List<List<Button>> MakeEButtonStack()
    {
        List<List<Button>> res = new List<List<Button>>();
        res.Add(e_buttonsRow0);
        res.Add(e_buttonsRow1);
        res.Add(e_buttonsRow2);
        res.Add(e_buttonsRow3);
        return res;
    }
    /*ќбработчик нажатий на €чейки с войском*/
    public void CellTap(string ind)
    {
        int ind1 = ind[0]- '0';
        int ind2 = ind[1] - '0';
        if (playerArmy.selected.isSelected)
        {
            if (ind1 == playerArmy.selected.ind1 && ind2 == playerArmy.selected.ind2)
            {
                unitInfoControl.ShowImage(true, creatureStack[playerArmy[ind1, ind2].Name], playerArmy.unitIcons[playerArmy[ind1, ind2].Name]);
                playerArmy.SetDeselected();
            }
            else
            {
                playerArmy.SwapCell(ind1, ind2);
            }
        }
        else if (!playerArmy[ind1, ind2].Free)
        {
            playerArmy.SetSelected(ind1, ind2);
        }
    }
    /*ќбработчик нажатий на €чейки с мобами*/
    public void CellTapEnemy(string ind)
    {
        int ind1 = ind[0] - '0';
        int ind2 = ind[1] - '0';
        if (enemyArmy.selected.isSelected)
        {
            if (ind1 == enemyArmy.selected.ind1 && ind2 == enemyArmy.selected.ind2)
            {
                unitInfoControl.ShowImage(false, creatureStack[enemyArmy[ind1, ind2].Name], enemyArmy.unitIcons[enemyArmy[ind1, ind2].Name]);
                enemyArmy.SetDeselected();
            }
            else
            {
                enemyArmy.SetDeselected();
                if (!enemyArmy[ind1, ind2].Free)
                    enemyArmy.SetSelected(ind1, ind2);
            }
        }
        else if (!enemyArmy[ind1, ind2].Free)
        {
            enemyArmy.SetSelected(ind1, ind2);
        }
    }
    /* нопка нажати€ старта игры*/
    public void OnStart()
    {
        armyInterfaceBlock.SetActive(false);
        battleManager.StartBattle(playerArmy, enemyArmy, playerArmyNames, enemyArmyNames, creatureStack);
    }
    /* нопка нажати€ меню*/
    public void OnMenu()
    {
        menuManager.MenuBlockSetActive();
        if (menuBacks.Count==0)
            menuBacks.Push(menuManager.MenuBlockSetNoActive);
    }
    /* нопка нажати€ на уровни*/
    public void OnLevels()
    {
        menuManager.LevelsSetActive();
        menuBacks.Push(menuManager.MenuSetActive);
    }
    /* нопка "назад" во всем меню*/
    public void OnMenuBack() => menuBacks.Pop()();
}

public class Settings
{
    public enum Language { rus, eng}
    public enum Sound { on, off}
    public Language language;
    public Sound sound;

}
