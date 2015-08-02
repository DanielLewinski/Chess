using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Do not use this script on pawn

public class Piece : MonoBehaviour 
{
	public Vector3[] moveDirections;
	public bool isRepetitive;
	public bool isWhite = true;
	public bool isPawn = false;
	public bool isKing = false;
	public bool isAlive = true;
	public bool wasMoved = false;


	void Start () 
	{
	}
	
	void Update () 
	{
	
	}

	void OnMouseDown()
	{
		if (!(Game.isWhitesTurn ^ isWhite) && !isPawn)
		{
			AvoidCheck( GetLegalMoves());
			//List<Field> legalmoves = GetLegalMoves();
			//AvoidCheck(legalmoves);

		}
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

				wasMoved = true;

				Field currentField = Board.board[(int)transform.position.x, (int)transform.position.y].GetComponent<Field>();
				currentField.HoldedPiece = null;

				transform.position = target;

				if (targetField.HoldedPiece != null)
					if (targetField.HoldedPiece.GetComponent<Piece>().isWhite ^ isWhite)
					{
						targetField.HoldedPiece.GetComponent<Piece>().isAlive = false; //I have no idea why it must be here but it works. Otherwise holdedPiece changes back to what it was
						Destroy(targetField.HoldedPiece);
					}
				targetField.HoldedPiece = gameObject;

				if (isPawn)
				{
					if (transform.position.y == GetComponent<Pawn>().promotionGoal)
						GetComponent<Pawn>().canBePromoted = true;
					else
						Game.canSwitchTurns = true;
				}
				else
				{
					//Game.NextTurn();
					Game.canSwitchTurns = true;
				}

			}
			else
				Debug.Log("Illegal");
		}
		else
			Debug.Log("No movement");

		Game.CleanBoard();
	}

	public List<Field> GetLegalMoves()
	{
		List<Field> legalMoves = new List<Field>();
		foreach (Vector3 direction in moveDirections)
		{
			Vector3 actualPosition = transform.position;
			actualPosition += direction;
			while (Game.isInRange(actualPosition))
			{
				Field field = Board.board[(int)actualPosition.x, (int)actualPosition.y].GetComponent<Field>();

				if (field.HoldedPiece == null)
				{
					//field.isLegal = true;
					legalMoves.Add(field);
				}
				else
				{
					if (field.HoldedPiece.GetComponent<Piece>().isWhite ^ isWhite)
					{
						//field.isLegal = true;
						legalMoves.Add(field);
					}
					break;
				}

				if (!isRepetitive)
					break;

				actualPosition += direction;
			}
		}
		return legalMoves;
	}

	public List<GameObject> FindAttackers()
	{
		List<GameObject> attackers = new List<GameObject>();
		
		Vector3[] attackDirections = new Vector3[] {
			new Vector3(1,0,0),
			new Vector3(1,-1,0),
			new Vector3(0,-1,0),
			new Vector3(-1,-1,0),
			new Vector3(-1,0,0),
			new Vector3(-1,1,0),
			new Vector3(0,1,0),
			new Vector3(1,1,0),
			new Vector3(1,2,0),
			new Vector3(2,1,0),
			new Vector3(2,-1,0),
			new Vector3(1,-2,0),
			new Vector3(-1,-2,0),
			new Vector3(-2,-1,0),
			new Vector3(-2,1,0),
			new Vector3(-1,2,0)
													};

		foreach(Vector3 direction in attackDirections)
		{
			Vector3 consideredPosition = transform.position + direction;
			Field ownedField = Board.board[(int)transform.position.x, (int)transform.position.y].GetComponent<Field>();
			List<Field> attackingFields = new List<Field>();
			while(Game.isInRange(consideredPosition))
			{
				Field consideredField = Board.board[(int)consideredPosition.x, (int)consideredPosition.y].GetComponent<Field>();
				if( consideredField.HoldedPiece != null)
				{
					Piece consideredPiece = consideredField.HoldedPiece.GetComponent<Piece>();
					if (consideredPiece.isWhite ^ isWhite)
					{
						if (!consideredPiece.isPawn)
							attackingFields = consideredPiece.GetLegalMoves();
						else
						{
							consideredField.HoldedPiece.GetComponent<Pawn>().GetLegalMoves(); //apply to pawns
							attackingFields = consideredField.HoldedPiece.GetComponent<Pawn>().legalMoves;
						}

						if (attackingFields.Contains(ownedField))
						{
							attackers.Add(consideredField.HoldedPiece);
						}
					}
					break;
				}
				if (Mathf.Abs(direction.x) == 2 || Mathf.Abs(direction.y) == 2)
					break;
				consideredPosition += direction;
			}
		}
		return attackers;
	}

	public int AvoidCheck(List<Field> targetFields)
	{
		Field ownedField = Board.board[(int)transform.position.x, (int)transform.position.y].GetComponent<Field>();
		int availableMoves = 0;
		Vector3 originalPosition = transform.position;

        foreach (Field consideredField in targetFields)
		{
			GameObject capturedPiece = consideredField.HoldedPiece;

			consideredField.HoldedPiece = gameObject;
			ownedField.HoldedPiece = null;
			if (isKing)
				transform.position = new Vector3(consideredField.transform.position.x, consideredField.transform.position.y, 0);

			if (!Game.CheckIfCheck())
			{
				consideredField.isLegal = true;
				++availableMoves;
			}

			consideredField.HoldedPiece = capturedPiece;
			ownedField.HoldedPiece = gameObject;
			transform.position = originalPosition;

		}
		return availableMoves;
	}
}
   