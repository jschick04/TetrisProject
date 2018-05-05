using UnityEngine;
using Utility;

namespace Core {

  public class Board : MonoBehaviour {

    [SerializeField] private Transform _emptySprite;
    private Transform[,] _grid;
    [SerializeField] private int _header = 8;
    [SerializeField] private int _height = 30;
    [SerializeField] private int _width = 10;

    public bool IsValidPosition(Piece piece) {
      foreach (Transform child in piece.transform) {
        Vector2 position = Vectorf.Round(child.position);

        if (!IsWithinBoard((int)position.x, (int)position.y)) {
          return false;
        }

        if (IsOccupied((int)position.x, (int)position.y, piece)) {
          return false;
        }
      }

      return true;
    }

    public void StorePieceInGrid(Piece piece) {
      if (piece == null) {
        return;
      }

      foreach (Transform child in piece.transform) {
        Vector2 position = Vectorf.Round(child.position);
        _grid[(int)position.x, (int)position.y] = child;
      }
    }

    private void Awake() {
      _grid = new Transform[_width, _height];
    }

    private void DrawEmptyCells() {
      if (_emptySprite != null) {
        for (int y = 0; y < _height - _header; y++) {
          for (int x = 0; x < _width; x++) {
            Transform clone = Instantiate(_emptySprite, new Vector3(x, y, 0), Quaternion.identity);
            clone.name = $"Board Space (x = {x}, y = {y})";
            clone.transform.parent = transform;
          }
        }
      } else {
        Debug.LogWarning("Empty Sprite is missing");
      }
    }

    private bool IsOccupied(int x, int y, Piece piece) => _grid[x, y] != null && _grid[x, y].parent != piece.transform;

    private bool IsWithinBoard(int x, int y) => x >= 0 && x < _width && y >= 0;

    private void Start() {
      DrawEmptyCells();
    }

  }

}