using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// User interface manager.
/// </summary>
public class UIManager : SingletonMonoBehaviour<UIManager> {
	
	[System.Serializable]
	public class Msg {
		public MsgType	type;
		public string[]	messages;
	}
		
	public enum MsgType {
		/// <summary>
		/// ３Dテキスト注目を促すメッセージ
		/// </summary>
		FINDMSG,
		/// <summary>
		/// はい、いいえの仕方のメッセージ
		/// </summary>
		YESNO,
		/// <summary>
		/// 告白成功
		/// </summary>
		SUCCESS,
		/// <summary>
		/// 告白失敗
		/// </summary>
		FAILED,
	}

	public string[]	messages;

	[SerializeField] float	m_waitMsgTime = 1.5f;
	[SerializeField] Text	m_text;
	[SerializeField] Msg[]	m_msgs;

	bool	m_taskStartFlag;
	float m_width;
	int		m_msgIndex;
	bool	m_isDisfOff;
	System.Action	m_task;

	void Start(){
		m_width = GetComponent<RectTransform>().sizeDelta.x;
	}

	void Update(){
		if(m_task!=null){
			m_task();
		}
	}

	public void Display(bool onoff){
		m_isDisfOff = !onoff;
		var imgs = this.GetComponentsInChildren<MaskableGraphic>();
		foreach(var one in imgs){
			one.enabled = onoff;
		}
		if(!m_isDisfOff){
			m_task = null;
		}
	}
		
	/// <summary>
	/// Sets the type of the message.
	/// </summary>
	/// <param name="type">Type.</param>
	public void SetMsgType(MsgType type){
		Debug.Log("SetMsgType");
		var msgs = m_msgs.Where(p=>p.type == type).First().messages;
		messages = new string[msgs.Length];
		for(int index = 0;index < msgs.Length;index++){
			messages[index] = msgs[index];
		}
		m_taskStartFlag = true;
		m_msgIndex = 0;
		m_task = MoveTask;
	}
		

	void MoveTask(){
		if(m_isDisfOff){
			m_task = null;
		}
		if(m_taskStartFlag){
			m_taskStartFlag = false;
			StartCoroutine(MoveCoroutine());
		}
	}


	IEnumerator MoveCoroutine(){

		if(messages.Length == 0){
			m_taskStartFlag = true;
			yield break;
		}

		m_text.text = messages[m_msgIndex % messages.Length];
		m_text.enabled = true;
		iTween.MoveFrom(m_text.gameObject, iTween.Hash (
			"x", -m_width
			, "time", 1
			,"easetype", iTween.EaseType.easeInCirc
		));

		yield return new WaitForSeconds(m_waitMsgTime);

		iTween.MoveTo(m_text.gameObject, iTween.Hash (
			"x", m_width*2
			, "time", 1
			,"easetype", iTween.EaseType.easeOutCirc
		));
		yield return new WaitForSeconds(m_waitMsgTime);
		m_text.enabled = false;
		m_text.transform.localPosition = new Vector3(0,0,0);
		m_taskStartFlag = true;
		m_msgIndex++;
		yield break;
	}
}
