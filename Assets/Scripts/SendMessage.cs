using NCMB;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SendMessage : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		// 新しいメッセージを保存する
		NCMBObject testClass = new NCMBObject("Messages");
		testClass["text"] = "Hello, NCMB!";
		testClass.SaveAsync();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
