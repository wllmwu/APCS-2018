using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  LevelGenerator levelGen;

  void Awake() {
    levelGen = GetComponent<LevelGenerator>();
    levelGen.GenerateLevel();
  }

}
