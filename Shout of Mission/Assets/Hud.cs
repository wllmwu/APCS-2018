using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

	public Text ammoText;

  public void UpdateAmmoText(int clip, int remaining) {
    ammoText.text = "" + clip + " / " + remaining;
  }

  public void ClearAmmoText() {
    ammoText.text = "";
  }

}
