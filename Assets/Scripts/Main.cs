using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    private enum GameState { WaitingToStart, Playing, GamerOver }
    [SerializeField] private GameState currentState = GameState.WaitingToStart;
    private int spawnerIndex = 1;
    private Spawner[] spawners;
    private Spawner currentSpawner;
    private int stackCount = 0;
    [SerializeField] Canvas Panel;
    private SoundManager soundManager;
    private int colorLevel;

    private void Awake() {
        spawners = FindObjectsOfType<Spawner>();
        soundManager = GetComponent<SoundManager>();
    }
    private void Start() {
        var random = new System.Random();
        colorLevel = random.Next(0, 100);
        WaitingToStart();
    }
    public void HandleClick(InputAction.CallbackContext context) {
        if(context.performed) {
            switch (currentState)
            {
                case GameState.WaitingToStart:
                    StartGame();
                    break;

                case GameState.Playing:
                    PlaceBlock();
                    break;

                case GameState.GamerOver:
                    ResetGame();
                    break;
            }
        }
    }
    private void PlaceBlock() {
        if (Block.currentBlock != null) {
            PlaceBlockReturn condition = Block.currentBlock.PlaceBlock();

            switch (condition) {
                case PlaceBlockReturn.MissedStack:
                    soundManager.GameOverSound();
                    GamerOver();
                    break;

                case PlaceBlockReturn.PerfectStack:
                    soundManager.PerfectStackSound();
                    stackCount++;
                    SpawnNewBlock();
                    break;

                case PlaceBlockReturn.PartialStack:
                    soundManager.PartialStackSound();
                    stackCount++;
                    SpawnNewBlock();
                    break;
            }
            
            Score.ScoreInstance.SetScore(stackCount);   
        }   
    }
    private void SpawnNewBlock() {
        spawnerIndex = spawnerIndex == 0 ? 1 : 0;
        currentSpawner = spawners[spawnerIndex];
        currentSpawner.SpawnBlock(stackCount, colorLevel);
    }
    private void WaitingToStart() {
        HomeTextAlpha(1);
        Score.ScoreInstance.Hide();
        currentState = GameState.WaitingToStart;
        soundManager.StartGameSound();
    }
    private void StartGame() {
        currentState = GameState.Playing;
        Score.ScoreInstance.Show();
        soundManager.StartGameSound();
        HomeTextAlpha(0);
        SpawnNewBlock();
    }
    private void GamerOver() {
        currentState = GameState.GamerOver;
        Score.ScoreInstance.SaveMaxScore(stackCount);
        Score.ScoreInstance.ShowMaxScore();

        Panel.transform.Find("EndGameText").GetComponent<TMP_Text>().enabled = true;
        
    }
    private void ResetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void HomeTextAlpha(int alpha) {
        Panel.transform.Find("HomeText").GetComponent<CanvasGroup>().alpha = alpha;
    }
}
