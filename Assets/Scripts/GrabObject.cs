using UnityEngine;
using System.Collections;

public class GrabObject : MonoBehaviour {


	public float RaySize;
	public float Speed = 100f;
	public KeyCode GrabObjectKey;
	//public string ClearSelectionKey;
	public KeyCode IncreaseDistanceKey;
	public KeyCode DecreaseDistanceKey;

	private float AdjustableDistance = 3f;
	private GameObject pickedUpObject;
	private Camera mainCamera;
	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		mainCamera = transform.GetComponentInChildren<Camera> ();
	}
	

	// Update is called once per frame
	void Update () {

		if (pickedUpObject != null) {
			ManipulateObject ();
		}

		if (Input.GetKeyDown(GrabObjectKey)) {
			if(pickedUpObject == null){
				grabobject ();
			}else{
				pickedUpObject = null;
			}
		}

		if (Input.GetKey (IncreaseDistanceKey)) {
			if(AdjustableDistance < 5f)
			AdjustableDistance += 0.01f;
		}

		if (Input.GetKey (DecreaseDistanceKey)) {
			if (AdjustableDistance > 2f)
			AdjustableDistance -= 0.01f;
		}


	}

	void grabobject(){
		RaycastHit hit;
		Ray SelectorRay = mainCamera.ViewportPointToRay (new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast (SelectorRay, out hit, RaySize)) {
			if (hit.collider.gameObject.name != "Terrain") {
				pickedUpObject = hit.collider.gameObject; //we use this to determine whether an object is picked up by the player. If it's not null, then the player is doing so.
				rb = pickedUpObject.GetComponent<Rigidbody>();
				//rb.isKinematic = true;
			}
		}

	}

	void ManipulateObject(){
		if(pickedUpObject != null){
			//pickedUpObject.transform.parent = transform; //attach the object to the camera so it moves along with it.
			float step = Speed * Time.deltaTime;
			Vector3 targetPosition = transform.position + (transform.forward * AdjustableDistance);
			pickedUpObject.transform.position = Vector3.MoveTowards(pickedUpObject.transform.position, targetPosition, step);
			//pickedUpObject.transform.position = transform.position + (transform.forward * AdjustableDistance); //might need changing as it's untested.
		}

	}
}

