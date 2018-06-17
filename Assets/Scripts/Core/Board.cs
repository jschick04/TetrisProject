using UnityEngine;
using Utility;

namespace Core {

  public class Board : MonoBehaviour {

    [SerializeField] private const int Header = 8;
    [SerializeField] private const int Height = 30;
    [SerializeField] private const int Width = 10;

    private int _completedRows;
    [SerializeField] private Transform _emptySprite;
    private Transform[,] _grid;

    public int CompletedRows => _completedRows;

    public void ClearAllRows() {
      _completedRows = 0;

      for (int y = 0; y < Height; y++) {
        if (IsComplete(y)) {
          _completedRows++;
          ClearRow(y);
          MoveAllRowsDown(y + 1);
          y--;
        }
      }
    }

    public bool IsAboveThreshold(Piece piece) {
      foreach (Transform child in piece.transform) {
        if (child.transform.position.y >= Height - Header - 1) {
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
      _grid = new Transform[Width, Height];
    }

    private void ClearRow(int y) {
      for (int x = 0; x < Width; x++) {
        if (_grid[x, y] != null) {
          Destroy(_grid[x, y].gameObject);
          _grid[x, y] = null;
        }
      }
    }

    private void DrawEmptyCells() {
      if (_emptySprite != null) {
        for (int y = 0; y < Height - Header; y++) {
          for (int x = 0; x < Width; x++) {
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
      for (int x = 0; x < Width; x++) {
        if (_grid[x, y] == null) {
          return false;
        }
      }

      return true;
    }

    private bool IsOccupied(int x, int y, Piece piece) =>
      _grid[x, y] != null && _grid[x, y].parent != piece.transform;

    private bool IsWithinBoard(int x, int y) => x >= 0 && x < Width && y >= 0;

    private void MoveAllRowsDown(int baseY) {
      for (int y = baseY; y < Height; y++) {
        MoveRowDown(y);
      }
    }

    private void MoveRowDown(int y) {
      for (int x = 0; x < Width; x++) {
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