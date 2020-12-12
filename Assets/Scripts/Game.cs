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
    [SerializeField] private GameObject _progressBarPanel;
    [SerializeField] private GameObject _winLelelEffectPrefab;

    private StartScreen _startScreen;
    private GameLostScreen _gameOverScreen;
    private NextLevelScreen _nextLevelScreen;
    private GameObject _winLelelEffect;

    private void Awake()
    {
        _startScreen = _start.GetComponent<StartScreen>();
        _gameOverScreen = _gameLost.GetComponent<GameLostScreen>();
        _nextLevelScreen = _nextLevel.GetComponent<NextLevelScreen>();
        _winLelelEffect = Instantiate(_winLelelEffectPrefab);
        _winLelelEffect.SetActive(false);
    }

    private void OnEnable()
    {
        _startScreen.PlayButtonClick += OnPlayButtonClick;
        _gameOverScreen.RestartButtonClick += OnRestartButtonClick;
        _nextLevelScreen.NextLevelButtonClick += OnNextLevelButtonClick;
        _leafChecker.GameLevelLost += OnGameLost;
        _leafChecker.GameLevelWin += OnGameWin;
        _scissors.CoinChanged += OnCoinChanged;
    }

    private void OnDisable()
    {
        _startScreen.PlayButtonClick -= OnPlayButtonClick;
        _gameOverScreen.RestartButtonClick -= OnRestartButtonClick;
        _nextLevelScreen.NextLevelButtonClick -= OnNextLevelButtonClick;
        _leafChecker.GameLevelLost -= OnGameLost;
        _leafChecker.GameLevelWin -= OnGameWin;
        _scissors.CoinChanged -= OnCoinChanged;
    }

    private void Start()
    {
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
        _winLelelEffect.SetActive(false);
        StartGame();
    }

    private void StartGame()
    {
        _treeCrownFiller.ReFillCrown();
        _leafChecker.ResetChecker();
        _scissors.ResetScissors();
        _progressBarPanel.SetActive(true);
    }

    public void OnGameLost()
    {
        _gameOverScreen.SetLevel(_treeCrownFiller.CurrentLevel);
        _treeCrownFiller.DropLeaves();
        _progressBarPanel.SetActive(false);
        _gameLost.SetActive(true);
    }

    public void OnGameWin()
    {
        _nextLevelScreen.SetLevel(_treeCrownFiller.CurrentLevel + " Done");
        _treeCrownFiller.DropYellowLeaves();
        _progressBarPanel.SetActive(false);
        _winLelelEffect.SetActive(true);
        _nextLevel.SetActive(true);
    }

    private void OnCoinChanged(int coin)
    {
        _score.text = coin.ToString();
    }
}
