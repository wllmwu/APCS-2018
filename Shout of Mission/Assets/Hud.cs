using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

	public Text ammoText;
  public Text reloading;

  void Start() {
    SetReloading(false);
    DisplayHud();
  }

  public void DisplayHud() {
    gameObject.transform.GetChild(0).gameObject.SetActive(true);
    gameObject.transform.GetChild(1).gameObject.SetActive(false);
  }

  public void UpdateAmmoText(int clip, int remaining) {
    ammoText.text = "" + clip + " / " + remaining;
  }

  public void ClearAmmoText() {
    ammoText.text = "";
  }

  public void SetReloading(bool visible) {
    reloading.gameObject.SetActive(visible);
  }

  public void DisplayGameOverScreen() {
    gameObject.transform.GetChild(0).gameObject.SetActive(false);
    gameObject.transform.GetChild(1).gameObject.SetActive(true);
  }

}
