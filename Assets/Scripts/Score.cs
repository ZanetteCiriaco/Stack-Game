using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Score: MonoBehaviour
{
    public static Score ScoreInstance {get; set;}
    private const string MaxScoreKey = "MaxScore";
    private CanvasGroup canvasGroup;
    [SerializeField] TMP_Text currentScoreText;
    [SerializeField] TMP_Text maxScoreText;
    void Awake()
    {
        ScoreInstance = this;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start() {
        maxScoreText.enabled = false;
        canvasGroup.alpha = 0;
    }

    public void SetScore(int score) {
        currentScoreText.text = $"{score}";
    }

    public void Hide() {
        canvasGroup.alpha = 0;
    }

    public void Show() {
        canvasGroup.alpha = 1;
    }

    public void ShowMaxScore() {
        int maxScore = GetMaxScore();
        maxScoreText.text = $"Recorde: {maxScore}";
        maxScoreText.enabled = true;
    }

    public void SaveMaxScore(int score) {
        if(score > GetMaxScore()) PlayerPrefs.SetInt(MaxScoreKey, score);
    }

    private int GetMaxScore() {
        return PlayerPrefs.GetInt(MaxScoreKey, 0);
    } 

    

}
