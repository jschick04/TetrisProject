using UnityEngine;

namespace Utility {

  public static class Vectorf {

    public static Vector2 Round(Vector2 vector) =>
      new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));

    public static Vector3 Round(Vector3 vector) =>
      new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));

  }

}