using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Unit
{
    public Skeleton(int hp, int damage, GameObject obj, Vector2 spawnPos, string name)
        : base(hp, damage, obj, spawnPos, name)
    {

    }
}
public class Zombie : Unit
{
    public Zombie(int hp, int damage, GameObject obj, Vector2 spawnPos, string name)
       : base(hp, damage, obj, spawnPos, name)
    {

    }
}
