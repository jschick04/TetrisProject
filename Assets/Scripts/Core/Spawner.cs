using UnityEngine;

namespace Core {

  public class Spawner : MonoBehaviour {

    [SerializeField] private Piece[] _pieces;

    public Piece SpawnPiece() {
      Piece piece = Instantiate(GetRandomPiece(), transform.position, Quaternion.identity, gameObject.transform);

      if (piece) {
        return piece;
      }

      Debug.LogWarning("Invalid piece in spawner");
      return null;
    }

    private Piece GetRandomPiece() {
      int i = Random.Range(0, _pieces.Length);

      if (_pieces[i]) {
        return _pieces[i];
      }

      Debug.LogWarning("Invalid Piece");
      return null;
    }

  }

}