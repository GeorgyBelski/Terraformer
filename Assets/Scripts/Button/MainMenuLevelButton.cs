using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LevelButtonType { Level1, Level2, Level3, Quit}
public class MainMenuLevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public LevelButtonType type;
    public GameObject description;
    public static bool firstSceneLoading = true;
    private void Start()
    {
        if (type != LevelButtonType.Quit)
        { description.SetActive(false); }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (type != LevelButtonType.Quit)
            description.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (type != LevelButtonType.Quit)
            description.SetActive(false);
    }

    public void Level1() 
    {
        
        SceneManager.LoadScene("LevelOne");
        RestrtScene();
    }
    public void Level2()
    {
        
        SceneManager.LoadScene("LevelTwo");
        RestrtScene();
    }
    public void Level3()
    {
        SceneManager.LoadScene("LevelThree");
        RestrtScene();
    }
    void RestrtScene() 
    {
        if (!firstSceneLoading)
        {
            MenuController.Restart(true);
        }
        else 
        {
            firstSceneLoading = false;
        }
    }

    public void QuitClick()
    {
        Application.Quit();
    }
}
