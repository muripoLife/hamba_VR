using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// User interface text width.
/// </summary>
public class UITextWidth : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var height = GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().sizeDelta.y;
		GetComponent<Text>().fontSize = (int)(height * 0.1f);
	}
}
