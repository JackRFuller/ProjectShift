using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	[Header("Time Attributes")]
	public float startingTime;
	private float timer;

	// Use this for initialization
	void Start () {

		Init();
	
	}

	void Init()
	{
		//LevelManager.instance.initNextLevel += SetTimer;
		SetTimer();
	}
	
	// Update is called once per frame
	void Update () {

		RunTimer();
	
	}

	void SetTimer()
	{
		timer = startingTime;
		UIManager.instance.UpdateTimer(timer);
	}

	void RunTimer()
	{
		timer -=  Time.deltaTime;
		if(timer < 0)
		{
			timer = 0;
		}

		//Update UI with current time
		UIManager.instance.UpdateTimer(timer);
	}
}
