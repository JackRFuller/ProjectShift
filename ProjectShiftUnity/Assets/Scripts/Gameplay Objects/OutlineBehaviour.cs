using UnityEngine;
using System.Collections;

public class OutlineBehaviour : MonoBehaviour {

	private SpriteRenderer sr;
	private Color currentColor;

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.material.mainTexture = spriteRenderer.sprite.texture;
    }


    void OnEnable()
    {
        Init();
    }

	void Init()
	{
		sr = GetComponent<SpriteRenderer>();
        //GetComponent<Animator>().enabled = true;
		currentColor = sr.color;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Color _objectsColour = other.GetComponent<SpriteRenderer>().color;

		if(gameObject.tag == other.tag)
		{
			int numOfGemsEarned = 0;

//			if(currentColor == _objectsColour)
//				numOfGemsEarned++;

			if(gameObject.tag == other.tag)
				numOfGemsEarned++;

            FreezePlayerShape(other.transform);

            //Trigger Level Complete
            LevelManager.instance.IncrementCurrentLevel();

			//Add On Score
			LevelManager.instance.IncremementScore();

            LevelHolderBehaviour lhScript = transform.parent.root.GetComponent<LevelHolderBehaviour>();
            lhScript.StartMoving(1);           
						
		}
		else
		{
			//Destroy(other.gameObject);
		}
	}

    void FreezePlayerShape(Transform playerShape)
    {
        //GetComponent<Animator>().enabled = false;
        playerShape.parent = transform;
        playerShape.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        playerShape.localPosition = Vector3.zero;
        playerShape.localRotation = transform.localRotation;
    }

}
