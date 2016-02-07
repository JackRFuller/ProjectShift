using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public static InputManager instance = null;

    [Header("Swipe Attributes")]
    [SerializeField] public float minSwipeLength;
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;

    [Header("Player Attributes")]
    public GameObject currentPlayer;

    void Awake()
    {
        //Init Singleton
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
	void Update ()
    {
        DetectSwipe();
	}

    void DetectSwipe()
    {
        if(Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            if(t.phase == TouchPhase.Began)
            {
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }

            if(t.phase == TouchPhase.Ended)
            {
                secondPressPos = new Vector2(t.position.x, t.position.y);
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //Check that it wasn't a tap
                if(currentSwipe.magnitude < minSwipeLength)
                {
                    Debug.Log("Tap");
                    return;
                }

                currentSwipe.Normalize();

                DetectDirection();
            }
        }
        else
        {
            //Mouse Input
            if (Input.GetMouseButtonDown(0))
            {
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }            
            if (Input.GetMouseButtonUp(0))
            {
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                // Make sure it was a legit swipe, not a tap
                if (currentSwipe.magnitude < minSwipeLength)
                {
                    Debug.Log("Tap");
                    return;
                }

                currentSwipe.Normalize();

                //Swipe directional check
                DetectDirection();
               
            }
        }
    }

    void DetectDirection()
    {
        // Swipe up
        if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
        {
            Debug.Log("Swipe Up");
            currentPlayer.SendMessage("MovePlayer", "Up", SendMessageOptions.DontRequireReceiver);

        }
        else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
        {
            Debug.Log("Swipe Down");
            currentPlayer.SendMessage("MovePlayer", "Down", SendMessageOptions.DontRequireReceiver);
        }
        else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
        {
            Debug.Log("Swipe Left");
            currentPlayer.SendMessage("MovePlayer", "Left", SendMessageOptions.DontRequireReceiver);
        }
        else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
        {
            Debug.Log("Swipe Right");
            currentPlayer.SendMessage("MovePlayer", "Right", SendMessageOptions.DontRequireReceiver);
        }
    }
}
