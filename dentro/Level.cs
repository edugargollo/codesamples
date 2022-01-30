using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level  {

    public Wave []waves;
}

[System.Serializable]
public class Wave
{
    public EnemySpawnData[] enemies;
}

[System.Serializable]
public class EnemySpawnData
{
    public int spawner;
    public EnemyBase enemy;
    public float waitTime;
}