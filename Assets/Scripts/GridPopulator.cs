using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridPopulator : MonoBehaviour {
    public GameObject prefab;
    public int times;
    public Sprite[] selection;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < selection.Length; i++) {
            GameObject current = Instantiate(prefab, transform);
            RectTransform rt = current.GetComponent<RectTransform>();
            current.GetComponent<Image>().sprite = selection[i];
            rt.sizeDelta = new Vector2(selection[i].bounds.size.x, selection[i].bounds.size.y);

            
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
