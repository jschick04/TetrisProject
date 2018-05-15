using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Managers {

  public class ScoreManager : MonoBehaviour {

    private const int MaxLines = 4;
    private const int MinLines = 1;

    private int _level = 1;
    [SerializeField] private bool _leveledUp;
    [SerializeField] private Text _levelText;
    private int _lines;
    [SerializeField] private int _linesPerLevel = 5;
    [SerializeField] private Text _linesText;
    private int _score;
    [SerializeField] private Text _scoreText;

    public int Level => _level;

    public bool LeveledUp => _leveledUp;

    public void Reset() {
      _level = 1;
      _lines = _linesPerLevel * _level;

      UpdateUiText();
    }

    public void ScoreLines(int clearedLines) {
      _leveledUp = false;

      clearedLines = Mathf.Clamp(clearedLines, MinLines, MaxLines);

      switch (clearedLines) {
        case 1 :
          _score += 40 * _level;
          break;
        case 2 :
          _score += 100 * _level;
          break;
        case 3 :
          _score += 300 * _level;
          break;
        case 4 :
          _score += 1200 * _level;
          break;
      }

      _lines -= clearedLines;

      if (_lines <= 0) {
        LevelUp();
      }

      UpdateUiText();
    }

    private string ConvertScore(int score, int totalLength) {
      var newScore = new StringBuilder(score.ToString());

      while (newScore.Length < totalLength) {
        newScore.Insert(0, 0);
      }

      return newScore.ToString();
    }

    private void LevelUp() {
      _level++;
      _lines = _linesPerLevel * _level;
      _leveledUp = true;
    }

    private void Start() {
      Reset();
    }

    private void UpdateUiText() {
      if (_linesText) {
        _linesText.text = _lines.ToString();
      }

      if (_levelText) {
        _levelText.text = _level.ToString();
      }

      if (_scoreText) {
        _scoreText.text = ConvertScore(_score, 5);
      }
    }

  }

}