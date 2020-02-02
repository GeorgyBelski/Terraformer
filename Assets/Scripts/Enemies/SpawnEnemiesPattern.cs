using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnEnemiesPattern : MonoBehaviour
{
    public enum WaveType
    {
        Simple, Squad 
    }

    public GameObject enemyDamager;
    public GameObject enemyTank;
    public GameObject enemyRusher;
    public GameObject enemyHeal;

    public List<PortalSettings> portals;

    protected float spawnRate;
    protected float portalPosition;
    protected float portalEnglePosition;
    protected float delay;
    protected int portalRange = 22;
    protected int realPortalRange;
    protected float portalLoadTime;
    protected float portalFinzlSize;

    public int maxWaves;
    public int wave;

    protected Sqad.Formation formation;
    protected WaveType patternType;
    protected int colCount;
    protected int colSize;
    protected float range;

    protected int enemieCountInList = 0;
    protected bool spawning;

    protected List<GameObject> list;

    void Start()
    {
        realPortalRange = portalRange;
    }

    public abstract void setPattern(int wave);

    

    protected void addEnemy(int count, GameObject enem)
    {
        for (int i = 0; i < count; i++)
        {
            list.Add(enem);
        }
            
    }

    protected void spawnPortal(PortalSettings portal)
    {
        //print("+");
        portal.gameObject.active = true;
        portal.transform.position = countVector();
        portal.setSettings(list, spawnRate, delay, portalLoadTime, portalFinzlSize);
    }

    private Vector3 countVector()
    {
        return new Vector3(portalPosition * Mathf.Sin(Sqad.DegreeToRadian(portalEnglePosition)), 1, portalPosition * Mathf.Cos(Sqad.DegreeToRadian(portalEnglePosition)));
    }

    public List<GameObject> getPattern()
    {
        return list; 
    }

    public WaveType getPatType()
    {
        return patternType;
    }
    //////
    ///-------------------------------------------
    ///Enemy Spawning
    ///-------------------------------------------
    /////
 /*   public void spawnEnemyes()
    {
        switch (patternType)
        {
            case WaveType.Simple:
                simpleSpawn();
                break;
            case WaveType.Squad:
                squadSpawn();
                break;
        }

    }

    private void simpleSpawn()
    {

        if (portal.deactivating)
            return;

        Enemy enem = Instantiate(list[enemieCountInList], new Vector3(
            portal.transform.position.x + (Mathf.Sin(SquadFormationSquare.DegreeToRadian(enemieCountInList * (360 / list.Count - 1))) * 2),
            0,
            portal.transform.position.z + (Mathf.Cos(SquadFormationSquare.DegreeToRadian(enemieCountInList * (360 / list.Count - 1))) * 2)), portal.transform.rotation)
            .GetComponent<Enemy>();

        enem.maxHealth = enem.maxHealth + 100 * wave;
        enem.health = enem.maxHealth;

        enemieCountInList++;

        if (list.Count <= enemieCountInList)
        {
            //enemieCountInList = 0;

            portal.deactivate();
            //print("+");
            //return;

        }


    }

    private void squadSpawn()
    {
        if (portal.deactivating)
            return;
        switch (formation)
        {
            case Sqad.Formation.Square:
                new SquadFormationSquare(list[0], list[1], colCount, colSize, range, realPortalRange, portalPosition);
                break;
            case Sqad.Formation.Circle:
                new SquadFormationCircle(list[0], list[1], colCount, colSize, range, realPortalRange, portalPosition);
                break;
        }
        portal.deactivate();
    }

    */

}
