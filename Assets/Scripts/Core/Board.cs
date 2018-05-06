using UnityEngine;
using Utility;

namespace Core {

  public class Board : MonoBehaviour {

    [SerializeField] private Transform _emptySprite;
    private Transform[,] _grid;
    [SerializeField] private int _header = 8;
    [SerializeField] private int _height = 30;
    [SerializeField] private int _width = 10;

    public void ClearAllRows() {
      for (int y = 0; y < _height; y++) {
        if (IsComplete(y)) {
          ClearRow(y);
          MoveAllRowsDown(y + 1);
          y--;
        }
      }
    }

    public bool IsAboveThreshold(Piece piece) {
      foreach (Transform child in piece.transform) {
        if (child.transform.position.y >= _height - _header - 1) {
          return true;
        }
      }

      return false;
    }

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

    private void ClearRow(int y) {
      for (int x = 0; x < _width; x++) {
        if (_grid[x, y] != null) {
          Destroy(_grid[x, y].gameObject);
          _grid[x, y] = null;
        }
      }
    }

    private void DrawEmptyCells() {
      if (_emptySprite != null) {
        for (int y = 0; y < _height - _header; y++) {
          for (int x = 0; x < _width; x++) {
            Transform clone = Instantiate(_emptySprite, new Vector3(x, y, 0), Quaternion.identity);
            clone.name = $"Board Space ({x}, {y})";
            clone.transform.parent = transform;
          }
        }
      } else {
        Debug.LogWarning("Empty Sprite is missing");
      }
    }

    private bool IsComplete(int y) {
      for (int x = 0; x < _width; x++) {
        if (_grid[x, y] == null) {
          return false;
        }
      }

      return true;
    }

    private bool IsOccupied(int x, int y, Piece piece) =>
      _grid[x, y] != null && _grid[x, y].parent != piece.transform;

    private bool IsWithinBoard(int x, int y) => x >= 0 && x < _width && y >= 0;

    private void MoveAllRowsDown(int baseY) {
      for (int y = baseY; y < _height; y++) {
        MoveRowDown(y);
      }
    }

    private void MoveRowDown(int y) {
      for (int x = 0; x < _width; x++) {
        if (_grid[x, y] != null) {
          _grid[x, y - 1] = _grid[x, y];
          _grid[x, y] = null;
          _grid[x, y - 1].position += new Vector3(0, -1, 0);
        }
      }
    }

    private void Start() {
      DrawEmptyCells();
    }

  }

}