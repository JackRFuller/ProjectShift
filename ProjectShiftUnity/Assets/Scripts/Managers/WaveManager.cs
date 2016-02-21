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
    public Sprite[] shapeShifterSprites = new Sprite[4];

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

        outlines.Clear();
        generatedOutlines.Clear();
        generatedOutlinePos.Clear();

        if (levels.Count < 2)
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
        bool isGeneratingShapeShifters = false;

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
        for (int i = 0; i < levels[0].transform.childCount; i++)
        {
            levels[0].transform.GetChild(i).parent = null;
        }

        levels[0].SetActive(false);

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

        if(_levelHolder == null)
        {
            Debug.Log("Lack of Level Holders");
        }

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
            //Choose the correct combo of shape shifter and outline
            int _shapeShifterID = Random.Range(0, generatedOutlines.Count);

            GameObject _shapeShifter = outlines[_shapeShifterID].transform.GetChild(0).gameObject;

            GameObject _correctShapeShifter = _shapeShifter;

            //Set Shape Shifter Sprite
            _shapeShifter.GetComponent<SpriteRenderer>().sprite = PullShapeShifterSprite(outlines[_shapeShifterID].tag);

            //Set Shape Shifter Tag
            _shapeShifter.GetComponent<ShapeShifterBehaviour>().shapeTag = outlines[_shapeShifterID].tag;

            //Set Shape Shifter Position
            for (int i =0; i < outlineSpawnPos.Length; i++)
            {
                if(outlines[_shapeShifterID].transform.localPosition == outlineSpawnPos[i])
                {
                    _shapeShifter.transform.localPosition = shapeShifterSpawnPos[i];
                }
            }

            //Set Random Colour
            _shapeShifter.GetComponent<SpriteRenderer>().color = objectColours[Random.Range(1, 4)];

            _shapeShifter.SetActive(true);  
            
            if(generatedOutlines.Count > 1)
            {
                for (int j = 0; j < generatedOutlines.Count; j++)
                {
                    if (outlines[j].transform.GetChild(0).gameObject != _correctShapeShifter)
                    {
                        _shapeShifter = outlines[j].transform.GetChild(0).gameObject;

                        //Pick a Random Shape Tag
                        string _shapeShifterTag = PullObjectTag(Random.Range(0, 4));

                        while(outlines[j].tag == _shapeShifterTag)
                        {
                            _shapeShifterTag = PullObjectTag(Random.Range(0, 4));
                        }                        

                        //Set Shape Shifter Sprite
                        _shapeShifter.GetComponent<SpriteRenderer>().sprite = PullShapeShifterSprite(_shapeShifterTag);

                        //Set Shape Shifter Tag
                        _shapeShifter.GetComponent<ShapeShifterBehaviour>().shapeTag = _shapeShifterTag;

                        //Set Random Colour
                        _shapeShifter.GetComponent<SpriteRenderer>().color = objectColours[Random.Range(1, 4)];

                        //Set Shape Shifter Position
                        for (int i = 0; i < outlineSpawnPos.Length; i++)
                        {
                            if (outlines[j].transform.localPosition == outlineSpawnPos[i])
                            {
                                _shapeShifter.transform.localPosition = shapeShifterSpawnPos[i];
                            }
                        }

                        _shapeShifter.SetActive(true);
                    }                   
                }
            }          
        }

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

    Sprite PullShapeShifterSprite(string _shapeShifterTag)
    {

        Sprite _shapeShifterSprite = null;

        switch (_shapeShifterTag)
        {
            case ("Hexagon"):
                _shapeShifterSprite = shapeShifterSprites[0];
                break;
            case ("Square"):
                _shapeShifterSprite = shapeShifterSprites[1];
                break;
            case ("Triangle"):
                _shapeShifterSprite = shapeShifterSprites[2];
                break;
            case ("Star"):
                _shapeShifterSprite = shapeShifterSprites[3];
                break;
        }

        return _shapeShifterSprite;
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
            }
        }


        return levelHolder;
            
    }    
}
