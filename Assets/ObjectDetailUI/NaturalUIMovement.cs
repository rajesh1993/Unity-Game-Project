using UnityEngine;
using System.Collections;

public class NaturalUIMovement : MonoBehaviour {

	private RectTransform t;
	private bool isOpen = false;
	private float step = 0;
	private bool isClose = false;

	void Start(){
		t = GetComponent<RectTransform>();
		t.sizeDelta = new Vector2(10,0);
	}

	void Update(){
		if (!isOpen){
			Vector2 curSize = t.sizeDelta;
			curSize = new Vector2(curSize.x, curSize.y + step);
			step += 0.05f;
			t.sizeDelta = curSize;
			if (t.sizeDelta.y >= 8){
				isOpen = true;
				step = 0;
			}
		}

		if (isClose){
			Vector2 curSize = t.sizeDelta;
			curSize = new Vector2(curSize.x, curSize.y - step);
			step += 0.05f;
			t.sizeDelta = curSize;
			if (t.sizeDelta.y <= 0){
				Destroy(gameObject);
			}
		}
	}

	public void close(){
		isClose = true;
	}
}
