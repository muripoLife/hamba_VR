using UnityEngine;
using System.Collections;

public class ParticleManager : SingletonMonoBehaviour<ParticleManager> {

	[SerializeField] ParticleSystem m_hanabi;


	/// <summary>
	/// Starts the hanabi.
	/// </summary>
	public void StartHanabi(){
		m_hanabi.Play();
	}

	/// <summary>
	/// Stops the hanabi.
	/// </summary>
	public void StopHanabi(){
		m_hanabi.Stop();
	}
}
