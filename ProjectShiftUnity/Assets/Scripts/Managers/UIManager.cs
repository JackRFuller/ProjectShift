using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public static UIManager instance;

	[Header("Timer")]
	public Text timerText;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != null)
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}

	public void UpdateTimer(float _currentTime)
	{
		if(_currentTime > 0)
		{
			timerText.text = _currentTime.ToString("F2");
		}
		else
		{
			SetMainTimerToGameOver();
		}
	}

	void SetMainTimerToGameOver()
	{
		timerText.text = "Game Over";
		timerText.fontSize = 168;
	}


}
