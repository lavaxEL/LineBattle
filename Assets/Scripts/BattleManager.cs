using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> playerObjects; // должно соответствовать порядку Main.playerArmyNames
    public List<GameObject> enemyObjects; // должно соответствовать порядку Main.enemyArmyNames

    public GameObject gameInterface; // блок игрового интерфейса
    public Image playerHpBar;
    public Image enemyHpBar;

    private UnitList playerList;
    private UnitList enemyList;

    private bool gameProcess = false;

    private const float unitsSpeed=4f;
    private const float timeBetweenActions=1f;

    public enum Action { move, attack, none }
    private Action action;

    public void StartBattle(PlayerArmy playerArmy, EnemyArmy enemyArmy, string[] playerArmyNames,
        string[] enemyArmyNames, CreatureStack creatureStack)
    {
        playerList = new UnitList(playerArmy.cells, playerArmy.Rows, playerArmy.Column,
            MakeArmyDictionary(playerArmyNames, playerObjects), creatureStack, Screen.width / 15);
        enemyList = new UnitList(enemyArmy.cells, enemyArmy.Rows, enemyArmy.Column,
            MakeArmyDictionary(enemyArmyNames, enemyObjects), creatureStack, -Screen.width / 15);

        Initialize(playerList, enemyList); // создание наших юинтов
        Initialize(enemyList, playerList); // создание вражеских юинтов
        playerHpBar.fillAmount = 1; // установка отображения hp бара армий
        enemyHpBar.fillAmount = 1;
        gameInterface.SetActive(true); // активируем игровой интерфейс
        action = Action.move; // изначальная фаза двжиения
        lastTime = Time.realtimeSinceStartup; // начинаем отсчет вермени
        gameProcess = true; // запуск игрового процесса
    }
    private Dictionary<string, GameObject> MakeArmyDictionary(string[] armyNames, List<GameObject> objects)
    {
        Dictionary<string, GameObject> res = new Dictionary<string, GameObject>();
        for (int i = 0; i < objects.Count; i++)
            res.Add(armyNames[i], objects[i]);
        return res;
    }
    private void Initialize(UnitList allyList, UnitList rivalList)
    {
        for (int i = 0; i < allyList.Count; i++)
        {
            for (int j = 0; j < allyList[i].Count; j++)
            {
                allyList[i][j].unit = Instantiate(allyList[i][j].unit,
                    new Vector3(allyList[i][j].targetPos.x, allyList[i][j].targetPos.y), Quaternion.identity);
                allyList[i][j].ally = allyList;
                allyList[i][j].rival = rivalList;
                Vector2 newTargetPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2 +
                    allyList.DistBetweenUnits / 2 + allyList.DistBetweenUnits * j, 0));
                allyList[i][j].targetPos = new Vector2(newTargetPos.x, allyList[i][j].targetPos.y);
                allyList[i][j].transform = allyList[i][j].unit.GetComponent<Transform>();
            }    
        }
        allyList.DistBetweenUnits *= -1;
    }
    private float lastTime;
    private void EventHandler()
    {
        switch(action)
        {
            case Action.move:
                {
                    playerList.Move(unitsSpeed);
                    enemyList.Move(unitsSpeed);
                    break;
                }
                
            case Action.attack:
                {
                    playerList.Attack();
                    enemyList.Attack();
                    playerHpBar.fillAmount = playerList.GetArmyHp() / playerList.Hp;
                    enemyHpBar.fillAmount = enemyList.GetArmyHp() / enemyList.Hp;
                    break;
                }     
        }
        if (action == Action.move &&
            playerList.status == UnitList.Status.moveComplete &&
            enemyList.status == UnitList.Status.moveComplete &&
            Time.realtimeSinceStartup - lastTime > timeBetweenActions)
            action = Action.attack;
    }
    private void Update()
    {
        if (!gameProcess)
            return;
        EventHandler();
    }
}
