﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

  [SerializeField] private Transform _emptySprite;
  [SerializeField] private int _header = 8;
  [SerializeField] private int _height = 30;
  [SerializeField] private int _width = 10;

  public void Start() {
    DrawEmptyCells();
  }

  private void DrawEmptyCells() {
    if (_emptySprite != null) {
      for (int y = 0; y < _height - _header; y++) {
        for (int x = 0; x < _width; x++) {
          Transform clone = Instantiate(_emptySprite, new Vector3(x, y, 0), Quaternion.identity);
          clone.name = $"Board Space (x = {x}, y = {y})";
          clone.transform.parent = transform;
        }
      }
    } else {
      Debug.LogWarning("Empty Sprite is missing");
    }
  }

}