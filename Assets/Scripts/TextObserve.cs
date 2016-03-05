using UnityEngine;
using System.Collections;
using UniRx;

public class TextObserve : MonoBehaviour {
	/// <summary>
	/// カメラに写ったGameObjectを流すストリーム
	/// </summary>
	Subject<GameObject> onVisibleStream = new Subject<GameObject>();

	/// <summary>
	/// 外部に公開するObservable
	/// </summary>
	public IObservable<GameObject> OnVisibleObservable
	{
		get
		{
			return onVisibleStream.AsObservable();
		}
	}

	/// <summary>
	/// カメラに写った時に実行されるUnityのコールバック
	/// </summary>
	public void OnBecameVisible()
	{
		Debug.Log("Disp Text3D In Camera");
		//OnNextで自分自身のGameObjectをストリームに流す
		onVisibleStream.OnNext(this.gameObject);
		onVisibleStream.OnCompleted();
	}
}
