using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternLvlTwo : SpawnEnemiesPattern
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
                    addEnemy(2, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = 20;
                    portalPosition = 26;

                    portalLoadTime = 3;
                    portalFinzlSize = 3;

                    delay = 4;
                    spawnPortal(portals[1]);

                    list = new List<GameObject>();
                    addEnemy(1, enemyDamager);
                    spawnRate = 1f;
                    portalEnglePosition = -20;
                    portalPosition = 20;

                    portalLoadTime = 3;
                    portalFinzlSize = 2;

                    delay = 10;
                    spawnPortal(portals[2]);

                    list = new List<GameObject>();
                    addEnemy(1, enemyTank);
                    spawnRate = 1f;
                    portalEnglePosition = -30;
                    portalPosition = 30;

                    portalLoadTime = 6;
                    portalFinzlSize = 4;

                    delay = 14;
                    spawnPortal(portals[4]);

                    break;
                case 2:
///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(1, enemyDamager);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 40;
                    portalPosition = 25;

                    portalLoadTime = 2;
                    portalFinzlSize = 2;

                    delay = 0;
                    spawnPortal(portals[0]);

///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(2, enemyDamager);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 45;
                    portalPosition = 26;

                    portalLoadTime = 4;
                    portalFinzlSize = 3;

                    delay = 2;
                    spawnPortal(portals[1]);

///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(2, enemyDamager);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 55;
                    portalPosition = 23;

                    portalLoadTime = 2;
                    portalFinzlSize = 3;

                    delay = 5;
                    spawnPortal(portals[3]);

///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(1, enemyTank);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 15;
                    portalPosition = 23;

                    portalLoadTime = 3;
                    portalFinzlSize = 4;

                    delay = 8;
                    spawnPortal(portals[2]);

///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(1, enemyTank);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 50;
                    portalPosition = 30;

                    portalLoadTime = 4;
                    portalFinzlSize = 4;

                    delay = 14;
                    spawnPortal(portals[4]);
                    break;
                case 3:
                    list = new List<GameObject>();

                    addEnemy(1, enemyTank);

                    spawnRate = 1f;
                    portalEnglePosition = 45;
                    portalPosition = 24;

                    portalLoadTime = 7;
                    portalFinzlSize = 4;

                    delay = 0;
                    spawnPortal(portals[0]);

                    list = new List<GameObject>();

                    addEnemy(1, enemyDamager);

                    spawnRate = 1f;
                    portalEnglePosition = 55;
                    portalPosition = 24;

                    portalLoadTime = 3;
                    portalFinzlSize = 2;

                    delay = 4;
                    spawnPortal(portals[1]);

                    list = new List<GameObject>();

                    addEnemy(1, enemyDamager);

                    spawnRate = 1f;
                    portalEnglePosition = 35;
                    portalPosition = 24;

                    portalLoadTime = 3;
                    portalFinzlSize = 2;

                    delay = 4;
                    spawnPortal(portals[2]);

                    list = new List<GameObject>();

                    addEnemy(1, enemyTank);

                    spawnRate = 1f;
                    portalEnglePosition = 170;
                    portalPosition = 25;

                    portalLoadTime = 4;
                    portalFinzlSize = 4;

                    delay = 20;
                    spawnPortal(portals[3]);

                    list = new List<GameObject>();

                    addEnemy(1, enemyTank);

                    spawnRate = 1f;
                    portalEnglePosition = 160;
                    portalPosition = 30;

                    portalLoadTime = 12;
                    portalFinzlSize = 4;

                    delay = 12;
                    spawnPortal(portals[4]);
                    break;
                case 4:
                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(1, enemyDamager);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 170;
                    portalPosition = 25;

                    portalLoadTime = 2;
                    portalFinzlSize = 2;

                    delay = 0;
                    spawnPortal(portals[0]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(2, enemyDamager);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 175;
                    portalPosition = 26;

                    portalLoadTime = 4;
                    portalFinzlSize = 3;

                    delay = 2;
                    spawnPortal(portals[1]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();

                    patternType = WaveType.Squad;
                    formation = Sqad.Formation.Circle;
                    colCount = 1;
                    colSize = 6;
                    range = 1;


                    addEnemy(1, enemyTank);
                    addEnemy(1, enemyDamager);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 160;
                    portalPosition = 23;

                    portalLoadTime = 10;
                    portalFinzlSize = 6;

                    delay = 10;
                    spawnPortal(portals[3]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    patternType = WaveType.Simple;
                    addEnemy(1, enemyHeal);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 165;
                    portalPosition = 23;

                    portalLoadTime = 5;
                    portalFinzlSize = 4;

                    delay = 14;
                    spawnPortal(portals[2]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(1, enemyTank);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 170;
                    portalPosition = 30;

                    portalLoadTime = 4;
                    portalFinzlSize = 4;

                    delay = 16;
                    spawnPortal(portals[4]);
                    break;
                case 5:
                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(3, enemyDamager);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 170;
                    portalPosition = 25;

                    portalLoadTime = 2;
                    portalFinzlSize = 2;

                    delay = 0;
                    spawnPortal(portals[0]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(4, enemyDamager);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 175;
                    portalPosition = 26;

                    portalLoadTime = 4;
                    portalFinzlSize = 3;

                    delay = 2;
                    spawnPortal(portals[1]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();

                    patternType = WaveType.Squad;
                    formation = Sqad.Formation.Circle;
                    colCount = 1;
                    colSize = 10;
                    range = 2;


                    addEnemy(1, enemyTank);
                    addEnemy(1, enemyDamager);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 160;
                    portalPosition = 23;

                    portalLoadTime = 10;
                    portalFinzlSize = 6;

                    delay = 10;
                    spawnPortal(portals[3]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    patternType = WaveType.Simple;
                    addEnemy(2, enemyHeal);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 165;
                    portalPosition = 23;

                    portalLoadTime = 5;
                    portalFinzlSize = 4;

                    delay = 14;
                    spawnPortal(portals[2]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(1, enemyTank);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 170;
                    portalPosition = 30;

                    portalLoadTime = 4;
                    portalFinzlSize = 4;

                    delay = 16;
                    spawnPortal(portals[4]);
                    break;
                case 6:
                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(3, enemyDamager);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 45;
                    portalPosition = 32;

                    portalLoadTime = 2;
                    portalFinzlSize = 2;

                    delay = 0;
                    spawnPortal(portals[0]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(3, enemyDamager);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 55;
                    portalPosition = 26;

                    portalLoadTime = 4;
                    portalFinzlSize = 3;

                    delay = 5;
                    spawnPortal(portals[1]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();

                    patternType = WaveType.Squad;
                    formation = Sqad.Formation.Square;
                    colCount = 3;
                    colSize = 3;
                    range = 1;


                    addEnemy(1, enemyTank);
                    addEnemy(1, enemyDamager);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 65;
                    portalPosition = 23;

                    portalLoadTime = 10;
                    portalFinzlSize = 7;

                    delay = 14;
                    spawnPortal(portals[3]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    patternType = WaveType.Simple;
                    addEnemy(1, enemyHeal);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 30;
                    portalPosition = 25;

                    portalLoadTime = 5;
                    portalFinzlSize = 4;

                    delay = 16;
                    spawnPortal(portals[2]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(2, enemyTank);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 35;
                    portalPosition = 30;

                    portalLoadTime = 5;
                    portalFinzlSize = 6;

                    delay = 22;
                    spawnPortal(portals[4]);
                    break;
                case 7:
                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(5, enemyDamager);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 210;
                    portalPosition = 25;

                    portalLoadTime = 2;
                    portalFinzlSize = 2;

                    delay = 0;
                    spawnPortal(portals[0]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(1, enemyHeal);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 220;
                    portalPosition = 26;

                    portalLoadTime = 4;
                    portalFinzlSize = 3;

                    delay = 2;
                    spawnPortal(portals[1]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();



                    //addEnemy(1, enemyTank);
                    addEnemy(8, enemyDamager);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 215;
                    portalPosition = 23;

                    portalLoadTime = 10;
                    portalFinzlSize = 6;

                    delay = 10;
                    spawnPortal(portals[3]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();

                    patternType = WaveType.Squad;
                    formation = Sqad.Formation.Circle;
                    colCount = 1;
                    colSize = 12;
                    range = 2;

                    addEnemy(1, enemyTank);
                    addEnemy(1, enemyDamager);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 200;
                    portalPosition = 23;

                    portalLoadTime = 8;
                    portalFinzlSize = 8;

                    delay = 14;
                    spawnPortal(portals[2]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    patternType = WaveType.Simple;
                    addEnemy(1, enemyHeal);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 210;
                    portalPosition = 30;

                    portalLoadTime = 4;
                    portalFinzlSize = 4;

                    delay = 16;
                    spawnPortal(portals[4]);
                    break;
                case 8:
                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(3, enemyRusher);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 170;
                    portalPosition = 25;

                    portalLoadTime = 2;
                    portalFinzlSize = 2;

                    delay = 0;
                    spawnPortal(portals[0]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(4, enemyRusher);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 175;
                    portalPosition = 26;

                    portalLoadTime = 4;
                    portalFinzlSize = 3;

                    delay = 2;
                    spawnPortal(portals[1]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();

                    patternType = WaveType.Squad;
                    formation = Sqad.Formation.Circle;
                    colCount = 1;
                    colSize = 6;
                    range = 1;


                    addEnemy(1, enemyTank);
                    addEnemy(1, enemyRusher);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 160;
                    portalPosition = 23;

                    portalLoadTime = 10;
                    portalFinzlSize = 6;

                    delay = 10;
                    spawnPortal(portals[3]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    patternType = WaveType.Simple;
                    addEnemy(1, enemyHeal);
                    //
                    spawnRate = 1f;
                    portalEnglePosition = 165;
                    portalPosition = 23;

                    portalLoadTime = 5;
                    portalFinzlSize = 4;

                    delay = 14;
                    spawnPortal(portals[2]);

                    ///////////////////////////////////////////////////////////////////////////////////////
                    list = new List<GameObject>();
                    addEnemy(4, enemyRusher);
                    //
                    spawnRate = 0.5f;
                    portalEnglePosition = 170;
                    portalPosition = 30;

                    portalLoadTime = 4;
                    portalFinzlSize = 4;

                    delay = 16;
                    spawnPortal(portals[4]);
                    break;

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}

