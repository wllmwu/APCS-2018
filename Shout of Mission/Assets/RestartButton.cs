using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour {

  public Button button;

	// Use this for initialization
	void Start () {
		button.onClick.AddListener(HandleClick);
	}
	
  void HandleClick() {
    GameObject.Find("GameManager").GetComponent<GameManager>().RestartGame();
  }
	
}
