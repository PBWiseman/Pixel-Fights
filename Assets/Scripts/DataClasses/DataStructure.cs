using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note: Entity has been moved to its own file due to its size

[System.Serializable]
public class FightEntity
{
    public int monster_id;
    public int count;
}

[System.Serializable]
public class Fight
{
    public int fight;
    public List<FightEntity> entities;
}

[System.Serializable]
public class Level
{
    public int level;
    public List<Fight> fights;
}

[System.Serializable]
public class LevelData
{
    public List<Enemy> enemies;
    public List<Level> levels;
}

[System.Serializable]
public class PlayerData
{
    public List<Player> players;
}