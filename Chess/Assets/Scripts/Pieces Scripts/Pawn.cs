using UnityEngine;
using System.Collections;

public class Pawn : MonoBehaviour 
{
	public bool wasMoved = false;

	//for En Passant move
	public uint? turnOfDoublePush = null;
	public bool canBePassed = false;
	GameObject passablePawn;

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
		GetLegalMoves();
	}

	void GetLegalMoves()
	{
		Vector3 actualPosition = transform.position;
		actualPosition += moveVector;
		if(Game.isInRange(actualPosition))
		{
			Field field = Board.board[(int)actualPosition.x, (int)actualPosition.y].GetComponent<Field>();
			if (field.HoldedPiece == null)
			{
				field.isLegal = true;
				if (!wasMoved)
				{
					actualPosition += moveVector;
					if (Game.isInRange(actualPosition))
					{
						field = Board.board[(int)actualPosition.x, (int)actualPosition.y].GetComponent<Field>();
						if (field.HoldedPiece == null)
							field.isLegal = true;
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
					if (field.isCapturedByOpponent)
						field.isLegal = true;

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
		Debug.Log(passablePosition.x + " " + passablePosition.y);
		Field passableField = Board.board[(int)passablePosition.x, (int)passablePosition.y].GetComponent<Field>();
		if (passableField.HoldedPiece != null)
			if (passableField.HoldedPiece.GetComponent<Piece>().isPawn)
				if (passableField.HoldedPiece.GetComponent<Pawn>().canBePassed)
				{
					passablePawn = passableField.HoldedPiece;
					Board.board[(int)targetPosition.x, (int)targetPosition.y].GetComponent<Field>().isLegal = true; //always vacant
					return true;
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
				Board.board[(int)passablePawn.transform.position.x, (int)passablePawn.transform.position.y].GetComponent<Field>().Capture();
			}
		}
		wasMoved = true;
	}
}