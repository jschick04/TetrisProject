using Core;
using UnityEngine;

namespace Utility {

  public class Ghost : MonoBehaviour {

    private readonly Color _color = new Color(1f, 1f, 1f, 0.2f);
    private Piece _ghostPiece;
    private bool _hitBottom;

    public void DrawGhost(Piece activePiece, Board board) {
      if (!_ghostPiece) {
        _ghostPiece = Instantiate(activePiece, activePiece.transform.position, activePiece.transform.rotation);
        _ghostPiece.gameObject.name = "GhostPiece";

        var allRenders = _ghostPiece.GetComponentsInChildren<SpriteRenderer>();

        foreach (var render in allRenders) {
          render.color = _color;
        }
      } else {
        _ghostPiece.transform.position = activePiece.transform.position;
        _ghostPiece.transform.rotation = activePiece.transform.rotation;
      }

      _hitBottom = false;

      while (!_hitBottom) {
        _ghostPiece.MoveDown();

        if (!board.IsValidPosition(_ghostPiece)) {
          _ghostPiece.MoveUp();
          _hitBottom = true;
        }
      }
    }

    public void Reset() {
      Destroy(_ghostPiece.gameObject);
    }

  }

}