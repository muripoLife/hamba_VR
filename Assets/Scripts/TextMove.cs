using UnityEngine;
using System.Collections;

/// <summary>
/// Text move.
/// </summary>
public class TextMove : MonoBehaviour {

	[SerializeField] float		m_radius;

	[SerializeField] float		m_anglerSpeed = 50;

	System.Action	m_task;
	GameObject		m_target;
	float			m_textY;
	float			m_time;

	void Awake(){
	}

	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {
		if(m_task!=null){
			m_task();
		}
	}


	/// <summary>
	/// Init the specified target.
	/// </summary>
	/// <param name="target">Target.</param>
	public void Init(GameObject target){
		m_time = UnityEngine.Random.Range(0f,500f);
		m_target = target;
		m_textY = m_target.transform.localPosition.y + UnityEngine.Random.Range(0.0f,9.0f);
		transform.localPosition = new Vector3(
			m_radius * Mathf.Cos(Mathf.Deg2Rad*CalcDeg(m_time)),
			m_textY,
			m_radius * Mathf.Sin(Mathf.Deg2Rad*CalcDeg(m_time))
		);
		m_task = MoveTask;
	}


	/// <summary>
	/// Moves the task.
	/// </summary>
	void MoveTask(){
		transform.localPosition = 
			new Vector3(
				m_radius * Mathf.Cos(Mathf.Deg2Rad*CalcDeg(m_time)),
				m_textY,
				m_radius * Mathf.Sin(Mathf.Deg2Rad*CalcDeg(m_time))
			);
		transform.LookAt(m_target.transform,Vector3.up);
		m_time += Time.deltaTime;
	}


	/// <summary>
	/// Calculates the deg.
	/// </summary>
	/// <returns>The deg.</returns>
	/// <param name="time">Time.</param>
	float CalcDeg(float time){
		return (time * m_anglerSpeed) % 360f;
	}
}
