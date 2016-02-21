using UnityEngine;
using System.Collections;

public class LevelHolderBehaviour : MonoBehaviour {

    public enum levelState
    {
        spawnedIn,
        active,
        finished,
    }

    public levelState currentLevelState;

   

    [Header("Lerping Attributes")]
    [SerializeField] private float timeTakenToLerp = 0.5F;
    private Vector3[] targetPos = new Vector3[2];
    private bool isLerping;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private float timeStartedLerping;


    void OnEnable()
    {
        currentLevelState = levelState.spawnedIn;
        targetPos[0] = new Vector3(0, -14, 0);
        targetPos[1] = new Vector3(-40, -14, 0);
    }
	
	// Update is called once per frame
	void Update () {

        if (isLerping)
        {
            MoveLevelHolder();
        }
	
	}

    void MoveLevelHolder()
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / timeTakenToLerp;

        transform.position = Vector3.Lerp(_startPos, _endPos, percentageComplete);

        if(percentageComplete >= 1.0F)
        {
            isLerping = false;

            if(_endPos == targetPos[1])
            {
                for(int i = 0; i <transform.childCount; i++)
                {
                    Transform childedObject = transform.GetChild(i);

                    if (childedObject.gameObject.name.Contains("Player"))
                    {
                        GameObject _newPlayer = transform.GetChild(i).gameObject;
                        _newPlayer.transform.parent = null;
                        //Reset Player Shape
                        PlayerController pcScript = _newPlayer.GetComponent<PlayerController>();
                        pcScript.ResetShape();                     
                        
                    }

                    childedObject.parent = null;
                    childedObject.gameObject.SetActive(false);
                    
                }

                gameObject.SetActive(false);

                WaveManager.instance.BringInNextLevel();

            }
        }
    }

    public void StartMoving(int _targetPosID)
    {
        _startPos = transform.position;
        _endPos = targetPos[_targetPosID];
        timeStartedLerping = Time.time;
        isLerping = true;

    }
}
