using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreIndicator : MonoBehaviour
{
    [SerializeField] Text scoreText;

    public void SetScore(int value) {
        scoreText.text = "Score:" + value;
    }
}
