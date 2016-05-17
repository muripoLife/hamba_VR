using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// Text create.
/// </summary>
public class TextCreate : SingletonMonoBehaviour<TextCreate> {


	/// <summary>
	/// 外部に公開するObservable
	/// </summary>
	public IObservable<bool> onVisibleTextObservable
	{
		get
		{
			return m_onVisibleStream.AsObservable();
		}
	}

	/// <summary>
	/// The is done effect.
	/// </summary>
	Subject<bool> m_onVisibleStream = new Subject<bool>();

	[SerializeField] GameObject m_textPrefab;
	[SerializeField] GameObject	m_text2AnimationPrefab;

	GameObject	m_textMesh;
	GameObject	m_textAnimation;

	string m_text;

	/// <summary>
	/// Delete this instance.
	/// </summary>
	public void Delete(){
		if(m_textAnimation!=null){
			Destroy(m_textAnimation);
			m_textAnimation = null;
		}
	}


	/// <summary>
	/// Inits the text.
	/// </summary>
	/// <param name="text">Text.</param>
	public void InitText(string text){
		m_text = text;
	}

	/// <summary>
	/// Moves the text.
	/// </summary>
	public void MoveText(){
		if(m_textAnimation==null){
			m_textAnimation = Instantiate (m_text2AnimationPrefab) as GameObject;
		}

		m_textAnimation.GetComponent<Text2Animation> ().Create(m_text);
		m_textAnimation.transform.localPosition = new Vector3(7,0,-1.5f);
		m_textAnimation.GetComponent<Text2Animation> ()
			.isCreateTextObservable
			.Subscribe(p=>Next(p));
	}

	void Next(bool flag){
		m_onVisibleStream.OnNext(flag);
		m_onVisibleStream.OnCompleted();
	}
}
