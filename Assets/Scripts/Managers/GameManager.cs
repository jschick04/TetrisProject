using Core;
using UnityEngine;

namespace Managers {

  public class GameManager : MonoBehaviour {

    private Piece _activePiece;
    [SerializeField] private Board _board;
    private float _dropSpeed = 0.5f;
    private float _dropTimer;
    [SerializeField] private Spawner _spawner;

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

      if (_dropTimer > 0) {
        _dropTimer -= Time.deltaTime;
        return;
      }

      _dropTimer = _dropSpeed;

      if (_activePiece) {
        _activePiece.MoveDown();

        if (!_board.IsValidPosition(_activePiece)) {
          _activePiece.MoveUp();
          _board.StorePieceInGrid(_activePiece);

          _activePiece = _spawner.SpawnPiece();
        }
      }
    }

  }

}