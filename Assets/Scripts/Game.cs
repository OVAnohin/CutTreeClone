using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject _start;
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private GameObject _nextLevel;
    [SerializeField] private TreeCrownFiller _treeCrownFiller;
    [SerializeField] private Scissors _scissors;
    [SerializeField] private LeafChecker _leafChecker;

    private StartScreen _startScreen;
    private GameOverScreen _gameOverScreen;
    private NextLevelScreen _nextLevelScreen;

    private void Awake()
    {
        _startScreen = _start.GetComponent<StartScreen>();
        _gameOverScreen = _gameOver.GetComponent<GameOverScreen>();
        _nextLevelScreen = _nextLevel.GetComponent<NextLevelScreen>();
    }

    private void OnEnable()
    {
        _startScreen.PlayButtonClick += OnPlayButtonClick;
        _gameOverScreen.RestartButtonClick += OnRestartButtonClick;
        _nextLevelScreen.NextLevelButtonClick += OnNextLevelButtonClick;
        _leafChecker.GameLevelLost += OnGameOver;
        _leafChecker.GameLevelWin += OnGameWin;
    }

    private void OnDisable()
    {
        _startScreen.PlayButtonClick -= OnPlayButtonClick;
        _gameOverScreen.RestartButtonClick -= OnRestartButtonClick;
        _nextLevelScreen.NextLevelButtonClick -= OnNextLevelButtonClick;
        _leafChecker.GameLevelLost -= OnGameOver;
        _leafChecker.GameLevelWin -= OnGameWin;
    }

    private void Start()
    {
        Time.timeScale = 0;
        _treeCrownFiller.ReFillCrown();
        _start.SetActive(true);
    }

    private void OnPlayButtonClick()
    {
        _start.SetActive(false);
        StartGame();
    }

    private void OnRestartButtonClick()
    {
        _gameOver.SetActive(false);
        StartGame();
    }

    private void OnNextLevelButtonClick()
    {
        _treeCrownFiller.NextLevel();
        _nextLevel.SetActive(false);
        StartGame();
    }

    private void StartGame()
    {
        _treeCrownFiller.ReFillCrown();
        _leafChecker.ResetChecker();
        _scissors.ResetScissors();
        Time.timeScale = 1;
    }

    public void OnGameOver()
    {
        Time.timeScale = 0;
        _treeCrownFiller.DropLeaves();
        _gameOver.SetActive(true);
    }

    public void OnGameWin()
    {
        Time.timeScale = 0;
        _treeCrownFiller.DropYellowLeaves();
        _nextLevel.SetActive(true);
    }
}
