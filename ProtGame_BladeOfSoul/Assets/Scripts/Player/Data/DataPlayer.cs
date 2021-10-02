using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class DataPlayer : ScriptableObject
{
    public float maxHealth;
    public float maxSpeed;
}
