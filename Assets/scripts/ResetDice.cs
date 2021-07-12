using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDice : MonoBehaviour
{
    [SerializeField] KeyCode reset_Key = KeyCode.None;

    [SerializeField] Dice[] dice = new Dice[5];
    [SerializeField] private DiceCollector dice_Collector = null;
    [SerializeField] private DiceThrower dice_Thrower = null;

    [SerializeField] Transform[] thrower_Children = new Transform[5];

    private void OnEnable()
    {
    }

    private void Update()
    {
        if(Input.GetKeyDown(reset_Key))
        {
            ResetTheDice();
            ResetDiceCollector();
            DiceThrowerReset();
        }
    }

    private void DiceThrowerReset()
    {
        dice_Thrower.SetHasSpunDice = false;
        dice_Thrower.SetHasThrownDice = false;
        for (int i = 0; i < 5; i++)
        {
            dice[i].transform.parent = thrower_Children[i].transform;
            dice[i].transform.localPosition = Vector3.zero;
        }
    }

    private void ResetTheDice()
    {
        foreach(Dice die in dice)
        {
            Rigidbody dieRB = die.GetDiceRigidbody;
            dieRB.constraints = RigidbodyConstraints.None;
            die.DiceThrowValue = 0;
            dieRB.useGravity = false;
        }
    }

    private void ResetDiceCollector()
    {
        dice_Collector.SetAssignedValuesBool = false;
        dice_Collector.SetDiceStoppedMovingBool = false;
        dice_Collector.SetHasFinishedJob = false;
        dice_Collector.gameObject.SetActive(false);
    }
}
