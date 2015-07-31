using UnityEngine;
using System.Collections;
//Do not use this script on pawn

public class Piece : MonoBehaviour 
{
	public Vector3[] directions;
	public bool isRepetitive;
	public bool isWhite = true;
	public bool isPawn = false;

	void Start () 
	{
	}
	
	void Update () 
	{
	
	}

	void OnMouseDown()
	{
		if (!(Game.isWhitesTurn ^ isWhite) && !isPawn)
			GetLegalMoves();
	}

	void OnMouseUp()
	{
		if (!(Game.isWhitesTurn ^ isWhite))
			MakeMove();
	}

	void MakeMove()
	{
		Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		target = new Vector3(Mathf.Round(target.x), Mathf.Round(target.y), 0);

		if ((target.x != transform.position.x || target.y != transform.position.y) && Game.isInRange(target))
		{
			Field targetField = Board.board[(int)target.x, (int)target.y].GetComponent<Field>();
			if (targetField.isLegal)
			{
				if (isPawn)
					GetComponent<Pawn>().UpdatePawnStatus(target);

				Field currentField = Board.board[(int)transform.position.x, (int)transform.position.y].GetComponent<Field>();
				currentField.HoldedPiece = null;

				transform.position = target;
				targetField.Capture();
				targetField.HoldedPiece = gameObject;

				Game.NextTurn();
			}
			else
				Debug.Log("Illegal");
		}
		else
			Debug.Log("No movement");

		Game.CleanBoard();
	}

	void GetLegalMoves()
	{
		foreach (Vector3 direction in directions)
		{
			Vector3 actualPosition = transform.position;
			actualPosition += direction;
			while (Game.isInRange(actualPosition))
			{
				Field field = Board.board[(int)actualPosition.x, (int)actualPosition.y].GetComponent<Field>();

				if (field.HoldedPiece == null)
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

	
}