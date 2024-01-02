
using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Character Skill", menuName = "Mighty Archer/Skill Data", order = 3)]
public class Skill : ScriptableObject
{
    public string name;
    public int idSkill;
    public int cooldownTime;
    // public Sprite icon;
    public string description;
}
