using System;
using UnityEngine;


[Serializable]
public class Character 
{
    public string characterName;
    public Sprite characterSprite;
    public int charID;
    public bool isOwned;
    //public Sprite iconLeft
    //public Sprite iconRight
    public GameObject charLeft;
    public GameObject charRight;

    public int price;
    public bool isUnlocked;

    public Skill char_skill;
}
