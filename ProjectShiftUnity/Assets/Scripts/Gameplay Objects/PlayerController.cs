using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Header("Movement Attributes")]
    public float movementSpeed;

	// Use this for initialization
	void Start () {

        Init();
	
	}

    void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }	
	
    void MovePlayer(string targetDirection)
    {
        Vector2 _targetMovement = Vector2.zero;

        switch (targetDirection)
        {
            case ("Up"):
                _targetMovement = Vector2.up;
                break;
            case ("Down"):
                _targetMovement = Vector2.down;
                break;
            case ("Left"):
                _targetMovement = Vector2.left;
                break;
            case ("Right"):
                _targetMovement = Vector2.right;
                break;
        }

        Vector2 _movementDirection = _targetMovement * movementSpeed;

        rb.AddForce(_movementDirection, ForceMode2D.Force);

    }

    public void ResetShape()
    {
        transform.parent = null;
        transform.eulerAngles =  Vector3.zero;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "ShapeShifter")
        {
            SpriteRenderer sprite = other.GetComponent<SpriteRenderer>();
            sr.sprite = sprite.sprite;
            sr.color = sprite.color;

            ShapeShifterBehaviour ssbScript = other.GetComponent<ShapeShifterBehaviour>();

            gameObject.tag = ssbScript.shapeTag;

            other.gameObject.SetActive(false);
        }
    }
}
