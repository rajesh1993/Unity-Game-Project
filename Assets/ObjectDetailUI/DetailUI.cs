using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DetailUI : MonoBehaviour {

	public Canvas prefab;
	public float range;
	public LayerMask mask;
	public KeyCode activationKey;
	public float maxDistance;

	private bool hasActiveUI = false;
	private Canvas curUI;

	//Stored Details
	private string name;
	private int vertexCount;
	private int fuelCost; //placeholder

	//Not even sure if the following fields are relevant
	private string[] materialList;
	private int[] materialListCost;

	//Delay Between Button Presses
	private bool buttonInactive = false;
	public float buttonDelayTime = 1;
	private float curButtonDelayTime = 0;


	//Mode Changing
	public bool followMode;
	private Vector3 lockedPos = new Vector3();


	//Player
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		if (player == null){
			Debug.Log ("Missing Player");
		}
		if (prefab == null){
			Debug.Log ("Missing Canvas Prefab");
		}
		lockedPos = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		Debug.DrawRay (transform.position,transform.forward * 10f,Color.green,0.01f);
		//Sends out a ray to detect for object
		
		if(Input.GetKeyDown (activationKey) && buttonInactive == false){
			if (followMode && hasActiveUI){
				curUI.GetComponentInChildren<NaturalUIMovement>().close ();
				hasActiveUI = false;
			}
			else if (hasActiveUI){
				curUI.GetComponent<NaturalUIMovement>().close();
				hasActiveUI = false;
				CreateUI();

			}
			else{
				CreateUI();
			}
		}


	}

	void CreateUI(){
		RaycastHit hit;
		if(Physics.Raycast (transform.position,transform.forward, out hit, range,mask)){
			MeshFilter mf = hit.collider.gameObject.GetComponent<MeshFilter>();
			if (mf != null){ //Check to see if object has a meshfilter
				buttonInactive = true;
				hasActiveUI = true;
				Vector3 rot = transform.rotation.eulerAngles;
				lockedPos = player.transform.position;
				curUI = Instantiate(prefab,transform.position + transform.forward * 3, Quaternion.Euler(rot.x + 15f,rot.y,0)) as Canvas;

				Transform[] children = curUI.GetComponentsInChildren<Transform>();
				foreach (Transform child in children){
					if (child.gameObject.name.Equals("ObjectName")){
						child.GetComponent<Text>().text = hit.collider.name;
					}
					else if (child.gameObject.name.Equals ("VertexCount")){
						child.GetComponent<Text>().text = "\n\n\n" + mf.sharedMesh.vertexCount.ToString();
					}
					else if (child.gameObject.name.Equals ("FuelCost")){
						child.GetComponent<Text>().text = "\n\n\n\n\n\n" + (mf.sharedMesh.vertexCount * 0.25f).ToString();
					}
				}
				curUI.worldCamera = transform.GetComponentInChildren<Camera>();
			}
			else{
				hasActiveUI = false;
			}
		}
	}

	void LateUpdate(){
		if (hasActiveUI){
			if (!followMode){ // Destroy the UI Popup when player is <Distance> away from it
				if (Vector3.Distance (transform.position, curUI.transform.position) > maxDistance){
					curUI.GetComponent<NaturalUIMovement>().close ();
					hasActiveUI = false;
				}
			}
			else{
				//Closes UI when player moves from initial position
				if (lockedPos != player.transform.position){ 
					curUI.GetComponent<NaturalUIMovement>().close ();
					Debug.Log ("CLOSE HERE");
					hasActiveUI = false;
				}
				//Track UI around player
				else{
					curUI.transform.position = Vector3.Lerp(curUI.transform.position,transform.position + transform.forward * 2, Time.fixedDeltaTime * 4);
				}
			}
		}

	}

	void FixedUpdate(){

		//Prevents opening menu 20 times a second
		if (buttonInactive){
			curButtonDelayTime += Time.deltaTime;
			if (curButtonDelayTime >= buttonDelayTime){
				buttonInactive = false;
				curButtonDelayTime = 0;
			}
		}
		//Track the rotation of the UI
		if (hasActiveUI){
			curUI.transform.rotation = Quaternion.Lerp(curUI.transform.rotation,transform.rotation,Time.fixedDeltaTime * 5);
		}
	}
}
