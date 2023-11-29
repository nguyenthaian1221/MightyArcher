using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Save Information of client who selected char, clientid,playerId;
/// </summary>


[CreateAssetMenu(fileName = "Player Character", menuName = "Mighty Archer/Character Data", order = 2)]
public class CharacterDataSO : ScriptableObject
{


    [Header("Client Info")]
    public ulong clientId;                  // The clientId who selected this character
    public int playerId;
    public int charId;


    void OnEnable()
    {
        EmptyData();
    }

    public void EmptyData()
    {
        charId = -1;
        clientId = 0;
        playerId = -1;
    }

}
