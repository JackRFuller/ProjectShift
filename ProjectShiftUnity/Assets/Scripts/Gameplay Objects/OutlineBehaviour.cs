using UnityEngine;
using System.Collections;

public class OutlineBehaviour : MonoBehaviour {

	private SpriteRenderer sr;
	private Color currentColor;	

    void OnEnable()
    {
        Init();
    }

	void Init()
	{
		sr = GetComponent<SpriteRenderer>();
		currentColor = sr.color;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Color _objectsColour = other.GetComponent<SpriteRenderer>().color;

		if(currentColor == _objectsColour || gameObject.tag == other.tag)
		{
			int numOfGemsEarned = 0;

			if(currentColor == _objectsColour)
				numOfGemsEarned++;

			if(gameObject.tag == other.tag)
				numOfGemsEarned++;

            FreezePlayerShape(other.transform);

           
						
		}
		else
		{
			Destroy(other.gameObject);
		}
	}

    void FreezePlayerShape(Transform playerShape)
    {
        playerShape.parent = transform;
        playerShape.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        playerShape.localPosition = Vector3.zero;
        playerShape.localRotation = transform.localRotation;
    }

}
