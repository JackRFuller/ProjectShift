using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {

    public static WaveManager instance;

    [Header("Object Attributes")]
    public Color[] objectColours = new Color[5];
    public Vector3[] outlineSpawnPos = new Vector3[4];
    public Vector3[] shapeShifterSpawnPos = new Vector3[4];
    public Vector3[] levelHolderSpawns = new Vector3[2];

    [Header("Level Balancing Attributes")]
    public int[] levelLimits = new int[5];

    private List<GameObject> levels = new List<GameObject>();
    private List<GameObject> playerShapes = new List<GameObject>();

    [Tooltip("Used To Hold the Current Level + 1")]
	public int tempWaveNum;
    private List<int> generatedOutlines = new List<int>();
    private List<int> generatedOutlinePos = new List<int>();
    private List<int> generatedShapeShifters = new List<int>();

    //Shape Shifter Lists
    private List<GameObject> outlines = new List<GameObject>();
    private List<string> outlineTags = new List<string>();
    private List<Vector3> shapeShifterPos = new List<Vector3>();

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
        generatedOutlines = new List<int>();
        generatedOutlinePos = new List<int>();
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
		int _numOfOutlines = 0;
        bool isGeneratingShapeShifters = true;

        switch (_levelID)
        {
            case (0):
			_numOfOutlines = 1;
                break;
            case (1):
			_numOfOutlines = 2;
                break;
            case (2):
			_numOfOutlines = 3;
                break;
            case (3):
			_numOfOutlines = 4;
                break;     
			case (4):
			_numOfOutlines = 4;
			break;    
        }

		//Debug.Log(_numOfOutlines);

		WaveGenerator(_numOfOutlines, isGeneratingShapeShifters);
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

	void WaveGenerator(int numOfOutlines, bool isGeneratingShapeShifters)
	{
		tempWaveNum++;

		//Debug Wave Type
		ManagerForDebugs.instance.WaveType("Num of Outlines: " + numOfOutlines.ToString());	
        		
		//Generate Level Holder
		GameObject _levelHolder = LevelHolder();

		//Generate Outline
		GameObject outline;

		//Generate Outline Tag
		string _outlineTag = "";

		//Generate Colour
		Color _generatedColour = Color.white;

		//Load in the 3 Outlines
		for(int i = 0; i < numOfOutlines; i++)
		{			
			//Generate An Outline
			//Choose Random Outline
			int outlineToLoad = Random.Range(0, SpawnManager.instance.outlinePrefabs.Length);

			//Pull the Outline's Tag
			_outlineTag = PullObjectTag(outlineToLoad);

			//Pull the Outline
			outline = PullOutlines(_outlineTag);

			//Check if an outlines has been generated and added to the list
			if(generatedOutlines.Count == 0)
			{
				//Assign Outline's Transform to Level Holder
				outline.transform.parent = _levelHolder.transform;

				//Choose A Random Position to Outline
				int _outlinePos = Random.Range(0, outlineSpawnPos.Length);
				outline.transform.localPosition = outlineSpawnPos[_outlinePos];

				//Choose a Random Colour for the Outline
				_generatedColour = objectColours[Random.Range(1, 5)];

				//Assign the random colour to the outline
				outline.GetComponent<SpriteRenderer>().color = _generatedColour;

				outline.SetActive(true);

				//Add Outline to the List
				generatedOutlines.Add(outlineToLoad);

				//Add OutlinePos To The List
				generatedOutlinePos.Add(_outlinePos);

			}
			else
			{				
				while(generatedOutlines.Contains(outlineToLoad))
				{
					outlineToLoad = Random.Range(0, SpawnManager.instance.outlinePrefabs.Length);
				}

				//Pull the Outline's Tag
				_outlineTag = PullObjectTag(outlineToLoad);

				//Pull the Outline
				outline = PullOutlines(_outlineTag);

				//Assign Outline's Transform to Level Holder
				outline.transform.parent = _levelHolder.transform;

				//Choose A Random Position to Outline
				int _outlinePos = Random.Range(0, outlineSpawnPos.Length);

				//Check that the position isn't in use
				while(generatedOutlinePos.Contains(_outlinePos))
				{
					_outlinePos = Random.Range(0, outlineSpawnPos.Length);
				}

				//Assign Generated Position
				outline.transform.localPosition = outlineSpawnPos[_outlinePos];

				//Choose a Random Colour for the Outline
				_generatedColour = objectColours[Random.Range(1, 5)];

				//Assign the random colour to the outline
				outline.GetComponent<SpriteRenderer>().color = _generatedColour;

				outline.SetActive(true);

				//Add Outline to the List
				generatedOutlines.Add(outlineToLoad);

				//Add OutlinePos To The List
				generatedOutlinePos.Add(_outlinePos);
			}

            outlines.Add(outline);
		}

        //Spawn In Shape Shifters
        if(isGeneratingShapeShifters)
        {
            //Choose Correct Shape for shifting
            int _correctShapeShift = Random.Range(0, generatedOutlines.Count);

            //Add Correct Shape To the list
            generatedShapeShifters.Add(_correctShapeShift);

            //Pull Shape Shift To Spawn
            string _shapeShift = outlines[_correctShapeShift].tag;

            //Add Generated Tag to List
            outlineTags.Add(_shapeShift);       

            GameObject _shapeShifter = PullShapeShifter(_shapeShift);           

            //Determine Position To Spawn
            for(int i =0; i < outlineSpawnPos.Length; i++)
            {
                if(outlines[_correctShapeShift].transform.localPosition == outlineSpawnPos[i])
                {
                    //Assign To Level Holder
                    _shapeShifter.transform.parent = _levelHolder.transform;
                    _shapeShifter.transform.localPosition = shapeShifterSpawnPos[i];
                    shapeShifterPos.Add(shapeShifterSpawnPos[i]);
                    break;
                }
            }

            //Assign Random Colour
            _shapeShifter.GetComponent<SpriteRenderer>().color = objectColours[Random.Range(1, 5)];

            //Set Shape Shifter As Active
            _shapeShifter.SetActive(true);

            //Check that there isn't just one outline
            if(generatedOutlines.Count > 1)
            {
                for(int j = 0; j < generatedOutlines.Count - 1; j++)
                {
                    int shapeShifterID = Random.Range(0, generatedOutlines.Count);
                    _shapeShift = outlines[shapeShifterID].tag;

                    //Check that Random Number Hasn't been pulled already
                    while (outlineTags.Contains(_shapeShift))
                    {
                        shapeShifterID = Random.Range(0, generatedOutlines.Count);
                        _shapeShift = outlines[shapeShifterID].tag;
                    }

                    //Add Generated Tag to List
                    outlineTags.Add(_shapeShift);

                    //Pull Shape Shifter
                    _shapeShifter = PullShapeShifter(_shapeShift);

                    int _spawnPosID = 0;
                    Vector3 _spawnPos = Vector3.zero;

                    //Determine Position To Spawn
                    for (int i = 0; i < numOfOutlines - 1; i++)
                    {
                        _spawnPosID = Random.Range(0, shapeShifterSpawnPos.Length);
                        _spawnPos = shapeShifterSpawnPos[_spawnPosID];

                        ////Check that the spawn position isn't already taken
                        //while (shapeShifterPos.Contains(_spawnPos) || outlines[_spawnPosID].tag == _shapeShift)
                        //{
                        //    _spawnPosID = Random.Range(0, shapeShifterSpawnPos.Length);
                        //    _spawnPos = shapeShifterSpawnPos[_spawnPosID];
                        //}                       
                    }


                }
            }
        }

        outlines.Clear();
        generatedOutlines.Clear();
        generatedOutlinePos.Clear();

        //Pull Player Shape & Set Pos
        GameObject playerShape = PullPlayerShape(_outlineTag);
		playerShape.transform.parent = _levelHolder.transform;
		playerShape.transform.localPosition = Vector3.zero;
		playerShape.GetComponent<SpriteRenderer>().color = objectColours[Random.Range(1, 5)];

		if(levels.Count == 0)
		{
			_levelHolder.transform.position = levelHolderSpawns[0];

			_levelHolder.SetActive(true);
			playerShape.SetActive(true);

			//Set Current Player
			playerShapes.Add(playerShape);
			InputManager.instance.currentPlayer = playerShape;
			//Adds Level To List
			levels.Add(_levelHolder);
			DetermineLevelType();


		}
		else
		{
			playerShapes.Add(playerShape);
			_levelHolder.transform.position = levelHolderSpawns[1];
			playerShape.SetActive(true);
			//Adds Level To List
			levels.Add(_levelHolder);
		}

       
	}

    void DetermineIfNextLevelShouldBeSpawnedIn()
    {
        if(levels.Count < 2)
        {
            DetermineLevelType();
        }
    }

    GameObject PullShapeShifter(string shapeShifterID)
    {
        GameObject _shapeShifter = null;

        for (int i = 0; i < SpawnManager.instance.shapeShifters.Count; i++)
        {
            ShapeShifterBehaviour ssb = SpawnManager.instance.shapeShifters[i].GetComponent<ShapeShifterBehaviour>();

            if(ssb.shapeTag == shapeShifterID)
            {
                if (!SpawnManager.instance.shapeShifters[i].activeInHierarchy)
                {                    
                    _shapeShifter = SpawnManager.instance.shapeShifters[i];
                    return _shapeShifter;
                }
            }
        }

        return _shapeShifter;
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
