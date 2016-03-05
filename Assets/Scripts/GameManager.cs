using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.SceneManagement;
using NCMB;

/// <summary>
/// Game manager.
/// </summary>
public class GameManager : MonoBehaviour {

	[SerializeField] bool m_isSendMsg = false;

	void Start() {
		UIManager.Instance.Display(false);
		StartCoroutine(GameStart());
	}


	// Use this for initialization
	IEnumerator GameStart(){
		///サーバーから結果が文字列を取得するまで待ちUIを更新する
		yield return FetchManager.Instance.OnGetResponsedAsObservable().StartAsCoroutine();
		AudioManager.Instance.Play(AudioManager.SE.KIRAKIRA);
		InitUI(UIManager.MsgType.FINDMSG);
		Init3DText(FetchManager.Instance.Result);

		///テキストがカメラの範囲内に入ったらUIを見えなくする
		yield return TextCreate.Instance.onVisibleTextObservable.StartAsCoroutine();
		UIManager.Instance.Display(false);

		///テキストのエフェクトが始める
		TextEffect.Instance.StartEffect();
		///エフェクトが終わるまで待つ
		yield return TextEffect.Instance.isDoneEffectObservable.Where(p=>p == true).StartAsCoroutine();

		///はい、いいえを聞いてくる
		InitUI(UIManager.MsgType.YESNO);
		HeadTracking.Instance.TrakingStart();
		yield return HeadTracking.Instance.isGETYESNOObservable.StartAsCoroutine();
		UIManager.Instance.Display(false);
		GetYESNO(HeadTracking.Instance.isHeadGesture);
		HeadTracking.Instance.TrakingStop();

		yield return new WaitForSeconds(15.0f);
		SceneManager.LoadScene("celestialSphere");

		yield break;
	}

	/// <summary>
	/// Gets the response coroutine.
	/// </summary>
	/// <returns>The response coroutine.</returns>
	/// <param name="result">Result.</param>
	void Init3DText(string result){
		Debug.Log("Get Message In GameManager " + result);
		TextCreate.Instance.InitText(result);
		TextCreate.Instance.MoveText();
	}

	/// <summary>
	/// Inits the U.
	/// </summary>
	/// <param name="type">Type.</param>
	void InitUI(UIManager.MsgType type){
		Debug.Log("InitUI");
		UIManager.Instance.Display(true);
		UIManager.Instance.SetMsgType(type);
	}
		
	void GetYESNO(bool isYES){
		if(isYES){
			Debug.Log("YES");
			ParticleManager.Instance.StartHanabi();
			AudioManager.Instance.Play(AudioManager.SE.HANABI);
			InitUI(UIManager.MsgType.SUCCESS);
			SenDResult(true);
		}
		else {
			Debug.Log("NO");
			InitUI(UIManager.MsgType.FAILED);
			SenDResult(false);
		}
	}

	/// <summary>
	/// Sents the result.
	/// </summary>
	/// <param name="res">If set to <c>true</c> res.</param>
	void SenDResult(bool res){
		// 新しいメッセージを保存する
		NCMBObject testClass = new NCMBObject("Result");
		if(res){
			testClass["text"] = "YES";
		}
		else {
			testClass["text"] = "NO";
		}
		testClass.SaveAsync();
	}

#if UNITY_EDITOR
	void OnValidate(){
		if(m_isSendMsg){
			m_isSendMsg = false;
			// 新しいメッセージを保存する

			NCMBObject testClass = new NCMBObject("Messages");
			testClass["text"] = "あいうえおかきくけこ";
			testClass.SaveAsync();

		}
	}
#endif
}
