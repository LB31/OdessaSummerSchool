using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardController : MonoBehaviour
{

    public string textPostContent = "";
    private TouchScreenKeyboard manualKeyboard;
    public Text textField;
    public bool textPostButton;
    public GameObject Panel;
    public TextManager textManager = new TextManager();

    public string getJsonGraffiti()
    {
        string jsonObject = JsonUtility.ToJson(textManager);
        return jsonObject;
    }

    private void Update()
    {
        //! Resizes the colour box around the text according to the text's size
        float width = textField.gameObject.GetComponent<RectTransform>().rect.width;
        float height = textField.gameObject.GetComponent<RectTransform>().rect.height;
        Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }


    public void AddContent() {
        MetadataContainer.metadataContainer.AddText(textManager.Post);
    }

    // Opens native keyboard
    void OnGUI()
    {


        if (textPostButton)
        {
            manualKeyboard = TouchScreenKeyboard.Open(textPostContent);
            TouchScreenKeyboard.hideInput = true;
            textField.text = textPostContent;


        if (manualKeyboard.active)
        {
            textField.text = textPostContent;
        }

        if (manualKeyboard != null)
        {
            textPostContent = manualKeyboard.text;
        }

            if (manualKeyboard.done)
            {
                textPostButton = false;
                textManager.Post = textField.text;
                textPostContent = "";
            }

        }
    }
}

[System.Serializable]
public class TextManager
{
    public string PostType = "Text";
    public string Post = "";
}