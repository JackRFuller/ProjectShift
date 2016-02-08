using UnityEngine;
using System.Collections;

public class OutlineBehaviour : MonoBehaviour {

	private SpriteRenderer sr;
	private Color currentColor;

	// Use this for initialization
	void Start () {

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
						
		}
		else
		{
			Destroy(other.gameObject);
		}
	}

}
