using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceThrower : MonoBehaviour
{
    #region Booleans
    private bool has_ThrownDice = false;
    public bool SetHasThrownDice { set { has_ThrownDice = value; } }

    private bool has_SpunDice = false;
    public bool SetHasSpunDice { set { has_SpunDice = value; } }
    #endregion

    [SerializeField] GameObject dice_Collector = null;
    [SerializeField] private Transform thrower_Transform = null;
    [SerializeField] private Rigidbody[] Dice = { null, null, null, null, null };
    [SerializeField] private float throw_Force = 10.0f;
    [SerializeField] private float thrower_SpinSpeed = 1.0f;
    [SerializeField] private float Dice_TorgueAmount = 10.0f;

    [SerializeField] private KeyCode dice_ThrowKey = KeyCode.None;

    private void Update()
    {
        if (!has_ThrownDice)
            SpinThrowerAndDice();

        if(Input.GetKeyDown(dice_ThrowKey) && !has_ThrownDice)
        {
            ThrowDice();
        }
    }

    private void SpinThrowerAndDice()
    {
        if (!has_SpunDice)
        {
            foreach (Rigidbody die in Dice)
            {
                die.AddTorque(Random.Range(10f, Dice_TorgueAmount), Random.Range(10f, Dice_TorgueAmount), Random.Range(10f, Dice_TorgueAmount));
            }
            has_SpunDice = true;
        }

        

        thrower_Transform.Rotate(thrower_Transform.up * thrower_SpinSpeed * Time.deltaTime);
    }

    private void ThrowDice()
    {
        for (int i = 0; i < Dice.Length; i++)
        {
            Dice[i].useGravity = true;
            Dice[i].AddForce(Dice[i].transform.parent.forward * throw_Force);
        }
        has_ThrownDice = true;
        StartCoroutine(DelayCollectorActivation());
    }

    private IEnumerator DelayCollectorActivation()
    {
        yield return new WaitForSeconds(0.5f);
        dice_Collector.SetActive(true);
    }
}
