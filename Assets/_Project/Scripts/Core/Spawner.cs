using UnityEngine;

namespace Core {

  public class Spawner : MonoBehaviour {

    private const float QueueScale = 0.5f;

    [SerializeField] private Piece[] _pieces;
    private Piece[] _queuedPieces = new Piece[3];
    [SerializeField] private Transform[] _queuedTransforms = new Transform[3];

    public Piece SpawnPiece() {
      //Piece piece = Instantiate(GetRandomPiece(), transform.position, Quaternion.identity, gameObject.transform);
      Piece piece = GetQueuedPiece();
      piece.transform.position = transform.position;
      piece.transform.localScale = Vector3.one;

      if (piece) {
        return piece;
      }

      Debug.LogWarning("Invalid piece in spawner");
      return null;
    }

    private void FillQueue() {
      for (int i = 0; i < _queuedPieces.Length; i++) {
        if (_queuedPieces[i] == null) {
          _queuedPieces[i] = Instantiate(GetRandomPiece(), transform.position, Quaternion.identity);
          _queuedPieces[i].transform.position = _queuedTransforms[i].position + _queuedPieces[i].QueueOffset;
          _queuedPieces[i].transform.localScale = new Vector3(QueueScale, QueueScale, QueueScale);
        }
      }
    }

    private Piece GetQueuedPiece() {
      Piece firstPiece = null;

      if (_queuedPieces[0]) {
        firstPiece = _queuedPieces[0];
      }

      for (int i = 1; i < _queuedPieces.Length; i++) {
        _queuedPieces[i - 1] = _queuedPieces[i];
        _queuedPieces[i - 1].transform.position = _queuedTransforms[i - 1].position + _queuedPieces[i].QueueOffset;
      }

      _queuedPieces[_queuedPieces.Length - 1] = null;

      FillQueue();

      return firstPiece;
    }

    private Piece GetRandomPiece() {
      int i = Random.Range(0, _pieces.Length);

      if (_pieces[i]) {
        return _pieces[i];
      }

      Debug.LogWarning("Invalid Piece");
      return null;
    }

    private void InitializeQueue() {
      for (int i = 0; i < _queuedPieces.Length; i++) {
        _queuedPieces[i] = null;
      }

      FillQueue();
    }

    private void Start() {
      InitializeQueue();
    }

  }

}