using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAppManager : MonoBehaviour {

    public static MainAppManager mainAppManager;

    public MultiTargetARHandler ARHandler;
    public ScreenshotManager ScreenshotManager;
    public GameObject MainUIElements;
    public GameObject PostUIElements;
    public GameObject CancelUI;
    public CloudUploading TargetUploader;
    public Transform SelectMenue;

    public Sprite[] allImages;
    public GameObject imagesPrefab;

    public GameObject imageSelector;
    public int selectedImageNumber;

    public ImageController imageController;
    public AnimationController animationController;

    public delegate void ControllerDelegate();
    ControllerDelegate controllerDelegate;


    //! Describes, if the user is drawing or writing
    private bool draw;

    // Post types: animation, images, video, audio, draw, text
    private string type;

    private GameObject target = null;

    //! Makes sure that there is only one instance of the static class object
    private void Awake () {
		if(mainAppManager == null)
        {
            
            mainAppManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}


    /// <summary>
    /// Is called when the user triggers a button to create a post
    /// </summary>
    /// <param name="draw">True when user wants to draw; false for a text post</param>
    public void PushCreateButton()
    {
        if (!MultiTargetEventHandler.ObjectDetected && !PostReconstructor.ObjectDetected)
        {
            string targetName = ARHandler.BuildNewTarget();
            if (targetName == null) return;

            //! Make a screenshot for the marker to upload
            ScreenshotManager.TakeAShot();

            StartCoroutine(FindTarget(targetName));


        }


    }

    /// <summary>
    /// Finds the GameObject of the new created target for further computing
    /// </summary>
    /// <param name="targetName">The name of the created object in the PushCreateButton method</param>
    /// <returns></returns>
    private IEnumerator FindTarget(string targetName)
    {

        while (target == null)
        {
            target = GameObject.Find(targetName);
            Debug.Log("searching " + targetName);
            yield return null;
        }

        ProcessPostRequest();
    }

    private void ProcessPostRequest()
    {
        Debug.Log("ProcessPostRequest()");
        MainUIElements.SetActive(false);
        PostUIElements.SetActive(true);

        MoveSelectMenue();

        //if (draw)
        //{
        //    target.GetComponentInChildren(typeof(Drawer), true).gameObject.SetActive(true);
        //}
        //else if (!draw)
        //{
        //    GameObject textController = target.GetComponentInChildren(typeof(Texter), true).gameObject;
        //    textController.SetActive(true);
        //    //! bool textPostButton allows to open the keyboard
        //    textController.GetComponent<KeyBoardController>().textPostButton = true;

        //}
        
    }

    public void MoveSelectMenue() {
        SelectMenue.GetComponent<Animator>().enabled = true;
    }

    public void PushTextButton() {
        if (controllerDelegate != null)
            controllerDelegate();
        controllerDelegate = null;
        GameObject textController = target.GetComponentInChildren(typeof(Texter), true).gameObject;
        textController.SetActive(true);
        //! bool textPostButton allows to open the keyboard
        textController.GetComponent<KeyBoardController>().textPostButton = true;
        ToggleDrawing(false);
        StopTransformChange(true);
        controllerDelegate += Write;
    }

    public void PushImageButton() {
        if(controllerDelegate != null)
        controllerDelegate();
        controllerDelegate = null;

        imageSelector.GetComponent<Animator>().enabled = true;


      
    }

    public void SetImageNumber(int number) {
        selectedImageNumber = number;


        imageSelector.SetActive(false);

        // Continue PushImageButton
        imageController = target.GetComponentInChildren<ImageController>();
        imageController.enabled = true;
        imageController.CreateImage(selectedImageNumber);
        ToggleDrawing(false);
        StopTransformChange(false);
        controllerDelegate += Image;
    }

    public void PushAnimationButton() {
        if (controllerDelegate != null)
            controllerDelegate();
        controllerDelegate = null;
        animationController = target.GetComponentInChildren<AnimationController>();
        animationController.enabled = true;
        animationController.CreateAnimation(0);
        ToggleDrawing(false);
        //StopTransformChange(false);
        controllerDelegate += Animation;
    }

    public void PushDrawButton() {
        if (controllerDelegate != null)
            controllerDelegate();
        controllerDelegate = null;
        target.GetComponentInChildren(typeof(Drawer), true).gameObject.SetActive(true);
        ToggleDrawing(true);
        StopTransformChange(true);
        controllerDelegate += Draw;
    }

    
    // Have to be called, each time another button is pushed
    public void ToggleDrawing(bool activate) {
        if (target.GetComponentInChildren<SwipeTrail>() != null) {
            target.GetComponentInChildren<SwipeTrail>().enabled = activate;
            target.transform.Find("Drawing/ColorPickerCanvas").gameObject.SetActive(activate);
        }

    }

    public void StopTransformChange(bool stop) {
        ImageController ic = target.GetComponentInChildren<ImageController>();
        if (stop) {
            ic.StopMoving();
        } else {
            ic.AllowMoving();
        }

    }


    public void Image() {
        imageController.AddContent();
    }
    public void Animation() {
        animationController.AddContent();
    }
    public void Audio() {

    }
    public void Video() {

    }
    public void Draw() {
        target.GetComponentInChildren<SwipeTrail>().AddContent();
    }

    public void Write() {
        target.GetComponentInChildren<KeyBoardController>().AddContent();
    }

    public void PushPostButton()
    {
        
        controllerDelegate();
        Texture2D takenScreenshot = ScreenshotManager.GetScreenshotImage();
            TargetUploader.texture = takenScreenshot;

            MainUIElements.SetActive(true);
            PostUIElements.SetActive(false);

        //    if (draw)
        //    {
        //        //! Deactivate SwipeTrail to stop user drawing on screen
        //        target.GetComponentInChildren<SwipeTrail>().enabled = false;
        //        target.GetComponentInChildren<ColorPicker>().gameObject.SetActive(false);
        //        TargetUploader.metadataStr = target.GetComponentInChildren<SwipeTrail>().getJsonGraffiti();
        //    }
        //    else if (!draw)
        //    {
        //        TargetUploader.metadataStr = target.GetComponentInChildren<KeyBoardController>().getJsonGraffiti();
        //        target.GetComponentInChildren<KeyBoardController>().enabled = false;
        //}

            TargetUploader.metadataStr = MetadataContainer.metadataContainer.GetJSON();
        print(TargetUploader.metadataStr);
            TargetUploader.CallPostTarget();

        ToggleDrawing(false);
        ScreenshotManager.DeleteLastScreenshot();
            target = null;
        

    }


    public void PushCancelButton()
    {
        CancelUI.SetActive(true);
    }

    public void PushKeepButton()
    {
            CancelUI.SetActive(false);
    }

    public void PushDiscardButton()
    {
        CancelUI.SetActive(false);
        MainUIElements.SetActive(true);
        PostUIElements.SetActive(false);
        ARHandler.DestroyLastTrackable();
        Destroy(target);
        target = null;
        ScreenshotManager.DeleteLastScreenshot();
    }

}
