using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {

    public static WaveManager instance;

    [Header("Object Attributes")]
    public Color[] objectColours = new Color[5];
    public Vector3[] outlineSpawnPos = new Vector3[4];
    public Vector3[] levelHolderSpawns = new Vector3[2];

    [Header("Level Balancing Attributes")]
    public int[] levelLimits = new int[5];

    private List<GameObject> levels = new List<GameObject>();
    private List<GameObject> playerShapes = new List<GameObject>();

    [Tooltip("Used To Hold the Current Level + 1")]
    private int tempWaveNum;

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

    void Start()
    {
        tempWaveNum = LevelManager.instance.currentLevel;
    }

    //Work out where the player is in terms of current level against level balancing
   public void DetermineLevelType()
    {
        //DebugText
        ManagerForDebugs.instance.WaveInit();     

        if(levels.Count < 2)
        {
            for (int i = 0; i < levelLimits.Length; i++)
            {
                if (tempWaveNum >= levelLimits[i] && tempWaveNum < levelLimits[i + 1])
                {
                    LoadInLevelType(i);
                    break;
                }
            }
        }
        
    }

    //Load in level appropriate to player's level
    void LoadInLevelType(int _levelID)
    {
        switch (_levelID)
        {
            case (0):
               LoadOneShapedLevel();
                break;
            case (1):
                LoadInTwoShapedLevel();
                break;
            case (2):
                break;
            case (3):
                break;            
        }
    }

    public void BringInNextLevel()
    {
        levels.RemoveAt(0);
        playerShapes.RemoveAt(0);
        levels[0].SetActive(true);
        LevelHolderBehaviour lhScript = levels[0].GetComponent<LevelHolderBehaviour>();
        lhScript.StartMoving(0);
        InputManager.instance.currentPlayer = playerShapes[0];
        LevelManager.instance.movesAvailableForLevel = 1;
        DetermineLevelType();
    }

    void LoadOneShapedLevel()
    {
        //DebugLevelType
        ManagerForDebugs.instance.WaveType("One Shape");


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

       if(levels.Count == 0)
        {
            //Set Current Player
            playerShapes.Add(playerShape);
            InputManager.instance.currentPlayer = playerShape;

            levelHolder.transform.position = levelHolderSpawns[0];

            levelHolder.SetActive(true);
            outline.SetActive(true);
            playerShape.SetActive(true);

            tempWaveNum++;

            //Adds Level To List
            levels.Add(levelHolder);
            DetermineLevelType();            
        }
        else
        {
            levelHolder.transform.position = levelHolderSpawns[1];

            //Adds Level To List
            levels.Add(levelHolder);
        }

        Debug.Log("Loaded Wave 1");	

        
    }

    void LoadInTwoShapedLevel()
    {
        //DebugLevelType
       ManagerForDebugs.instance.WaveType("Two Shapes");

        //Enable Checks
        List<GameObject> outlines = new List<GameObject>();
        List<Vector3> spawnedPositions = new List<Vector3>();

        GameObject levelHolder = LevelHolder();
        GameObject outline;
        string objectsTag = "";
        Color _generatedColour = Color.white;

        for (int i = 0; i < 2; i++)
        {
            //Choose Random Outline
            int outlineToLoad = Random.Range(0, SpawnManager.instance.outlinePrefabs.Length);
            objectsTag = PullObjectTag(outlineToLoad);
            outline = PullOutlines(objectsTag);

            //Check That The Outline Hasn't Been Chosen Already
            if (outlines.Count == 0 )
            {
                outline.transform.parent = levelHolder.transform;
                outline.transform.localPosition = outlineSpawnPos[Random.Range(0, outlineSpawnPos.Length)];
                _generatedColour = objectColours[Random.Range(1, 5)];
                outline.GetComponent<SpriteRenderer>().color = _generatedColour;

                outline.SetActive(true);
                outlines.Add(outline);                
            }
            else
            {
                SpriteRenderer outlineSprite = outline.GetComponent<SpriteRenderer>();
                //Check it isn't the same shape
                while (outlineSprite.sprite == outlines[0].GetComponent<SpriteRenderer>().sprite)
                {
                    outlineToLoad = Random.Range(0, SpawnManager.instance.outlinePrefabs.Length);
                    objectsTag = PullObjectTag(outlineToLoad);
                    outline = PullOutlines(objectsTag);
                    outlineSprite = outline.GetComponent<SpriteRenderer>();
                }
               
                outline.transform.parent = levelHolder.transform;
                outline.transform.localPosition = outlineSpawnPos[Random.Range(0, outlineSpawnPos.Length)];

                ////Check that it isn't in the same location
                while (outline.transform.localPosition == outlines[0].transform.localPosition)
                {
                    outline.transform.localPosition = outlineSpawnPos[Random.Range(0, outlineSpawnPos.Length)];
                }

                _generatedColour = objectColours[Random.Range(1, 5)];
                outlineSprite.color = _generatedColour;

                ////Check that it isn't the same colour
                while (outlineSprite.color == outlines[0].GetComponent<SpriteRenderer>().color)
                {
                    _generatedColour = objectColours[Random.Range(1, 5)];
                    outlineSprite.color = _generatedColour;
                }

                outline.SetActive(true);
                outlines.Add(outline);
            }

        }

        //Pull Player Shape & Set Pos
        GameObject playerShape = PullPlayerShape(objectsTag);
        playerShape.transform.parent = levelHolder.transform;
        playerShape.transform.localPosition = Vector3.zero;
        playerShape.GetComponent<SpriteRenderer>().color = _generatedColour;

        if(levels.Count == 0)
        {
            levelHolder.transform.position = levelHolderSpawns[0];

            levelHolder.SetActive(true);
            playerShape.SetActive(true);

            //Set Current Player
            playerShapes.Add(playerShape);
            InputManager.instance.currentPlayer = playerShape;
            tempWaveNum++;
            //Adds Level To List
            levels.Add(levelHolder);

            DetermineLevelType();

            
        }
        else
        {
            playerShapes.Add(playerShape);
            levelHolder.transform.position = levelHolderSpawns[1];
            playerShape.SetActive(true);
            //Adds Level To List
            levels.Add(levelHolder);
        }


        
    }

    void DetermineIfNextLevelShouldBeSpawnedIn()
    {
        if(levels.Count < 2)
        {
            DetermineLevelType();
        }
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
