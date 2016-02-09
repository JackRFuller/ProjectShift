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
                    if(transform.GetChild(i).childCount > 0)
                    {
                        //Reset Player Shape
                        PlayerController pcScript = transform.GetChild(i).GetChild(0).GetComponent<PlayerController>();
                        pcScript.ResetShape();                     
                        
                    }
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                gameObject.SetActive(false);
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
