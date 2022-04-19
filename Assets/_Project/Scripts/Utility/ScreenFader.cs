using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utility {

  [RequireComponent(typeof(MaskableGraphic))]
  public class ScreenFader : MonoBehaviour {

    private float _currentAlpha;
    [SerializeField] private float _delay;
    private MaskableGraphic _graphic;
    private float _incrament;
    private Color _originalColor;
    [SerializeField] private float _startAlpha = 1f;
    [SerializeField] private float _targetAlpha;
    [SerializeField] private float _timeToFade = 1f;

    private IEnumerator FadeRoutine() {
      yield return new WaitForSeconds(_delay);

      while (Mathf.Abs(_targetAlpha - _currentAlpha) > 0.01f) {
        yield return new WaitForEndOfFrame();
        _currentAlpha = _currentAlpha + _incrament;
        var tempColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);
        _graphic.color = tempColor;
      }

      gameObject.SetActive(false);
    }

    private void Start() {
      _graphic = GetComponent<MaskableGraphic>();

      if (_graphic == null) {
        Debug.LogWarning("Missing MaskableGraphic");
        return;
      }

      _originalColor = _graphic.color;
      _currentAlpha = _startAlpha;
      _incrament = (_targetAlpha - _startAlpha) / _timeToFade * Time.deltaTime;

      StartCoroutine(nameof(FadeRoutine));
    }

  }

}