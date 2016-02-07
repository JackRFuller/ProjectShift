using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;

    [Header("Movement Attributes")]
    public float movementSpeed;

	// Use this for initialization
	void Start () {

        Init();
	
	}

    void Init()
    {
        rb = GetComponent<Rigidbody2D>();
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

        rb.AddForce(_movementDirection * Time.deltaTime, ForceMode2D.Force);

        Debug.Log("hit");
    }
}
