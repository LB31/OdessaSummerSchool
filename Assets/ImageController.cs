using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageController : MonoBehaviour {
    public Sprite[] allImages;
    public GameObject imagesPrefab;

    public int lastImage;

    public GameObject currentImage = null;

    private bool movingAllowed = true;
	// Use this for initialization
	void Start () {
       

    }

    public void StopMoving() {
        movingAllowed = false;
    }

    public void AllowMoving() {
        movingAllowed = true;
    }

    public void CreateImage(int number) {
        currentImage = Instantiate(imagesPrefab, transform);
        currentImage.GetComponent<Renderer>().material.SetTexture("_MainTex", MainAppManager.mainAppManager.allImages[number].texture);
        lastImage = number;
        //currentImage.transform.rotation = new Quaternion(0, -90, 0, 0);
    }

    public void AddContent() {
        MetadataContainer.metadataContainer.AddImage(lastImage, currentImage.transform.position);
    }

    // Update is called once per frame
    void Update() {

        if (movingAllowed) {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0)) {
                Vector3 mouse = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) {
                    currentImage.transform.position = hit.point;
                }
            }
#elif UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                // get the touch position from the screen touch to world point
                Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1));
                // lerp and set the position of the current object to that of the touch, but smoothly over time.
                
                //if(touchedPos.y > (Screen.height / 4))
                currentImage.transform.position = touchedPos;
            }
        }
#endif

        }
    }



}
