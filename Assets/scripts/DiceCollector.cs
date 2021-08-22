using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DiceCollector : MonoBehaviour
{
    #region The Dice and It's Position Values
    [SerializeField] private Dice[] thrown_Dice = new Dice[5] { null, null, null, null, null };
    [SerializeField] private Vector3[] dice_ArrangedPositions = new Vector3[5] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
    [SerializeField] private float move_ToPositionSpeed = 10.0f;
    #endregion

    //planning on exapnding that?
    #region Score Ccounter Variables
    [SerializeField] private ScoreCounter score_Counter = null;
    
    private int[] score_Counter_DiceVals = new int[5] {0,0,0,0,0};
    public int[] GetScoreCounterDice { get { return score_Counter_DiceVals; } }
    #endregion

    #region Booleans
    private bool has_DiceStoppedMoving = false;
    public bool SetDiceStoppedMovingBool { set { has_DiceStoppedMoving = value; } }

    private bool has_AssignedValues = false;
    public bool SetAssignedValuesBool { set { has_AssignedValues = value; } }

    private bool has_FinishedAssigningAndArranging = false;
    public bool SetHasFinishedJob { set { has_FinishedAssigningAndArranging = value; } }
    #endregion

    #region Ray Cast Variables
    [SerializeField] private LayerMask die_FaceLayer; 
    [SerializeField] private Vector3 ray_Position = Vector3.zero;
    [SerializeField] private float ray_Length = 3.0f;
    #endregion

    private void FixedUpdate()
    {
        if (!has_DiceStoppedMoving)
            DiceStopCheck();
    }

    private void Update()
    {
        
        if (!has_AssignedValues && has_DiceStoppedMoving)
        {
            AssignDiceValues();
        }
        else if (has_AssignedValues && has_DiceStoppedMoving && !has_FinishedAssigningAndArranging)
            MoveDiceToPositions();
    }

    private void DiceStopCheck()
    {
        for (int i = 0; i < 5; i++)
        {
            if(thrown_Dice[i].GetDiceRigidbody == null)
            {
                print("nor rigidbody found");
                return;
            }
            float tdVelMag = thrown_Dice[i].GetDiceRigidbody.velocity.magnitude;
            float tdAngVelMag = thrown_Dice[i].GetDiceRigidbody.angularVelocity.magnitude;
            if(tdVelMag > 0.0f && tdAngVelMag > 0.0f)
            {
                return;
            }
        }
        has_DiceStoppedMoving = true;
    }

    private void AssignDiceValues()
    {
        foreach (Dice td in thrown_Dice)
        {
            int scvIndex = 0;
            td.transform.parent = null;
            Rigidbody dieRB = td.GetComponent<Rigidbody>();
            dieRB.constraints = RigidbodyConstraints.FreezeRotation;
            Vector3 rayPosition = td.transform.position + ray_Position;
            RaycastHit hit = new RaycastHit();
            //Debug.DrawRay(rayPosition, Vector3.down*ray_Length, Color.red);

            if (Physics.Raycast(rayPosition , Vector3.down , out hit , ray_Length , die_FaceLayer))
            {
                Face face = hit.collider.gameObject.GetComponent<Face>();
                if (face != null)
                {
                    Debug.DrawRay(rayPosition, Vector3.down * ray_Length, Color.red, 5f);
                    td.DiceThrowValue = face.GetFaceValue;
                    score_Counter_DiceVals[scvIndex] = face.GetFaceValue;
                    scvIndex++;
                }
                else
                {
                    print("no face found for: " + hit.collider.name);
                    return;
                }
            }
        }
        has_AssignedValues = true;

        StartCoroutine(ArrangeDiceByValue());
    }

    private IEnumerator ArrangeDiceByValue()
    {
        yield return new WaitUntil(() => has_AssignedValues);
        for (int startingIndex = 0; startingIndex < 5; startingIndex++)
        {
            Dice memory = null;

            for (int currentIndex = 0; currentIndex < 5; currentIndex++)
            {
                if(thrown_Dice[currentIndex].DiceThrowValue > thrown_Dice[startingIndex].DiceThrowValue)
                {
                    memory = thrown_Dice[startingIndex];
                    thrown_Dice[startingIndex] = thrown_Dice[currentIndex];
                    thrown_Dice[currentIndex] = memory;
                }
            }
        }
    }

    private void MoveDiceToPositions()
    {
        for (int i = 0; i < 5; i++)
        {
            if (thrown_Dice[i] == null)
            {
                print("no die to move");
                return;
            }
            Vector3 pos = dice_ArrangedPositions[i];
            if (Vector3.Distance(thrown_Dice[i].GetDiceRigidbody.transform.position, pos) < 0.1f)
            {
                return;
            }
            thrown_Dice[i].GetDiceRigidbody.transform.position = Vector3.Lerp(thrown_Dice[i].GetDiceRigidbody.transform.position, pos, move_ToPositionSpeed * Time.deltaTime);
        }

        score_Counter.ProduceResults();
    }


    //NOT USED I'VE BEEN AWAY FOR TOO LONG??
    private void IsDiceInPosition()
    {
        foreach(Dice die in thrown_Dice)
        {
            if (!die.IsInPosition)
                return;
        }
        has_FinishedAssigningAndArranging = true;
    }
}
