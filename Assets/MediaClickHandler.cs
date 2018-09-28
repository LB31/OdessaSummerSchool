using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaClickHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void buh() {
        Destroy(gameObject);
    }

    void Update() {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0)) {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) {
                print("clicked");
            }
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
 
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                    // get the touch position from the screen touch to world point
                    Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1));
         
                    Ray castPoint = Camera.main.ScreenPointToRay(touchedPos);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) {
                    print("blicked");
                }
            }

        }
#endif
    }
}
