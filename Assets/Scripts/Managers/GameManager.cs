using Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Managers {

  public class GameManager : MonoBehaviour {

    private const float DownInputDelay = 0.05f;
    private const float InitialDropSpeed = 0.5f;
    private const float InputDelay = 0.5f;

    private Piece _activePiece;
    [SerializeField] private Board _board;
    private Camera _camera;
    private float _downInputTimer;
    private float _dropSpeed;
    private float _dropTimer;
    private bool _gameOver;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Ghost _ghost;
    [SerializeField] private Holder _holder;
    private bool _inputReady;
    private float _inputTimer;
    private bool _isPaused;
    [SerializeField] private GameObject _pausePanel;
    private bool _rotateClockwise = true;
    [SerializeField] private IconToggle _rotationIconToggle;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private GameObject _screenFader;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private Spawner _spawner;

    public void Hold() {
      if (_holder == null) {
        return;
      }

      if (_holder.HeldPiece == null) {
        _holder.Catch(_activePiece);
        _activePiece = _spawner.SpawnPiece();

        PlayFx(_soundManager.HoldSound);
      } else if (_holder.canRelease) {
        Piece piece = _activePiece;
        _activePiece = _holder.Release();
        _activePiece.transform.position = _spawner.transform.position;
        _holder.Catch(piece);

        PlayFx(_soundManager.HoldSound);
      } else {
        Debug.LogWarning("HOLDER: Wait for cool down");
        PlayFx(_soundManager.ErrorSound);
      }

      if (_ghost) {
        _ghost.Reset();
      }
    }

    public void RestartLevel() {
      Time.timeScale = 1f;
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TogglePause() {
      if (_gameOver || !_pausePanel) {
        return;
      }

      _isPaused = !_isPaused;
      _pausePanel.SetActive(_isPaused);

      if (_soundManager) {
        _soundManager.musicSource.volume = _isPaused ? _soundManager.MusicVolume * 0.25f : _soundManager.MusicVolume;
      }

      Time.timeScale = _isPaused ? 0 : 1;
    }

    public void ToggleRotation() {
      _rotateClockwise = !_rotateClockwise;

      if (_rotationIconToggle) {
        _rotationIconToggle.ToggleIcon();
      }
    }

    private void DropPiece() {
      _activePiece.MoveDown();

      if (_board.IsValidPosition(_activePiece)) {
        return;
      }

      _activePiece.MoveUp();

      if (_board.IsAboveThreshold(_activePiece)) {
        GameOver();
      } else {
        _board.StorePieceInGrid(_activePiece);

        if (_ghost) {
          _ghost.Reset();
        }

        if (_holder) {
          _holder.canRelease = true;
        }

        _activePiece = _spawner.SpawnPiece();
        _board.ClearAllRows();

        PlayFx(_soundManager.DropSound);

        if (_board.CompletedRows == 0) {
          return;
        }

        _scoreManager.ScoreLines(_board.CompletedRows);

        if (_scoreManager.LeveledUp) {
          PlayFx(_soundManager.LevelUp);
          _dropSpeed = Mathf.Clamp(InitialDropSpeed - ((float)_scoreManager.Level - 1) * 0.05f, 0.05f, 1f);
        } else {
          if (_board.CompletedRows > 1) {
            PlayFx(_soundManager.GetRandomVocal());
          }
        }

        PlayFx(_soundManager.ClearRowSound);
      }
    }

    private void GameOver() {
      Debug.LogWarning($"{_activePiece.name} is above the limit - Game Over");
      _gameOver = true;

      if (_gameOverPanel != null) {
        _gameOverPanel.SetActive(true);
      }

      PlayFx(_soundManager.GameOverVocal);
      PlayFx(_soundManager.GameOverSound);
    }

    private void LateUpdate() {
      if (_ghost != null) {
        _ghost.DrawGhost(_activePiece, _board);
      }
    }

    private void PlayFx(AudioClip fx) {
      if (_camera == null || !_soundManager.FxEnabled || fx == null) {
        return;
      }

      AudioSource.PlayClipAtPoint(fx, _camera.transform.position, _soundManager.FxVolume);
    }

    private void ProcessInput() {
      if (_inputTimer > 0) {
        _inputReady = false;
        _inputTimer -= Time.deltaTime;
      } else {
        _inputReady = true;
      }

      if (Input.GetKey(KeyCode.A) && _inputReady || Input.GetKeyDown(KeyCode.A)) {
        _activePiece.MoveLeft();
        _inputTimer = InputDelay;

        if (!_board.IsValidPosition(_activePiece)) {
          _activePiece.MoveRight();
          PlayFx(_soundManager.ErrorSound);
        } else {
          PlayFx(_soundManager.MoveSound);
        }
      } else if (Input.GetKey(KeyCode.D) && _inputReady || Input.GetKeyDown(KeyCode.D)) {
        _activePiece.MoveRight();
        _inputTimer = InputDelay;

        if (!_board.IsValidPosition(_activePiece)) {
          _activePiece.MoveLeft();
          PlayFx(_soundManager.ErrorSound);
        } else {
          PlayFx(_soundManager.MoveSound);
        }
      }

      if (Input.GetKey(KeyCode.W) && _inputReady || Input.GetKeyDown(KeyCode.W)) {
        _activePiece.RotateClockwise(_rotateClockwise);
        _inputTimer = InputDelay;

        if (!_board.IsValidPosition(_activePiece)) {
          _activePiece.RotateClockwise(!_rotateClockwise);
        }
      }

      if (Input.GetKeyDown(KeyCode.Space)) {
        Hold();
      }

      if (Input.GetKey(KeyCode.S)) {
        if (_downInputTimer <= 0) {
          _activePiece.MoveDown();

          if (!_board.IsValidPosition(_activePiece)) {
            _activePiece.MoveUp();
          }

          _downInputTimer = DownInputDelay;
        }

        _downInputTimer -= Time.deltaTime;
      }
    }

    private void Start() {
      if (_spawner == null) {
        Debug.LogWarning("Invalid Spawner");
        return;
      }

      if (_activePiece == null) {
        _activePiece = _spawner.SpawnPiece();
      }

      _camera = Camera.main;
      _dropSpeed = InitialDropSpeed;

      if (_screenFader != null) {
        _screenFader.SetActive(true);
      }
    }

    private void Update() {
      if (_board == null || _spawner == null || _gameOver || _soundManager == null || _scoreManager == null || _holder == null) {
        return;
      }

      ProcessInput();

      if (_dropTimer > 0) {
        _dropTimer -= Time.deltaTime;
        return;
      }

      _dropTimer = _dropSpeed;

      if (!_activePiece) {
        return;
      }

      DropPiece();
    }

  }

}