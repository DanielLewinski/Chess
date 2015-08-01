using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pawn : MonoBehaviour 
{
	public bool wasMoved = false;

	//for En Passant move
	public uint? turnOfDoublePush = null;
	public bool canBePassed = false;
	public GameObject passablePawn;
	public List<Field> legalMoves = new List<Field>();

	Vector3 moveVector;
	Vector3[] captureVector = new Vector3[2];
	int promotionGoal;


	void Start () 
	{
		if(GetComponent<Piece>().isWhite)
		{
			moveVector = new Vector3(0, 1, 0);
			captureVector[0] = new Vector3(1, 1, 0);
			captureVector[1] = new Vector3(-1, 1, 0);
			promotionGoal = 7;
		}
		else
		{
			moveVector = new Vector3(0, -1, 0);
			captureVector[0] = new Vector3(1, -1, 0);
			captureVector[1] = new Vector3(-1, -1, 0);
			promotionGoal = 0;
		}
		GetComponent<Piece>().isPawn = true;
	}
	
	void Update () 
	{
		//consider a function doing that
		if (turnOfDoublePush != null)
		{
			if (Game.turnsTaken - turnOfDoublePush < 2)
				canBePassed = true;
			else
			{
				canBePassed = false;
				turnOfDoublePush = null;
			}
		}
	}

	void OnMouseDown()
	{
		if (!(Game.isWhitesTurn ^ GetComponent<Piece>().isWhite))
		{
			GetLegalMoves();
			AvoidCheck(legalMoves);
		}
	}

	public void GetLegalMoves()
	{
		legalMoves.Clear();
		Vector3 actualPosition = transform.position;
		actualPosition += moveVector;
		if(Game.isInRange(actualPosition))
		{
			Field field = Board.board[(int)actualPosition.x, (int)actualPosition.y].GetComponent<Field>();
			if (field.HoldedPiece == null)
			{
				legalMoves.Add(field);
				if (!wasMoved)
				{
					actualPosition += moveVector;
					if (Game.isInRange(actualPosition))
					{
						field = Board.board[(int)actualPosition.x, (int)actualPosition.y].GetComponent<Field>();
						if (field.HoldedPiece == null)
							legalMoves.Add(field);
					}
				}
            }
		}

		foreach (Vector3 direction in captureVector)
		{
			actualPosition = transform.position;
			actualPosition += direction;

			if(!CheckEnPassant(actualPosition))
				if(Game.isInRange(actualPosition))
				{
					Field field = Board.board[(int)actualPosition.x, (int)actualPosition.y].GetComponent<Field>();
					if(field.HoldedPiece != null)
						if (field.HoldedPiece.GetComponent<Piece>().isWhite ^ GetComponent<Piece>().isWhite)
							legalMoves.Add(field);

				}
		}
	}

	bool CheckEnPassant(Vector3 targetPosition)
	{
		Vector3 passablePosition;
		if (GetComponent<Piece>().isWhite)
			passablePosition = new Vector3(targetPosition.x, targetPosition.y - 1, 0);
		else
			passablePosition = new Vector3(targetPosition.x, targetPosition.y + 1, 0);
		if (Game.isInRange(passablePosition))
		{
			Field passableField = Board.board[(int)passablePosition.x, (int)passablePosition.y].GetComponent<Field>();
			if (passableField.HoldedPiece != null)
				if (passableField.HoldedPiece.GetComponent<Piece>().isPawn)
					if (passableField.HoldedPiece.GetComponent<Pawn>().canBePassed)
					{
						passablePawn = passableField.HoldedPiece;
						legalMoves.Add(Board.board[(int)targetPosition.x, (int)targetPosition.y].GetComponent<Field>());
                        return true;
					}
		}
		return false;
	}

	public void UpdatePawnStatus(Vector3 targetPosition)
	{
		if(!wasMoved && (Mathf.Abs(targetPosition.y - transform.position.y) == 2))
		{
			canBePassed = true;
			turnOfDoublePush = Game.turnsTaken;
		}

		if(passablePawn != null)
		{
			if( Mathf.Abs(targetPosition.y - passablePawn.transform.position.y) == 1)
			{
				Destroy(passablePawn);
			}
		}
		wasMoved = true;
	}

	public int AvoidCheck(List<Field> targetFields)
	{
		Field ownedField = Board.board[(int)transform.position.x, (int)transform.position.y].GetComponent<Field>();
		int availableMoves = 0;
		Vector3 originalPosition = transform.position;

		Field passableField;

		foreach (Field consideredField in targetFields)
		{
			GameObject capturedPiece = consideredField.HoldedPiece;

			consideredField.HoldedPiece = gameObject;
			ownedField.HoldedPiece = null;

			if (passablePawn != null)
				if (Mathf.Abs(consideredField.transform.position.y - passablePawn.transform.position.y) == 1)
				{
					passableField = Board.board[(int)passablePawn.transform.position.x, (int)passablePawn.transform.position.y].GetComponent<Field>();
					passableField.HoldedPiece = null;

					
				}
			print(transform.position.x + " " + transform.position.y);
			if (!Game.CheckIfCheck())
			{
				consideredField.isLegal = true;
				++availableMoves;
			}

			consideredField.HoldedPiece = capturedPiece;
			ownedField.HoldedPiece = gameObject;
			transform.position = originalPosition;

			if (passablePawn != null)
				if (Mathf.Abs(consideredField.transform.position.y - passablePawn.transform.position.y) == 1)
				{
					passableField = Board.board[(int)passablePawn.transform.position.x, (int)passablePawn.transform.position.y].GetComponent<Field>();
					passableField.HoldedPiece = passablePawn;


				}
			
		}
		return availableMoves;
	}
}