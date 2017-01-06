using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectObjects : MonoBehaviour {
	
	//public Camera mainCamera;
	public float RaySize;
	public Material SelectedMaterial;
	public bool ReturnPhysics;
	//public bool Debugging;
	public string SelectObjectsKey;
	public string CombineObjectsKey;
	public string UncombineObjectsKey;
	public string ClearSelection;

	private string RTMBparent = "RuntimeMeshBatcherParent_";
	private List<Material> _materials;
	private Camera mainCamera;
	private List<GameObject> CombinedParents;
	private List<GameObject> _list;
	private GameObject RMBC;
	private bool SelectorRayOn = false;
	// Use this for initialization
	void Start () {
		_materials = new List<Material> ();
		_list = new List<GameObject> ();
		CombinedParents = new List<GameObject> ();
		mainCamera = gameObject.GetComponent<Camera> ();
		//var mainCamera = (Camera) transform.gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (SelectObjectsKey)) {
			Selector ();
		}

		if (Input.GetKeyDown (UncombineObjectsKey)) {
			Uncombine();
		}
		
		if(Input.GetKeyDown(CombineObjectsKey)){
			MeshBatcher();
		}

		if (Input.GetKeyDown (ClearSelection)) {
			_list.Clear ();
			_materials.Clear ();
			Debug.Log("Your selections have been cleared");
		}

		/*
		//Debugging tools
		if (Input.GetKeyDown (KeyCode.I)) {

			foreach (GameObject obj in _list) {
				if(obj == null)
					Debug.Log("Error: Object in _list is NULL!");
				if(obj.transform.parent != null){
					Debug.Log(obj + " and Object's parent: " + obj.transform.parent.name);
					var TestString = "RuntimeMeshBatcherParent_";
					var obj_parent = obj.transform.parent;
					if (obj_parent.name.Contains (TestString)){
						Debug.Log ("YOUR PARENT IS RTMB!!!!!!!");
					}
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			for (int i = 0; i < _list.Count; i++) {
				if (_list [i].name.Contains ("CombinedMesh_")) {
					Debug.Log (i + " has been destroyed!");
					_list.Remove(_list[i]);
					Destroy (_list [i]);
				}
			}
		}


		if(Input.GetKeyDown(KeyCode.Z)){
			Debug.Log ("||||||||||||||||||||||/BEGIN");
			//int i = 0;
			//foreach(GameObject obj in _list){
			//	Debug.Log ("Slot " + i + ": " + obj);
			//	i++;
			//}

			for(int i = 0; i < _list.Count; i++){
				Debug.Log (i + " " + _list[i]);
			}
			Debug.Log ("||||||||||||||||||||||/END");
		}
		*/
		// END OF DEBUGGING TOOLS


	}

	void Selector(){

		RaycastHit hit;
		Ray SelectorRay = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if(Physics.Raycast(SelectorRay, out hit, RaySize)){
			if(SelectedMaterial != null){
				_materials.Add(hit.collider.gameObject.GetComponent<MeshRenderer>().sharedMaterial);
				hit.collider.gameObject.GetComponent<MeshRenderer>().sharedMaterial = SelectedMaterial;
			}
			_list.Add(hit.collider.gameObject);
			//Debug.Log(hit.collider.gameObject + " was added to list");
		}
	}

		void MeshBatcher(){
		//A couple temporary lists needed for swapping and removing objects from larger lists.
		List<GameObject> TempList = new List<GameObject> ();
		List<GameObject> KillList = new List<GameObject> ();

		foreach(GameObject obj in _list){
			if (obj.name.Contains ("CombinedMesh_")) {
				KillList.Add(obj);
			}

			if(obj.transform.parent != null){
				var Parent_Object = obj.transform.parent;
				if (Parent_Object.name.Contains(RTMBparent)){
					//Debug.Log(RTMBparent + "This part works!");
					foreach(Transform child in Parent_Object){
						//Debug.Log ("This is the child object: " + child.gameObject);
						TempList.Add (child.gameObject);
						//_list.Add(child.gameObject);
					}
				}
			}
		}

		foreach (GameObject obj in TempList) {
			_list.Add (obj);
			_materials.Add(obj.GetComponent<MeshRenderer>().sharedMaterial);
		}

		foreach (GameObject obj in KillList) {
			_list.Remove (obj);
			_materials.Remove(obj.GetComponent<MeshRenderer>().sharedMaterial);
			Destroy(obj);
		}

		KillList.Clear ();
		TempList.Clear ();

		//debug
		/*
		if (Debugging) {
			for (int z = 0; z < _list.Count; z++) {
				Debug.Log (z + " " + _list [z]);
			}
		}
		*/
		//

		int i = 0;

		//putting all the game objects to combine into an array that can be passed off to RTMB
		GameObject[] ObjectsArray = new GameObject[_list.Count];
		foreach (GameObject obj in _list) {
			ObjectsArray [i] = obj;
			i++;
		}
		//debug log
			for (int j = 0; j < ObjectsArray.Length; j++) {
				Rigidbody rb = ObjectsArray [j].GetComponent<Rigidbody> ();
				MeshRenderer mr = ObjectsArray [j].GetComponent<MeshRenderer> ();
				Collider col = ObjectsArray [j].GetComponent<Collider> ();
				col.enabled = false;

				//ObjectsArray[j].GetComponent<MeshRenderer>().sharedMaterial = _materials[j];
				ObjectsArray [j].transform.parent = null;
				//Debug.Log ("ArraySlot " + j + ": " + ObjectsArray [j]);
				if(rb != null){
					rb.isKinematic = true;
					Debug.Log ("RB EXISTS!");
				}else {
					Debug.Log("RB DOESN'T EXIST! :(");
				}
			}
		
		//adding the parent object created in RTMB to a list of RTMB parent objects.
		CombinedParents.Add(RuntimeMeshBatcherController.instance.CombineMeshes(ObjectsArray));
		_list.Clear();
		_materials.Clear ();
		/*Here we are simply checking for empty parent gameobjects in the world created by
		 * runtimemeshbatcher by checking one list putting any that exist into a kill list
		 * and we are removing them if there are any.
		 */
		foreach (GameObject obj in CombinedParents) {
			if (obj.transform.childCount == 0 || obj == null) {
				KillList.Add (obj);
			}
		}

		foreach (GameObject obj in KillList) {
			CombinedParents.Remove (obj);
			Destroy (obj);
		}

		KillList.Clear ();
	}

	void Uncombine(){
		GameObject obj = _list [0].transform.parent.gameObject;
		Debug.Log (obj);
		CombinedParents.Remove (obj);
		RuntimeMeshBatcherController.instance.UncombineMeshes (obj, ReturnPhysics);
		_list.Clear ();
	}


}
