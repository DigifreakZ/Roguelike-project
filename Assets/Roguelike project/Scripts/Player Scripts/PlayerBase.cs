using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New character", menuName = "Character")]

public class PlayerBase : ScriptableObject
{
    public string creatureName;
    public float maxHealth;
    public float currentHealth;
    public float speed;
}
