using UnityEngine;

namespace Core {

  public class Piece : MonoBehaviour {

    [SerializeField] private bool _canRotate = true;
    [SerializeField] private Vector3 _queueOffset;

    public Vector3 QueueOffset => _queueOffset;

    public void MoveDown() {
      Move(new Vector3(0, -1, 0));
    }

    public void MoveLeft() {
      Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight() {
      Move(new Vector3(1, 0, 0));
    }

    public void MoveUp() {
      Move(new Vector3(0, 1, 0));
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

    private void Move(Vector3 moveDirection) {
      transform.position += moveDirection;
    }

  }

}