using UnityEngine;
using UnityEngine.UI;

namespace Utility {

  [RequireComponent(typeof(Image))]
  public class IconToggle : MonoBehaviour {

    private bool _defaultIconState = true;
    [SerializeField] private Sprite _iconDisabled;
    [SerializeField] private Sprite _iconEnabled;
    private Image _image;

    public void ToggleIcon() {
      if (!_image || !_iconEnabled || !_iconDisabled) {
        Debug.LogWarning("Missing Icons");
        return;
      }

      _defaultIconState = !_defaultIconState;
      _image.sprite = _defaultIconState ? _iconEnabled : _iconDisabled;
    }

    private void Start() {
      _image = GetComponent<Image>();
      _image.sprite = _defaultIconState ? _iconEnabled : _iconDisabled;
    }

  }

}