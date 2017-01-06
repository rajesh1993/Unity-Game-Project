
using UnityEngine;

public class NoClip : MonoBehaviour {
	
	GameObject g;
	public bool noClipOn = false;
	
	public void EnableNoClip(string gameObjName) {
		g = GameObject.Find(gameObjName);
		
		if(noClipOn == false && Input.GetKeyDown(KeyCode.N)) {
			noClipOn = !noClipOn;
			g.GetComponent<Collider>().enabled = !noClipOn;
			g.GetComponent<Renderer>().enabled = !noClipOn;
		}
		else {
			if(noClipOn == true && Input.GetKeyDown(KeyCode.N)) {
				noClipOn = !noClipOn;
				g.GetComponent<Collider>().enabled = !g.GetComponent<Collider>().enabled;
				g.GetComponent<Renderer>().enabled = !g.GetComponent<Renderer>().enabled;
			}				
		}
		
	}

}