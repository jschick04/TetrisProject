using UnityEngine;

namespace Core {

  public class Holder : MonoBehaviour {

    private const float Scale = 0.5f;

    public bool canRelease;

    [SerializeField] private Transform _holderTransform;

    public Piece HeldPiece { get; private set; }

    public void Catch(Piece piece) {
      if (HeldPiece) {
        Debug.LogWarning("HOLDER: Release a shape before trying to hold");
        return;
      }

      if (!piece) {
        Debug.LogWarning("HOLDER: Invalid piece");
        return;
      }

      if (_holderTransform) {
        piece.transform.position = _holderTransform.position + piece.QueueOffset;
        piece.transform.rotation = new Quaternion(0, 0, 0, 0);
        piece.transform.localScale = new Vector3(Scale, Scale, Scale);
        HeldPiece = piece;
      } else {
        Debug.LogWarning("HOLDER: No transform assigned");
      }
    }

    public Piece Release() {
      HeldPiece.transform.localScale = Vector3.one;
      Piece piece = HeldPiece;

      HeldPiece = null;

      canRelease = false;

      return piece;
    }

  }

}