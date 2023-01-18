using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Level", menuName = "LevelSystem/GameLevel", order = 1)]
///<summary> At the Momement a Level only consists of the Scene itself, but already implementing this type of managing Levels for later in Development</summary>
///
public class GameLevel : ScriptableObject
{
    [SerializeField]
    public string levelPath;


}
