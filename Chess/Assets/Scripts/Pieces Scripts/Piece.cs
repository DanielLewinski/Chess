using UnityEngine;
using System.Collections;
//Do not use this script on pawn

public class Piece : MonoBehaviour 
{
	public Vector3[] directions;
	public bool isRepetitive;

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}

	void OnMouseDown()
	{
		GetLegalMoves();
	}

	void OnMouseUp()
	{
		CleanBoard();
		Event e = Event.current;
		Debug.Log(e.mousePosition.x);
	}

	void GetLegalMoves()
	{
		foreach (Vector3 direction in directions)
		{
			Vector3 actualPosition = transform.position;
			actualPosition += direction;
			while (isInRange(actualPosition))
			{
				Field field = Board.board[(int)actualPosition.x, (int)actualPosition.y].GetComponent<Field>();

				if (field.isVacant)
					field.isLegal = true;
				else
				{
					if (field.isCapturedByOpponent)
						field.isLegal = true;
					break;
				}

				if (!isRepetitive)
					break;

				actualPosition += direction;
			}
		}
	}

	bool isInRange(Vector3 position)
	{
		return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
	}

	void CleanBoard()
	{
		foreach (GameObject field in Board.board)
		{
			field.GetComponent<Field>().isLegal = false;
		}
	}
}