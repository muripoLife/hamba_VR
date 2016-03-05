using UnityEngine;
using System.Collections;
using UniRx;
using NCMB;
using System.Collections.Generic;
using System;
using System.Linq;


/// <summary>
/// Fetch manager.
/// </summary>
public class FetchManager : SingletonMonoBehaviour<FetchManager> {

	/// <summary>
	/// リトライ時間
	/// </summary>
	[SerializeField] float m_retryTime = 3.0f;

	[SerializeField] List<string> data = new List<string> ();

	[SerializeField] bool m_isClearPlayerPrefs = false;

	public string Result {
		get;private set;
	}

	private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	DateTime LastTextSendTime = DateTime.Parse ("1992/2/16 12:15:12");

	string dayString = "1992/2/16 12:15:12";
	string dayKey = "LastTextSendTime";

	void Start(){
		// TODO: 初回のみPlayerPrefsに保存し、２回目起動時はPlayerPrefsのあたいを利用
		if(PlayerPrefs.HasKey(dayKey)){
			dayString = PlayerPrefs.GetString(dayKey);
			Debug.Log("Get Time String" + dayString);
		}
		else {
			Debug.Log("Not Have Time Key. Set Time String");
			PlayerPrefs.SetString(dayKey,dayString );
		}
		LastTextSendTime = DateTime.Parse(dayString);

		GetText();
	}


	#if UNITY_EDITOR
	void OnValidate(){
		if(m_isClearPlayerPrefs){
			if(PlayerPrefs.HasKey(dayKey)){
				PlayerPrefs.DeleteKey(dayKey);
			}
			m_isClearPlayerPrefs = false;
		}
	}
	#endif



	/// <summary>
	/// メッセージを取得した事を通知する。
	/// </summary>
	/// <value>The on get responsed as observable.</value>
	public IObservable<string> OnGetResponsedAsObservable()
	{
		return m_onGetResponse.AsObservable();
	}

	private Subject<string> m_onGetResponse = new Subject<string>();


	void GetText ()
	{
		//QueryTestを検索するクラスを作成
		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Messages");
		// 最新の一件だけ取得
		query.Limit = 1;
		query.OrderByDescending ("updateDate");
		//検索    
		query.FindAsync ((List<NCMBObject> objList, NCMBException e) => {
			if (e != null) {
				//検索失敗時の処理
				StartRetry();
			} else {
				
				//オブジェクトを出力
				foreach (NCMBObject obj in objList) {
					Debug.Log("Message Receive");
					// 更新日時を比較
					var lastTime = FromDateTime(LastTextSendTime);
					var dataTime = FromDateTime(obj.UpdateDate.Value);
					Debug.Log("Load Time " + LastTextSendTime.ToString());
					Debug.Log("New Message Time" + obj.UpdateDate.Value.ToString());
					if (lastTime < dataTime){
						// 更新
						LastTextSendTime = obj.UpdateDate.Value;
						PlayerPrefs.SetString(dayKey,LastTextSendTime.ToString());
						PlayerPrefs.Save();

						// 若ければ取得
						data.Add (System.Convert.ToString (obj ["text"]));
					}
				}
					
				if(data.Count <= 0){
					StartRetry();
				}
				else if (data[0] == "") {
					data.Clear();
					StartRetry();
				}
				else {
					Result = data[0];
					Debug.Log("Get Response "+ data[0]);
					m_onGetResponse.OnNext(data[0]);
					data.Clear();
					m_onGetResponse.OnCompleted();
				}
			}
		});
	}
		
	/// <summary>
	/// Starts the retry.
	/// </summary>
	void StartRetry(){
		StartCoroutine(RetryFetch());
	}

	/// <summary>
	/// メッセージ受診失敗時に一呼吸おいてからリトライする
	/// </summary>
	/// <returns>The fetch.</returns>
	IEnumerator RetryFetch(){
		yield return new WaitForSeconds(m_retryTime);
		Debug.Log("Retry Get Text");
		GetText();
		yield return null;
	}

	public static long FromDateTime( DateTime dateTime )
	{
		double nowTicks = ( dateTime.ToUniversalTime() - UNIX_EPOCH ).TotalSeconds;
		return (long)nowTicks;
	}

}
