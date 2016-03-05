using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// テキストのエフェクトを管理するクラス
/// </summary>
public class TextEffect : SingletonMonoBehaviour<TextEffect> {


	/// <summary>
	/// 外部に公開するObservable
	/// </summary>
	public IObservable<bool> isDoneEffectObservable
	{
		get
		{
			return m_isDoneStream.AsObservable();
		}
	}

	/// <summary>
	/// The is done effect.
	/// </summary>
	Subject<bool> m_isDoneStream = new Subject<bool>();

	/// <summary>
	/// エフェクト処理を開始する
	/// </summary>
	public void StartEffect(){
		///初期化
		StartCoroutine(EffectCoroutine());
	}


	/// <summary>
	/// エフェクトの処理をここに書く
	/// </summary>
	/// <returns>The coroutine.</returns>
	IEnumerator EffectCoroutine() {
		///下記の処理をエフェクトにさしかえる。
		yield return new WaitForSeconds(3.0f);
		m_isDoneStream.OnNext(true);
		m_isDoneStream.OnCompleted();
		Debug.Log("Effect End");
	}
}
