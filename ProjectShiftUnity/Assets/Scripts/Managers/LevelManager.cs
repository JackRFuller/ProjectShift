using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	[Header("Level Attributes")]
	public int currentLevel = 0;
	public int movesAvailableForLevel = 0;
	public delegate void nextLevel();
	public nextLevel initNextLevel;

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

	void Start()
	{
		Init();
        Debug.Log("Init");
	}

	void Init()
	{
		//Setup Next Level Delegate
		initNextLevel += IncrementCurrentLevel;
		initNextLevel += ResetNumberOfMovesAvailable;

        ManagerForDebugs.instance.GameStarted();
        Debug.Log("Game Started");
	}

	public void IncrementCurrentLevel()
	{        
		currentLevel++;
        ManagerForDebugs.instance.WaveNumber(currentLevel);
    }


	void ResetNumberOfMovesAvailable()
	{
		movesAvailableForLevel = 1;
	}


}
