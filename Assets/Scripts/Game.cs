using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject _start;
    [SerializeField] private GameObject _gameLost;
    [SerializeField] private GameObject _nextLevel;
    [SerializeField] private TreeCrownFiller _treeCrownFiller;
    [SerializeField] private Scissors _scissors;
    [SerializeField] private LeafChecker _leafChecker;
    [SerializeField] private TMP_Text _score;

    private StartScreen _startScreen;
    private GameOverScreen _gameOverScreen;
    private NextLevelScreen _nextLevelScreen;

    private void Awake()
    {
        _startScreen = _start.GetComponent<StartScreen>();
        _gameOverScreen = _gameLost.GetComponent<GameOverScreen>();
        _nextLevelScreen = _nextLevel.GetComponent<NextLevelScreen>();
    }

    private void OnEnable()
    {
        _startScreen.PlayButtonClick += OnPlayButtonClick;
        _gameOverScreen.RestartButtonClick += OnRestartButtonClick;
        _nextLevelScreen.NextLevelButtonClick += OnNextLevelButtonClick;
        _leafChecker.GameLevelLost += OnGameOver;
        _leafChecker.GameLevelWin += OnGameWin;
        _scissors.CoinChanged += OnCoinChanged;
    }

    private void OnDisable()
    {
        _startScreen.PlayButtonClick -= OnPlayButtonClick;
        _gameOverScreen.RestartButtonClick -= OnRestartButtonClick;
        _nextLevelScreen.NextLevelButtonClick -= OnNextLevelButtonClick;
        _leafChecker.GameLevelLost -= OnGameOver;
        _leafChecker.GameLevelWin -= OnGameWin;
        _scissors.CoinChanged -= OnCoinChanged;
    }

    private void Start()
    {
        Time.timeScale = 0;
        _treeCrownFiller.ReFillCrown();
        _startScreen.SetLevel(_treeCrownFiller.CurrentLevel);
        _start.SetActive(true);
    }

    private void OnPlayButtonClick()
    {
        _score.text = _scissors.Coin.ToString();
        _start.SetActive(false);
        StartGame();
    }

    private void OnRestartButtonClick()
    {
        _gameLost.SetActive(false);
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
        _gameOverScreen.SetLevel(_treeCrownFiller.CurrentLevel);
        _gameLost.SetActive(true);
    }

    public void OnGameWin()
    {
        Time.timeScale = 0;
        _treeCrownFiller.DropYellowLeaves();
        _nextLevelScreen.SetLevel(_treeCrownFiller.CurrentLevel + " Done");
        _nextLevel.SetActive(true);
    }

    private void OnCoinChanged(int coin)
    {
        _score.text = coin.ToString();
    }
}
