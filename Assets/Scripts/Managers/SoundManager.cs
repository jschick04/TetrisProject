using UnityEngine;
using Utility;

namespace Managers {

  public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioClip[] _backgroundMusicClips;
    [SerializeField] private AudioClip _clearRowSound, _dropSound, _errorSound, _gameOverSound, _gameOverVocal, _moveSound;
    [SerializeField] private bool _fxEnabled = true;
    [SerializeField] private IconToggle _fxIconToggle;
    [SerializeField][Range(0, 1)] private float _fxVolume = 1f;
    [SerializeField] private bool _musicEnabled = true;
    [SerializeField] private IconToggle _musicIconToggle;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField][Range(0, 1)] private float _musicVolume = 1f;
    [SerializeField] private AudioClip[] _vocalClips;

    public AudioClip ClearRowSound => _clearRowSound;

    public AudioClip DropSound => _dropSound;

    public AudioClip ErrorSound => _errorSound;

    public bool FxEnabled => _fxEnabled;

    public float FxVolume => _fxVolume;

    public AudioClip GameOverSound => _gameOverSound;

    public AudioClip GameOverVocal => _gameOverVocal;

    public AudioClip MoveSound => _moveSound;

    public AudioClip GetRandomVocal() => GetRandomClip(_vocalClips);

    public void PlayBackgroundMusic(AudioClip musicClip) {
      if (!_musicEnabled || !musicClip || !_musicSource) {
        return;
      }

      _musicSource.Stop();

      _musicSource.clip = musicClip;
      _musicSource.volume = _musicVolume;
      _musicSource.loop = true;

      _musicSource.Play();
    }

    public void ToggleFx() {
      _fxEnabled = !_fxEnabled;

      if (_fxIconToggle) {
        _fxIconToggle.ToggleIcon();
      }
    }

    public void ToggleMusic() {
      _musicEnabled = !_musicEnabled;
      UpdateMusic();

      if (_musicIconToggle) {
        _musicIconToggle.ToggleIcon();
      }
    }

    private static AudioClip GetRandomClip(AudioClip[] clips) => clips[Random.Range(0, clips.Length)];

    private void Start() {
      PlayBackgroundMusic(GetRandomClip(_backgroundMusicClips));
    }

    private void UpdateMusic() {
      if (_musicSource.isPlaying == _musicEnabled) {
        return;
      }

      if (_musicEnabled) {
        PlayBackgroundMusic(GetRandomClip(_backgroundMusicClips));
      } else {
        _musicSource.Stop();
      }
    }

  }

}