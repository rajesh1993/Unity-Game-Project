using UnityEngine;
using System.Collections;

public class setPhysicsRealTime : MonoBehaviour {

	public enum Mode {
		Gravity,
		Friction,
		Bounciness,
		None
	}

    public KeyCode increaseButton = KeyCode.O;
    public KeyCode decreaseButton = KeyCode.P;

    private Mode mode;

	public Vector3 newGravity;

	private Rigidbody targetRb;

	// Use this for initialization
	void Start() {
		print("Toggle between states with F1, F2, and F3. F1 is gravity, F2 is friction, F3 is bounciness");
		mode = Mode.None;
	}

	// Update is called once per frame
	void Update() {
		// Toggle between physics modifying modes
		if (Input.GetKeyDown(KeyCode.F1)) {
			if (mode == Mode.Gravity) {
				mode = Mode.None;
				return;
			}
			print("Modifying gravity, W to decrease, S to increase");
			mode = Mode.Gravity;
		}
		if (Input.GetKeyDown(KeyCode.F2)) {
			if (mode == Mode.Friction) {
				mode = Mode.None;
				return;
			}
			print("Modifying friction, W to increase, S to decrease");
			mode = Mode.Friction;
		}
		if (Input.GetKeyDown(KeyCode.F3)){
			if (mode == Mode.Bounciness) {
				mode = Mode.None;
				return;
			}
			print("Modifying bounciness, W to increase, S to decrease");
			mode = Mode.Bounciness;
		}

		if (mode == Mode.None)
			print("Toggles off");

		if (mode != Mode.None) {
			ModifyTargetPhysics(mode);
		}
	}

	void ModifyTargetPhysics(Mode mode) {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if ((Physics.Raycast(ray, out hit)) == true) {
			// Add physics material to target if it doesn't have one
			if (hit.collider.material == null) {
				hit.collider.material = new PhysicMaterial();
				hit.collider.material.name = hit.collider.name + " material";
			}
			
			if (mode == Mode.Gravity) {
				// Add rigidbody to target if it doesn't have one
				if (hit.collider.GetComponent<Rigidbody>() == null)
					hit.collider.gameObject.AddComponent<Rigidbody>();
				else {
					targetRb = hit.collider.gameObject.GetComponent<Rigidbody>();
					if (Input.GetKey(increaseButton)) {
						// Modifying gravity
							targetRb.useGravity = false;
							targetRb.AddForce(newGravity, ForceMode.Acceleration);
					}
					else if (Input.GetKey(decreaseButton)) {
						targetRb.useGravity = false;
						targetRb.AddForce(-newGravity, ForceMode.Acceleration);
					}
					else {
						targetRb.useGravity = true;
					}
				}
			}

			// Modifying friction in increments of 0.01
			if (mode == Mode.Friction) {
				hit.collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
				if (Input.GetKey(increaseButton) && hit.collider.material.dynamicFriction <= .99f) {
					hit.collider.material.dynamicFriction += .01f;
					hit.collider.material.staticFriction += .01f;
					// Adjusting for floating point imprecision
					hit.collider.material.staticFriction = hit.collider.material.staticFriction > .99f ? 1 : hit.collider.material.staticFriction;
					hit.collider.material.dynamicFriction = hit.collider.material.dynamicFriction > .99f ? 1 : hit.collider.material.dynamicFriction;
					hit.collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
				}
				else if (Input.GetKey(decreaseButton) && hit.collider.material.dynamicFriction >= 0.01f) {
					hit.collider.material.dynamicFriction -= .01f;
					hit.collider.material.staticFriction -= .01f;
					// Adjusting for floating point imprecision
					hit.collider.material.staticFriction = hit.collider.material.staticFriction < .01f ? 0 : hit.collider.material.staticFriction;
					hit.collider.material.dynamicFriction = hit.collider.material.dynamicFriction < .01f ? 0 : hit.collider.material.dynamicFriction;
				}
			}

			// Modifying bounciness in increments of 0.01
			if (mode == Mode.Bounciness) {
				hit.collider.material.bounceCombine = PhysicMaterialCombine.Maximum;
				if (Input.GetKey(increaseButton) && hit.collider.material.bounciness <= .99f) {
					hit.collider.material.bounciness += .01f;
					// Adjusting for floating point imprecision
					hit.collider.material.bounciness = hit.collider.material.bounciness > .99f ? 1 : hit.collider.material.bounciness;
				}
				else if (Input.GetKey(decreaseButton) && hit.collider.material.bounciness >= 0.01f) {
					hit.collider.material.bounciness -= .01f;
					// Adjusting for floating point imprecision
					hit.collider.material.bounciness = hit.collider.material.bounciness > .99f ? 0 : hit.collider.material.bounciness;
				}
			}
		}
	}
}
