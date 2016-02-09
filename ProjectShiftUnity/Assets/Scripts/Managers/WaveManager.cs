using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

    public static WaveManager instance;

    [Header("Object Attributes")]
    public Color[] objectColours = new Color[5];
    public Vector3[] outlineSpawnPos = new Vector3[4];


    [Header("Level Balancing Attributes")]
    public int[] levelLimits = new int[5];

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
	
	}	

    //Work out where the player is in terms of current level against level balancing
   public void DetermineLevelType()
    {
        for(int i = 0; i < levelLimits.Length; i++)
        {
            if(LevelManager.instance.currentLevel >= levelLimits[i] && LevelManager.instance.currentLevel < levelLimits[i + 1])
            {
                LoadInLevelType(i);
                break;
            }
        }
    }

    //Load in level appropriate to player's level
    void LoadInLevelType(int _levelID)
    {
        switch (_levelID)
        {
            case (0):
                LoadInInitialLevel();
                break;
            case (1):
                break;
            case (2):
                break;
            case (3):
                break;            
        }
    }

    void LoadInInitialLevel()
    {
        //Pull Outline
        int outlineToLoad = Random.Range(0, SpawnManager.instance.outlinePrefabs.Length);
        string objectsTag = PullObjectTag(outlineToLoad);
        GameObject outline = PullOutlines(objectsTag);

		Color _generatedColour = objectColours[Random.Range(1,5)];
		outline.GetComponent<SpriteRenderer>().color = _generatedColour;

        //Pull Level Holder
        GameObject levelHolder = LevelHolder();

        //Set Outline Position
        outline.transform.parent = levelHolder.transform;
        outline.transform.localPosition = outlineSpawnPos[Random.Range(0, outlineSpawnPos.Length)];

        //Pull Player Shape & Set Pos
        GameObject playerShape = PullPlayerShape(objectsTag);
        playerShape.transform.parent = levelHolder.transform;
        playerShape.transform.localPosition = Vector3.zero;
		playerShape.GetComponent<SpriteRenderer>().color = _generatedColour;

        levelHolder.SetActive(true);
        outline.SetActive(true);
        playerShape.SetActive(true);

		//Set Current Player
		InputManager.instance.currentPlayer = playerShape;
    }

    GameObject PullPlayerShape(string playerShapeID)
    {
        GameObject _playerShape = null;

        for(int i = 0; i < SpawnManager.instance.shapes.Count; i++)
        {
            if(SpawnManager.instance.shapes[i].tag == playerShapeID)
            {
                if (!SpawnManager.instance.shapes[i].activeInHierarchy)
                {
                    _playerShape = SpawnManager.instance.shapes[i];
                    return _playerShape;
                }
            }
        }

        return _playerShape;
    }

    string PullObjectTag(int objectID)
    {
        string objectTag = "";

        switch (objectID)
        {
            case (0):
                return objectTag = "Square";
                
            case (1):
                return objectTag = "Hexagon";
                
            case (2):
                return objectTag = "Star";
                
            case (3):
                return objectTag = "Triangle";
                
        }

        return objectTag;
    }

    GameObject PullOutlines(string outlineID)
    {
        GameObject outline = null;

        for(int i = 0; i < SpawnManager.instance.outlines.Count; i++)
        {
            if(SpawnManager.instance.outlines[i].tag == outlineID)
            {
                if (!SpawnManager.instance.outlines[i].activeInHierarchy)
                {
                    outline = SpawnManager.instance.outlines[i];
                    return outline;
                }
                    
            }
        }

        return outline;
    }

    GameObject LevelHolder()
    {
        GameObject levelHolder = null;

        for(int i = 0; i < SpawnManager.instance.levelHolders.Count; i++)
        {
            if (!SpawnManager.instance.levelHolders[i].activeInHierarchy)
            {
                levelHolder = SpawnManager.instance.levelHolders[i];
                levelHolder.transform.position = new Vector3(0, -14, 0);
                return levelHolder;
            }
        }

        return levelHolder;
            
    }    
}
