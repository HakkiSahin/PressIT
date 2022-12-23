using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "LevelCreate")]
public class LevelCreator : ScriptableObject
{

    public List<LevelProperty> levels;


    [System.Serializable]
    public class LevelProperty
    {

        public int level;
        public List<GameObject> levelItems;

    }
}
