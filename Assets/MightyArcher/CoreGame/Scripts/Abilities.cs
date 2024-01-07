using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    public Image abilityImage1;
    public Image abilityImage2;
    //public float cooldown1 = 5;
    public static bool isCooldownLeft = false;
    public static bool isCooldownRight = false;

    //public KeyCode ability1;
    private int saveRoundleft;
    private int roundExpireLeft;

    private int saveRoundRight;
    private int roundExpireRight;

    private void Start()
    {
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
    }


    private void Update()
    {
        AbilityLeft();
        AbilityRight();

    }


    public void OnClickLeft()
    {
        if (isCooldownLeft == false)
        {
            isCooldownLeft = true;
            abilityImage1.fillAmount = 1;
            saveRoundleft = GameController.skillRoundCountPlayerLeft;
            roundExpireLeft = GameController.roundExpireLeft;
        }
    }

    public void OnClickRight()
    {
        if (isCooldownRight == false)
        {
            isCooldownRight = true;
            abilityImage2.fillAmount = 1;
            saveRoundRight = GameController.skillRoundCountPlayerRight;
            roundExpireRight = GameController.roundExpireRight;
        }
    }



    void AbilityLeft()
    {

        var curRound = GameController.round;

        if (isCooldownLeft)
        {
            abilityImage1.fillAmount = CalculateCooldownTime(curRound, roundExpireLeft, saveRoundleft);

            if (abilityImage1.fillAmount <= 0)
            {
                abilityImage1.fillAmount = 0;
                isCooldownLeft = false;
            }


        }

    }

    void AbilityRight()
    {

        var curRound = GameController.round;

        if (isCooldownRight)
        {
            abilityImage2.fillAmount = CalculateCooldownTime(curRound, roundExpireRight, saveRoundRight);

            if (abilityImage2.fillAmount <= 0)
            {
                abilityImage2.fillAmount = 0;
                isCooldownRight = false;
            }


        }

    }

    //void AbilityLeft()
    //{
    //    //if (Input.GetKey(ability1) && isCooldown ==false)
    //    //{
    //    //    isCooldown = true;
    //    //    abilityImage1.fillAmount = 1;
    //    //}

    //    if (isCooldown)
    //    {
    //        abilityImage1.fillAmount -= 1 / cooldown1 * Time.deltaTime;

    //        if (abilityImage1.fillAmount <= 0)
    //        {
    //            abilityImage1.fillAmount = 0;
    //            isCooldown = false;
    //        }


    //    }

    //}


    // Formular to display cooldown time
    public float CalculateCooldownTime(int cur, int future, int saveRound)
    {
        if (future != saveRound)

            return 1 - (float)(cur - saveRound) / (float)(future - saveRound);
        else
            return 0;
    }







}
