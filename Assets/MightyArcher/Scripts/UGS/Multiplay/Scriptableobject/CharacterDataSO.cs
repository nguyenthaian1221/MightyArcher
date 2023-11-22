using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Player Character", menuName = "Mighty Archer/Character Data", order = 2)]
public class CharacterDataSO : ScriptableObject
{


    [Header("Data")]
    public string characterName;            // Character name

    [Header("Client Info")]
    public ulong clientId;                  // The clientId who selected this character
    public int playerId;                    
    public bool isSelected;


    void OnEnable()
    {
        EmptyData();
    }

    public void EmptyData()
    {
        isSelected = false;
        clientId = 0;
        playerId = -1;
    }

}
