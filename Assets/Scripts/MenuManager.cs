using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuBlock; // ���� ���� ����
    public GameObject Menu; // ������� ����
    public GameObject Levels; // ������� ������

    public void MenuBlockSetActive() => MenuBlock.SetActive(true);
    public void MenuBlockSetNoActive() => MenuBlock.SetActive(false);
    private void AllSetNoActive()
    {
        Menu.SetActive(false);
        Levels.SetActive(false);
    }
    public void MenuSetActive()
    {
        AllSetNoActive();
        Menu.SetActive(true);
    }
    public void LevelsSetActive()
    {
        AllSetNoActive();
        Levels.SetActive(true);
    }
}
