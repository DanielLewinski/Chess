using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	public static bool isWhitesTurn = true;
	public static uint turnsTaken = 0;

	void Start () 
	{
		UpdateFieldsStatus();
	}
	
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
			NextTurn();
	}

	static void UpdateFieldsStatus()
	{
		GameObject[] fields;
		fields = GameObject.FindGameObjectsWithTag("Field");
		foreach (GameObject field in fields)
		{
			Field currentField = field.GetComponent<Field>();
			currentField.CheckIfEnemy();
		}
	}

	public static void NextTurn()
	{
		++turnsTaken;
		isWhitesTurn = !isWhitesTurn;
		UpdateFieldsStatus();

		Camera.main.transform.Rotate(new Vector3(0, 0, 180));
		GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
		foreach (GameObject piece in pieces)
			piece.transform.Rotate(new Vector3(0, 0, 180));

	}

	public static bool isInRange(Vector3 position)
	{
		return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
	}

	public static void CleanBoard()
	{
		foreach (GameObject field in Board.board)
		{
			field.GetComponent<Field>().isLegal = false;
		}
	}
}