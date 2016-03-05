using UnityEngine;
using System.Collections;

public class TextAnimCont : MonoBehaviour {

	[SerializeField] Animator m_anim;

	// Use this for initialization
	void Start () {
		m_anim.enabled = false;
	}

	/// <summary>
	/// Starts the animation.
	/// </summary>
	public void StartAnimation(){
		StartCoroutine(AnimStartCoroutine());
	}

	/// <summary>
	/// Animations the start coroutine.
	/// </summary>
	/// <returns>The start coroutine.</returns>
	IEnumerator AnimStartCoroutine(){
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.4f,0.8f));
		m_anim.enabled = true;
	}
}
