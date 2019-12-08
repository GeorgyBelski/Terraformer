using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Image startMenu;
    public TextMeshProUGUI startVectory, startDefeat;
    public static Image menu; 
    public static TextMeshProUGUI vectory, defeat;
    public static bool isMenuShowed = false;
    void Start()
    {
        menu = startMenu;
        vectory = startVectory;
        defeat = startDefeat;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuShowed = !isMenuShowed;
        }
        if (isMenuShowed && Input.GetMouseButtonDown(1))
        {
            isMenuShowed = false;
        }

        ShowMenu(isMenuShowed);
    }
    public static void ShowMenu(bool show) 
    {
        menu.gameObject.SetActive(show);
        if (isMenuShowed != show) 
        { isMenuShowed = show; }
    }
    public static void ShowVictory(bool show) 
    {
        vectory.gameObject.SetActive(show);
    }
    public static void ShowDefeat(bool show)
    {
        defeat.gameObject.SetActive(show);
    }
    public void ExpandCreep() 
    {
        CreepHexagonGenerator.Expand();
    }
    public void RepairCreep()
    {
        CreepHexagonGenerator.Repair();
    }
    public void RestartClick() 
    {
        TowerManager.Restart();
        CreepHexagonGenerator.Restart();
        EnemyManagerPro.Restart();
        ResourceManager.Restart();
        ShowMenu(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitClick() 
    {
        Application.Quit();
    }
}
