using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDatabase;

    public TextMeshProUGUI nameText;

    public SpriteRenderer artworkSprite;

    private int selectedOption = 0;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        Load();
        UpdateCharacter(selectedOption);
    }


    public void NextOption()
    {
        selectedOption++;

        if (selectedOption >= characterDatabase.characterCount)
        {
            selectedOption = 0;
        }

        UpdateCharacter(selectedOption);
        Save();
    }

    public void BackOption()
    {
        selectedOption--;

        if (selectedOption <= 0)
        {
            selectedOption = characterDatabase.characterCount - 1;
        }

        UpdateCharacter(selectedOption);
        Save();
    }



    private void UpdateCharacter(int selectedOption)
    {
        Character character = characterDatabase.GetCharacter(selectedOption);

        artworkSprite.sprite = character.characterSprite;
        nameText.text = character.characterName;

    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }


    private void Save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
    }

}
