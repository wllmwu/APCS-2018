
using UnityEngine;

public class WeaponCollection : MonoBehaviour {

	private int selectedWeapon = 0;
	void Start () {
		SelectWeapon();
	}
	
	// Update is called once per frame
	void Update () {
		int previousWeapon = selectedWeapon;
		if (Input.GetAxis("Mouse ScrollWheel") > 0f){
			if (selectedWeapon >= transform.childCount - 1){
				selectedWeapon = 0;
			}
			else {
				selectedWeapon ++;
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0f){
			if (selectedWeapon <= 0){
				selectedWeapon = transform.childCount - 1;
			}
			else {
				selectedWeapon --;
			}
		}
		if (previousWeapon != selectedWeapon){
		SelectWeapon();

		}
	}

	void SelectWeapon(){
				int i = 0;
    Gun selected = null;
		foreach(Transform weapon in transform){
			if (i == selectedWeapon){
				weapon.gameObject.SetActive(true);
        selected = (Gun)weapon.gameObject.GetComponent<Gun>();
			}
			else{
				weapon.gameObject.SetActive(false);
			}
			i ++;
		}
    if (selected != null) {
      GameObject.Find("Canvas").GetComponent<Hud>().UpdateAmmoText(selected.clipBullets, selected.remainingBullets);
    }
    else {
      GameObject.Find("Canvas").GetComponent<Hud>().ClearAmmoText();
    }
	}
}
