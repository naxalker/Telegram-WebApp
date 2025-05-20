using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Elements")]
    [SerializeField] private TargetEmoji _targetEmoji;
    [SerializeField] private SpriteRenderer _playArea;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameObject _gameOverPanel;

    [Header("Game Settings")]
    [SerializeField] private float _gameTime = 30f;

    private int _score;
    private float _currentTime;
    private bool _isGameOver = false;

    private Bounds _playAreaBounds;

    public bool IsGameOver => _isGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _playAreaBounds = _playArea.bounds;

        StartGame();
    }

    private void Update()
    {
        if (_isGameOver) return;

        _currentTime -= Time.deltaTime;
        _timerText.text = "Time: " + Mathf.Max(0, Mathf.CeilToInt(_currentTime));

        if (_currentTime <= 0)
        {
            EndGame();
        }
    }

    public void AddScore(int amount)
    {
        if (_isGameOver) return;
        _score += amount;
        _scoreText.text = "Score: " + _score;
    }

    public void MoveTarget()
    {
        if (_isGameOver) return;

        float randomX = Random.Range(_playAreaBounds.min.x, _playAreaBounds.max.x);
        float randomY = Random.Range(_playAreaBounds.min.y, _playAreaBounds.max.y);

        _targetEmoji.transform.position = new Vector3(randomX, randomY, _targetEmoji.transform.position.z);
    }

    private void StartGame()
    {
        _score = 0;
        _currentTime = _gameTime;
        _isGameOver = false;
        _scoreText.text = "Score: " + _score;
        _timerText.text = "Time: " + Mathf.CeilToInt(_currentTime);
        _gameOverPanel.SetActive(false);
        MoveTarget();
    }

    private void EndGame()
    {
        _isGameOver = true;
        _timerText.text = "Time: 0";
        _targetEmoji.gameObject.SetActive(false);
        _gameOverPanel.SetActive(true);
        Debug.Log("Game Over! Final Score: " + _score);
    }

    public void RestartGame()
    {
        if (_targetEmoji != null) _targetEmoji.gameObject.SetActive(true);

        StartGame();
    }
}