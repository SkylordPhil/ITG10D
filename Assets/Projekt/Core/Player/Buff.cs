using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Player/Buff", order = 1)]
public class Buff : ScriptableObject
{
    [SerializeField] private string buffName;
}
