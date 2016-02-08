using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager instance;

    [Header("Level Holder")]
    public GameObject levelHolderPrefab;
    [SerializeField] private int numberOfLHToSpawn;
    [HideInInspector] public List<GameObject> levelHolders;

    [Header("Outlines")]
    public GameObject[] outlinePrefabs = new GameObject[4];
    [SerializeField] private int numberOfOutlinesToSpawn;
    public GameObject outlineTidy;
    [HideInInspector] public List<GameObject> outlines;

    [Header("Player Shapes")]
    public GameObject[] shapePrefabs = new GameObject[4];
    [SerializeField] private int numberOfShapesToSpawn;
    public GameObject shapeTidy;
    [HideInInspector] public List<GameObject> shapes;

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

        Init();
	
	}

    void Init()
    {
        SpawnInItems();
    }

    void SpawnInItems()
    {
        SpawnInLevelHolders();

        SpawnInOutlines();

        SpawnInPlayerShapes();
    }

    void SpawnInLevelHolders()
    {
        levelHolders = new List<GameObject>();
        for(int i = 0; i < numberOfLHToSpawn; i++)
        {
            GameObject _levelHolders = Instantiate(levelHolderPrefab) as GameObject;
            _levelHolders.SetActive(false);
            levelHolders.Add(_levelHolders);
        }
    }

    void SpawnInOutlines()
    {
        outlines = new List<GameObject>();
        for(int i = 0; i < outlinePrefabs.Length; i++)
        {
            for(int j = 0; j < numberOfOutlinesToSpawn; j++)
            {
                GameObject _outline = Instantiate(outlinePrefabs[i]) as GameObject;
                _outline.transform.parent = outlineTidy.transform;
                _outline.transform.localPosition = Vector3.zero;
                _outline.SetActive(false);
                outlines.Add(_outline);

            }
        }
    }

    void SpawnInPlayerShapes()
    {
        shapes = new List<GameObject>();

        for(int i = 0; i < shapePrefabs.Length; i++)
        {
            for(int j = 0; j < numberOfShapesToSpawn; j++)
            {
                GameObject _playerShape = Instantiate(shapePrefabs[i]) as GameObject;
                _playerShape.transform.parent = shapeTidy.transform;
                _playerShape.transform.localPosition = Vector3.zero;
                _playerShape.SetActive(false);
                shapes.Add(_playerShape);
            }
        }

        //Test
        WaveManager.instance.DetermineLevelType();
    }

    void ResetOutlines()
    {
        for(int i = 0; i < outlines.Count; i++)
        {
            outlines[i].transform.parent = outlineTidy.transform.parent;
            outlines[i].transform.localPosition = Vector3.zero;
            outlines[i].SetActive(false);            
        }
    }

    void ResetPlayerShapes()
    {
        for(int i = 0; i < shapes.Count; i++)
        {
            shapes[i].transform.parent = shapeTidy.transform.parent;
            shapes[i].transform.localPosition = Vector3.zero;
            shapes[i].SetActive(false);
        }
    }

    void ResetLevelHolders()
    {
        for(int i = 0; i < levelHolders.Count; i++)
        {
            levelHolders[i].transform.position = new Vector3(0, -14, 0);
            levelHolders[i].SetActive(false);
        }
    }
	
}
