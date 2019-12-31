using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    static private int score = 0;
    private const string SCORE_STRING = "SCORE:";

    public Text scoreText; 

    public void IncrementScore()
    {
        // set score and increment
        scoreText.text = SCORE_STRING + (++score);
    }
}
