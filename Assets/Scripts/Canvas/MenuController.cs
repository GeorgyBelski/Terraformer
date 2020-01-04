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

    [Header("Sounds")]
    public AudioSource audioSource;
    public List<AudioClip> sounds;

    private static AudioSource audioSourceStatic;
    public static AudioClip defeatSound;
    public static AudioClip victorySound;

    private static bool isDefeatPlayed = false;

    void Start()
    {
        audioSourceStatic = audioSource;
        defeatSound = sounds[3];
        victorySound = sounds[4];
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
        //audioSourceStatic.PlayOneShot(victorySound, 0.6f);
        vectory.gameObject.SetActive(show);
    }
    public static void ShowDefeat(bool show)
    {
        if (!isDefeatPlayed)
        {
            audioSourceStatic.PlayOneShot(defeatSound, 0.6f);
            isDefeatPlayed = true;
        }


        defeat.gameObject.SetActive(show);
    }
    public void ExpandCreep() 
    {
        CreepHexagonGenerator.Expand();
        if (CreepHexagonGenerator.creepHexagonGenerator.isExpanding)
            audioSource.PlayOneShot(sounds[0], 0.6f);
        else
            audioSource.PlayOneShot(sounds[2], 0.6f);
    }
    public void RepairCreep()
    {
        CreepHexagonGenerator.Repair();
        if (CreepHexagonGenerator.creepHexagonGenerator.isRepairing)
            audioSource.PlayOneShot(sounds[1], 0.6f);
        else
            audioSource.PlayOneShot(sounds[2], 0.6f);
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
