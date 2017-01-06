/// Movement Lock - Disables player movement, toggled with LeftControl.  Attach it to any object, preferably the player controller.

using UnityEngine;
using System.Collections;

public class CharacterLock : MonoBehaviour {

	//public CharacterMotor toDisable;
	public bool disable;

	void Start() {
		disable = false;
		//toDisable = GetComponent<CharacterMotor>();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.LeftControl)) {
			ToggleOnOff();
		}

	}

	void ToggleOnOff()
    {
		disable = !disable;
        //toDisable.enabled = !toDisable.enabled;
		if (disable) {
			if (GetComponent<Rigidbody>() == null)
				gameObject.AddComponent<Rigidbody>();
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
		}
		else {
			if (GetComponent<Rigidbody>() != null)
				Destroy(GetComponent<Rigidbody>());
		}
    }
}