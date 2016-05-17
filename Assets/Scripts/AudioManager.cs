using UnityEngine;
using System.Collections;

/// <summary>
/// Audio manager.
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager> {

	public enum SE {
		/// <summary>
		/// 花火
		/// </summary>
		HANABI,
		/// <summary>
		/// キラキラおん
		/// </summary>
		KIRAKIRA,
	}

	//[SerializeField] AudioSource m_hanabi;
	//[SerializeField] AudioSource m_kirakira;

	public void Play(SE se){
		switch (se){
		case SE.HANABI:
			//m_hanabi.Play();
			break;
		case SE.KIRAKIRA:
			//m_kirakira.Play();
			break;
		}
	}
}
