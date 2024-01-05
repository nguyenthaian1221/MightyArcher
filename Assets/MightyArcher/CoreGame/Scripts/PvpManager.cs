using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PvpManager : MonoBehaviour
{
    public static int mapIndex;
    public static int charindex1;
    public static int charindex2;

    public CharacterDatabase characterDatabase;


    public TextMeshProUGUI name1;
    public TextMeshProUGUI name2;
    public TextMeshProUGUI maptext;


    public Image charSprite1;
    public Image charSprite2;



    private void Start()
    {
        mapIndex = 1;
        charindex1 = 0;
        charindex2 = 0;
        UpdateUI();
    }


    public void OnPlayBtnClick()
    {
        StartCoroutine(DelayedAction());
    }


    public void OnChar1BackBtnClicked()
    {
        charindex1--;

        if (charindex1 < 0)
        {
            charindex1 = characterDatabase.characterCount - 1;
        }
        //====
        var character1 = characterDatabase.GetCharacter(charindex1);
        if (!character1.isUnlocked)
        {
            OnChar1BackBtnClicked();
        }
        //=====
        UpdateUI();
    }

    public void OnChar1NextBtnClicked()
    {

        charindex1++;

        if (charindex1 >= characterDatabase.characterCount)
        {
            charindex1 = 0;
        }
        //====
        var character1 = characterDatabase.GetCharacter(charindex1);
        if (!character1.isUnlocked)
        {
            OnChar1NextBtnClicked();
        }

        UpdateUI();
    }

    public void OnChar2BackBtnClicked()
    {

        charindex2--;

        if (charindex2 < 0)
        {
            charindex2 = characterDatabase.characterCount - 1;
        }
        //====
        var character2 = characterDatabase.GetCharacter(charindex2);
        if (!character2.isUnlocked)
        {
            OnChar2BackBtnClicked();
        }

        UpdateUI();
    }

    public void OnChar2NextBtnClicked()
    {

        charindex2++;

        if (charindex2 >= characterDatabase.characterCount)
        {
            charindex2 = 0;
        }
        //====
        var character2 = characterDatabase.GetCharacter(charindex2);
        if (!character2.isUnlocked)
        {
            OnChar2NextBtnClicked();
        }

        UpdateUI();
    }

    public void OnMapBackBtnClicked()
    {
        mapIndex--;

        if (mapIndex < 1)
        {
            mapIndex = 3;
        }

        UpdateUI();
    }

    public void OnMapNextBtnClicked()
    {
        mapIndex++;

        if (mapIndex > 3)
        {
            mapIndex = 1;
        }

        UpdateUI();
    }


    void UpdateUI()
    {
        var character1 = characterDatabase.GetCharacter(charindex1);
        var character2 = characterDatabase.GetCharacter(charindex2);


        charSprite1.sprite = character1.characterSprite;
        charSprite2.sprite = character2.characterSprite;

        name1.text = character1.characterName;
        name2.text = character2.characterName;

        //<color=red>Map: <color=blue> {f} </color>

        maptext.text = String.Format("<color=red>Map: <color=blue> {0} </color>", mapIndex);

    }


    IEnumerator DelayedAction()
    {

        yield return new WaitForSeconds(1f);
        LoadMapByIndex();

    }

    void LoadMapByIndex()
    {
        //SceneManager.LoadScene("GameWithPlayer");
        switch (mapIndex)
        {
            case 1:
                SceneManager.LoadScene("GameWithPlayer");
                break;
            case 2:
                SceneManager.LoadScene("GameWithPlayer");
                break;
            case 3:
                SceneManager.LoadScene("GameWithPlayer");
                break;

        }


    }

}
