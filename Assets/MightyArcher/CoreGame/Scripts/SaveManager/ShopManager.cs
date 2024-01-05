using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public CharacterDatabase characterDatabases;

    public Image imageholder;
    public TextMeshProUGUI nametxt;
    public TextMeshProUGUI coins;



    public int currentCharIndex;
    public Button buyButton;
    public Button ownedButton;

    public Image skillHolder;
    public TextMeshProUGUI nameskilltxt;
    public TextMeshProUGUI descriptiontxt;



    private void Start()
    {
        for (int i = 0; i < characterDatabases.characterCount; i++)
        {
            var character = characterDatabases.GetCharacter(i);

            if (character.price == 0)
            {
                character.isUnlocked = true;
            }
            else
            {
                #region Reset shop status
                //PlayerPrefs.DeleteKey(character.characterName);
                //character.isUnlocked = false;
                #endregion
                character.isUnlocked = PlayerPrefs.GetInt(character.characterName, 0) == 0 ? false : true;
            }


        }

        currentCharIndex = PlayerPrefs.GetInt("SelectedChar", 0);
        UpdateUI();
    }


    void UpdateUI()
    {
        coins.text = PlayerPrefs.GetInt("PlayerCoins", 0).ToString();
        var character = characterDatabases.GetCharacter(currentCharIndex);
        imageholder.sprite = character.characterSprite;
        nametxt.text = character.characterName;

        skillHolder.sprite = character.char_skill.icon;
        nameskilltxt.text = character.char_skill.name;
        descriptiontxt.text = character.char_skill.description;


        if (character.isUnlocked)
        {
            buyButton.gameObject.SetActive(false);
            ownedButton.gameObject.SetActive(true);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = character.price + " $";
            ownedButton.gameObject.SetActive(false);

            if (character.price <= PlayerPrefs.GetInt("PlayerCoins", 0))
            {
                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }

        }

    }


    public void UnlockChar()
    {
        var character = characterDatabases.GetCharacter(currentCharIndex);
        PlayerPrefs.SetInt(character.characterName, 1);
        PlayerPrefs.SetInt("SelectedChar", currentCharIndex);
        character.isUnlocked = true;
        PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("PlayerCoins", 0) - character.price);
        UpdateUI();

        //Save data on cloud

        var data = new Dictionary<string, object> { { "PlayerCoins", PlayerPrefs.GetInt("PlayerCoins") }, { character.characterName,1} };
        CloudSaveService.Instance.Data.Player.SaveAsync(data);

    }


    public void ChangeNext()
    {
        currentCharIndex++;
        if (currentCharIndex >= characterDatabases.characterCount)
        {
            currentCharIndex = 0;
        }
        //if (!character.isUnlocked)
        //  return;
        UpdateUI();
        PlayerPrefs.SetInt("SelectedChar", currentCharIndex);
    }

    public void ChangePrevious()
    {

        currentCharIndex--;
        if (currentCharIndex < 0)
        {
            currentCharIndex = characterDatabases.characterCount - 1;
        }

        //if (!character.isUnlocked)
        //  return;
        UpdateUI();
        PlayerPrefs.SetInt("SelectedChar", currentCharIndex);

    }

}
