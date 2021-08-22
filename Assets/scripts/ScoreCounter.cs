using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TypeOfScore {None ,ThreeOfAKind , FourOfAKind , FullHouse , SmallStraight , LargeStraight , Yahtzee , Chance}

[Serializable]
public class DiceRepetetion
{
    public int diceValue = 0;
    public int repitition = 0;

    DiceRepetetion(int d_Value , int rep)
    {
        diceValue = d_Value;
        repitition = rep;
    }
}

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private DiceCollector dice_Collector = null;
    private TypeOfScore finalScore = TypeOfScore.None;
    private bool has_FoundScore = false;
    private int[] diceValues = new int[5];
    private int[] repitition;
    private List<int> repitition_Values = new List<int>();

    #region Repititions & Straights

    [SerializeField] private DiceRepetetion[] dice_Rep = new DiceRepetetion[6];

    private int[][] three_OfAKind =
    {
        new int[] {1,1,3},
        new int[] {1,3,1},
        new int[] {3,1,1}
    };

    private int[][] four_OfAKind =
    {
        new int[] {1,4},
        new int[] {4,1}
    };

    private int[][] fullHouse =
    {
        new int[] {3,2},
        new int[] {2,3}
    };

    private int[][] small_Straights =
    {
        //1-4
        new int[] { 1 , 1 , 2 , 3 , 4 },
        new int[] { 1 , 2 , 2 , 3 , 4 },
        new int[] { 1 , 2 , 3 , 3 , 4 },
        new int[] { 1 , 2 , 3 , 4 , 4 },
        new int[] { 1 , 2 , 3 , 4 , 6 },
        //2-5
        new int[] { 2 , 2 , 3 , 4 , 5 },
        new int[] { 2 , 3 , 3 , 4 , 5 },
        new int[] { 2 , 3 , 4 , 4 , 5 },
        new int[] { 2 , 3 , 4 , 5 , 5 },
        //3-5
        new int[] { 1 , 3 , 4 , 5 , 6 },
        new int[] { 3 , 3 , 4 , 5 , 6 },
        new int[] { 3 , 4 , 4 , 5 , 6 },
        new int[] { 3 , 4 , 5 , 5 , 6 },
        new int[] { 3 , 4 , 5 , 6 , 6 }
    };

    private int[][] large_Straights =
    {
        new int[] { 1 , 2 , 3 , 4 , 5 },
        new int[] { 2 , 3 , 4 , 5 , 6 }
    };

    private int[][] yahtzees =
    {
        new int[] {1, 1, 1, 1, 1},
        new int[] {2, 2, 2, 2, 2},
        new int[] {3, 3, 3, 3, 3},
        new int[] {4, 4, 4, 4, 4},
        new int[] {5, 5, 5, 5, 5},
        new int[] {6, 6, 6, 6, 6}

    };
    #endregion

    public void ProduceResults()
    {
        has_FoundScore = false;
        repitition_Values = new List<int>();
        AssignAndArrangeArrayValues();
        DetectRepititions_AndProduceResults();
        DetectStraightsAndYahtzee();
        DisplayScore();
    }

    public void ResetScoreCounter()
    {
        foreach (DiceRepetetion dr in dice_Rep)
        {
            dr.repitition = 0;
        }
    }

    private void AssignAndArrangeArrayValues()
    {
        
        diceValues = dice_Collector.GetScoreCounterDice;
        
        for (int x = 0; x < diceValues.Length; x++)
        {
            for (int y = 0; y < diceValues.Length; y++)
            {
                int memory = 0;
                if(diceValues[x] < diceValues[y])
                {
                    memory = diceValues[x];
                    diceValues[x] = diceValues[y];
                    diceValues[y] = memory;
                }
            }
        }
    }

    private void DetectStraightsAndYahtzee()
    {
        foreach(int[] sStraight in small_Straights)
        {
            if(diceValues.SequenceEqual(sStraight))
            {
                finalScore = TypeOfScore.SmallStraight;
                has_FoundScore = true;
                return;
            }
        }
        foreach(int[] lStraight in large_Straights)
        {
            if(diceValues.SequenceEqual(lStraight))
            {
                finalScore = TypeOfScore.LargeStraight;
                has_FoundScore = true;
                return;
            }
        }
        foreach(int[] yahtzee in yahtzees)
        {
            if(diceValues.SequenceEqual(yahtzee))
            {
                finalScore = TypeOfScore.Yahtzee;
                has_FoundScore = true;
                return;
            }
        }
    }

    private void DetectRepititions_AndProduceResults()
    {
        for (int i = 0; i < diceValues.Length; i++)
        {
            foreach(DiceRepetetion dr in dice_Rep)
            {
                if(diceValues[i] == dr.diceValue)
                {
                    dr.repitition += 1;
                }
            }
        }
        foreach(DiceRepetetion dr in dice_Rep)
        {
            if(dr.repitition != 0)
            {
                repitition_Values.Add(dr.repitition);
            }
        }

        int[] rep = repitition_Values.ToArray();
        foreach (int[] dr in three_OfAKind)
        {
            if(rep.SequenceEqual(dr))
            {
                finalScore = TypeOfScore.ThreeOfAKind;
                has_FoundScore = true;
                return;
            }
        }
        foreach (int[] dr in four_OfAKind)
        {
            if(rep.SequenceEqual(dr))
            {
                finalScore = TypeOfScore.FourOfAKind;
                has_FoundScore = true;
                return;
            }
        }
        foreach (int[] dr in fullHouse)
        {
            if(rep.SequenceEqual(dr))
            {
                finalScore = TypeOfScore.FullHouse;
                has_FoundScore = true;
                return;
            }
        }
    }

    private void DisplayScore()
    {
        if (!has_FoundScore)
            finalScore = TypeOfScore.Chance;

        print(finalScore);
    }
}
