using Core;
using UnityEngine;

namespace Managers {

  public class GameManager : MonoBehaviour {

    private const float InputDelay = 0.2f;

    private Piece _activePiece;
    [SerializeField] private Board _board;
    private float _dropSpeed = 0.5f;
    private float _dropTimer;
    private float _inputTimer;
    [SerializeField] private Spawner _spawner;

    private void ProcessInput() {
      bool inputReady;

      if (_inputTimer > 0) {
        inputReady = false;
        _inputTimer -= Time.deltaTime;
      } else {
        inputReady = true;
      }

      if (Input.GetKey(KeyCode.A) && inputReady || Input.GetKeyDown(KeyCode.A)) {
        _activePiece.MoveLeft();
        _inputTimer = InputDelay;

        if (!_board.IsValidPosition(_activePiece)) {
          _activePiece.MoveRight();
        }
      } else if (Input.GetKey(KeyCode.D) && inputReady || Input.GetKeyDown(KeyCode.D)) {
        _activePiece.MoveRight();
        _inputTimer = InputDelay;

        if (!_board.IsValidPosition(_activePiece)) {
          _activePiece.MoveLeft();
        }
      } else if (Input.GetKey(KeyCode.Space) && inputReady) {
        _activePiece.RotateRight();
        _inputTimer = InputDelay;

        if (!_board.IsValidPosition(_activePiece)) {
          _activePiece.RotateLeft();
        }
      }

      if (Input.GetKeyDown(KeyCode.S)) {
        _dropSpeed /= 10;
      } else if (Input.GetKeyUp(KeyCode.S)) {
        _dropSpeed *= 10;
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
    }

    private void Update() {
      if (_board == null || _spawner == null) {
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

      _activePiece.MoveDown();

      if (!_board.IsValidPosition(_activePiece)) {
        _activePiece.MoveUp();
        _board.StorePieceInGrid(_activePiece);

        _activePiece = _spawner.SpawnPiece();
        _board.ClearAllRows();
      }
    }

  }

}