using UnityEngine;

namespace Core {

  public enum MoveDirection {

    Up,
    Down,
    Left,
    Right

  }

  public class Piece : MonoBehaviour {

    private bool _canRotate = true;
    [SerializeField] private Vector3 _queueOffset;

    public Vector3 QueueOffset => _queueOffset;

    public void Move(MoveDirection moveDirection) {
      Vector3 newPosition;

      switch (moveDirection) {
        case MoveDirection.Up :
          newPosition = new Vector3(0, 1, 0);
          break;
        case MoveDirection.Down :
          newPosition = new Vector3(0, -1, 0);
          break;
        case MoveDirection.Left :
          newPosition = new Vector3(-1, 0, 0);
          break;
        case MoveDirection.Right :
          newPosition = new Vector3(1, 0, 0);
          break;
        default :
          newPosition = Vector3.zero;
          break;
      }

      transform.position += newPosition;
    }

    public void RotateClockwise(bool clockwise) {
      if (clockwise) {
        RotateRight();
      } else {
        RotateLeft();
      }
    }

    public void RotateLeft() {
      if (_canRotate) {
        transform.Rotate(0, 0, 90);
      }
    }

    public void RotateRight() {
      if (_canRotate) {
        transform.Rotate(0, 0, -90);
      }
    }

  }

}