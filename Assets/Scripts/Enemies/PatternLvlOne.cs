using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternLvlOne : SpawnEnemiesPattern
{
    public override void setPattern(int wave)
    {
        {
            enemieCountInList = 0;
            this.wave = wave;
            realPortalRange = portalRange + wave;
            
            switch (wave)
            {
                case 1:
                    list = new List<GameObject>();
                    addEnemy(1, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = 0;
                    portalPosition = 22;

                    portalLoadTime = 2;
                    portalFinzlSize = 2;

                    delay = 0;
                    spawnPortal(portals[0]);

                    list = new List<GameObject>();
                    addEnemy(1, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = 20;
                    portalPosition = 26;

                    portalLoadTime = 2;
                    portalFinzlSize = 2;

                    delay = 4;
                    spawnPortal(portals[1]);

                    list = new List<GameObject>();
                    addEnemy(3, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = -20;
                    portalPosition = 20;

                    portalLoadTime = 5;
                    portalFinzlSize = 3;

                    delay = 10;
                    spawnPortal(portals[4]);

                    /*
                    list = new List<GameObject>();
                    addEnemy(4, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = -10;
                    portalPosition = 30;

                    portalLoadTime = 8;
                    portalFinzlSize = 4;

                    delay = 10;
                    spawnPortal(portals[3]);

                    list = new List<GameObject>();
                    addEnemy(2, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = 30;
                    portalPosition = 22;

                    portalLoadTime = 5;
                    portalFinzlSize = 3;

                    delay = 14;
                    spawnPortal(portals[4]);
                    */
                    break;
                case 2:
                    list = new List<GameObject>();
                    addEnemy(1, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = 40;
                    portalPosition = 25;

                    portalLoadTime = 2;
                    portalFinzlSize = 2;

                    delay = 0;
                    spawnPortal(portals[0]);

                    list = new List<GameObject>();
                    addEnemy(1, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = 45;
                    portalPosition = 26;

                    portalLoadTime = 2;
                    portalFinzlSize = 2;

                    delay = 2;
                    spawnPortal(portals[1]);

                    list = new List<GameObject>();
                    addEnemy(2, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = 25;
                    portalPosition = 23;

                    portalLoadTime = 3;
                    portalFinzlSize = 3;

                    delay = 8;
                    spawnPortal(portals[2]);

                    list = new List<GameObject>();
                    addEnemy(4, enemyDamager);
                    spawnRate = 0.5f;
                    portalEnglePosition = 50;
                    portalPosition = 30;

                    portalLoadTime = 8;
                    portalFinzlSize = 4;

                    delay = 12;
                    spawnPortal(portals[4]);
                    break;
                case 3:
                    list = new List<GameObject>();
                    addEnemy(1, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = 45;
                    portalPosition = 24;

                    portalLoadTime = 2;
                    portalFinzlSize = 2;

                    delay = 0;
                    spawnPortal(portals[0]);

                    list = new List<GameObject>();
                    addEnemy(2, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = 38;
                    portalPosition = 26;

                    portalLoadTime = 3;
                    portalFinzlSize = 3;

                    delay = 4;
                    spawnPortal(portals[1]);

                    list = new List<GameObject>();
                    addEnemy(1, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = 50;
                    portalPosition = 23;

                    portalLoadTime = 1;
                    portalFinzlSize = 2;

                    delay = 8;
                    spawnPortal(portals[2]);

                    list = new List<GameObject>();
                    addEnemy(3, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = 40;
                    portalPosition = 25;

                    portalLoadTime = 4;
                    portalFinzlSize = 4;

                    delay = 12;
                    spawnPortal(portals[3]);

                    list = new List<GameObject>();
                    addEnemy(5, enemyDamager);
                    spawnRate = 0.5f;
                    portalEnglePosition = 160;
                    portalPosition = 30;

                    portalLoadTime = 12;
                    portalFinzlSize = 5;

                    delay = 20;
                    spawnPortal(portals[4]);
                    break;
                default:
                    break;

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        maxWaves = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
