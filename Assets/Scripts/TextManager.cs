using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Text manager.
/// </summary>
public class TextManager : MonoBehaviour
{

	[SerializeField] GameObject m_textPrefab;

	[SerializeField] GameObject m_sphereObject;

	[SerializeField] string[]	m_messages;

	[SerializeField] float m_textCreateInterval = 2f;

	List<GameObject>	m_objsPool;

	System.Action m_task;

	float m_time;

	//
	[SerializeField] List<string> data = new List<string> ();

	//
	DateTime LastTextSendTime = DateTime.Parse ("1992/2/16 12:15:12");
	//DateTime.Today;

	void Awake ()
	{
		m_objsPool = new List<GameObject> ();
	}


	// Use this for initialization
	void Start ()
	{
		m_time = 0f;
		m_task = CreateTask;
	}


	// Update is called once per frame
	void Update ()
	{
		if (m_task != null) {
			m_task ();
		}
	}


	/// <summary>
	/// Adds the text.
	/// </summary>
	void AddText (string text)
	{
		var textObj = Instantiate (m_textPrefab) as GameObject;
		textObj.GetComponent<TextMesh> ().text = text;
		textObj.GetComponent<TextMove> ().Init (m_sphereObject);
		m_objsPool.Add (textObj);
		if (m_objsPool.Count () > 20) {
			m_objsPool.RemoveAt (0);
		}
	}


	void CreateTask ()
	{
		if (m_time > m_textCreateInterval) {

			// サーバーから最新取得
			GetText ();

			if (data.Count > 0) {
				//int index = m_objsPool.Count() % m_messages.Length;
				int index = m_objsPool.Count () % data.Count;
				//AddText(m_messages[index]);
				AddText (data [index]);
				//data.RemoveAt (index)dd;
				data.Clear();
			} 
			m_time = 0;
		}
		m_time += Time.deltaTime;
	}

	void GetText ()
	{
		/*
		//QueryTestを検索するクラスを作成
		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Messages");
		// 最新の一件だけ取得
		query.Limit = 1;
		query.OrderByDescending ("updateDate");
		//検索    
		query.FindAsync ((List<NCMBObject> objList, NCMBException e) => {
			if (e != null) {
				//検索失敗時の処理
			} else {
				//オブジェクトを出力
				foreach (NCMBObject obj in objList) {
					// 更新日時を比較
					if (LastTextSendTime.CompareTo (Convert.ToDateTime (obj.UpdateDate)) < 0) {
						// 若ければ取得
						data.Add (System.Convert.ToString (obj ["text"]));
						// 更新
						LastTextSendTime = Convert.ToDateTime (obj.UpdateDate);
					}
				}
			}
		});
		*/
	}
}
