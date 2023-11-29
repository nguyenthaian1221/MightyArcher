using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class CharacterDatabase : ScriptableObject
{
    public Character[] character;
    //[Header("Client Info")]
    //public ulong[] clientId;                  // The clientId who selected this character
    //public int[] playerId;                    // With player is [1,2,3,4] -> more in case more player can play



    public int characterCount => character.Length;

    public Character GetCharacter(int index)
    {
        return character[index];    
    }
    
}
