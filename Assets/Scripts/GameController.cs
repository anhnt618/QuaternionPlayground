using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] int _numberOfEnemiesToSpawn = 0;
    public int NumberOfEnemiesToSpawn { get { return _numberOfEnemiesToSpawn; } set { _numberOfEnemiesToSpawn = value; } }
    
    public void AddEnemies(int count)
    {
        NumberOfEnemiesToSpawn += count;
    }
}
