using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

/// <summary>
/// Text2 animation.
/// </summary>
public class Text2Animation : MonoBehaviour {

	[System.Serializable]
	public class TextAnims {
		public char			kana;
		public GameObject	animPrefab;
	}

	[SerializeField] GameObject kaPrefab;


	/// <summary>
	/// 外部に公開するObservable
	/// </summary>
	public IObservable<bool> isCreateTextObservable
	{
		get
		{
			return m_isCreateTextObservable.AsObservable();
		}
	}

	/// <summary>
	/// The is done effect.
	/// </summary>
	Subject<bool> m_isCreateTextObservable = new Subject<bool>();


	public TextAnims[] texts;

	List<GameObject> m_objs = new List<GameObject>();

	//public GameObject text;

	/// <summary>
	/// Create the specified text.
	/// </summary>
	/// <param name="text">Text.</param>
	public void Create(string text){
		int index = 0;
		foreach(var c in text){
			var anims= texts.Where(p=>p.kana == c).FirstOrDefault();
			GameObject anim = null;
			if(anims!=null && anims.animPrefab!=null){
				anim = Instantiate(anims.animPrefab) as GameObject;
				anim.transform.parent = transform;
				var size = anim.GetComponent<SpriteRenderer>().bounds.size.x;
				anim.transform.localPosition = new Vector3(size * index,0,0);
				anim.transform.localScale = new Vector3(1,1,1);
				anim.GetComponent<TextAnimCont>().StartAnimation();
				m_objs.Add(anim);
			}
			else {
				anim = Instantiate(kaPrefab) as GameObject;
				anim.transform.parent = transform;
				var size = anim.GetComponent<SpriteRenderer>().bounds.size.x;
				anim.transform.localPosition = new Vector3(size * index,0,0);
				anim.transform.localScale = new Vector3(1,1,1);
				anim.GetComponent<TextAnimCont>().StartAnimation();
				m_objs.Add(anim);
			}
			index++;
		}
			
		m_objs[0].GetComponent<TextObserve>()
			.OnVisibleObservable
			.Subscribe(p=>Next(p));

	}

	void Next(bool flag){
		m_isCreateTextObservable.OnNext(flag);
		m_isCreateTextObservable.OnCompleted();
	}


	/// <summary>
	/// Gets the text object.
	/// </summary>
	/// <returns>The text object.</returns>
	GameObject GetTextObj(){
		if(m_objs.Count > 0){
			return m_objs[0];
		}
		return null;
	}
}
