using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Rigidbody Dice_RB = null;
    public Rigidbody GetDiceRigidbody { get { return Dice_RB; } }

    private int throwValue = 0;
    public int DiceThrowValue { get { return throwValue; } set { throwValue = value; } }

    //Not sure what this is for see DiceCollector IsDiceInPosition()
    private bool is_InPosition = false;
    public bool IsInPosition { set { is_InPosition = value; } get { { return is_InPosition; } } }

    private void Awake()
    {
        Dice_RB = GetComponent<Rigidbody>();
    }
}
