using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

  LevelGenerator levelGen;
  public List<MonoBehaviour> playerScripts;

  void Awake() {
    levelGen = GetComponent<LevelGenerator>();
    levelGen.GenerateLevel();
  }

  void StartGame() {
    //
  }

  public void EndGame() {
    foreach (Enemy e in (Enemy[])FindObjectsOfType<Enemy>()) {
      e.Stop();
    }
    foreach (MonoBehaviour s in playerScripts) {
      try {
        s.enabled = false;
      }
      catch {}
    }
    GameObject.Find("Canvas").GetComponent<Hud>().DisplayGameOverScreen();
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
  }

  public void RestartGame() {
    //GameObject.Find("Canvas").GetComponent<Hud>().DisplayHud();
    //StartGame();
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    foreach (MonoBehaviour s in playerScripts) {
      s.enabled = true;
    }
  }

}
