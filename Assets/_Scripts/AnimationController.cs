using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    public GameObject[] allAnimations;
    public GameObject animationsPrefab;

    public string lastAnimation = "";

    public GameObject currentAnimation = null;

    public bool movingAllowed = true;
    // Use this for initialization
    void Start() {


    }

    public void ToggleMoving(bool allowMove) {
        movingAllowed = allowMove;
    }


    public void CreateAnimation(int number) {
        currentAnimation = Instantiate(allAnimations[number], transform);
        lastAnimation = allAnimations[number].name;
    }

    public void AddContent() {
        MetadataContainer.metadataContainer.AddAnimation(lastAnimation, currentAnimation.transform);
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
                    currentAnimation.transform.position = hit.point;
                }
            }
#elif UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                // get the touch position from the screen touch to world point
                Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                // lerp and set the position of the current object to that of the touch, but smoothly over time.
                
                //if(touchedPos.y > (Screen.height / 4))
                currentAnimation.transform.position = touchedPos;
            }
        }
#endif

        }
    }



}
