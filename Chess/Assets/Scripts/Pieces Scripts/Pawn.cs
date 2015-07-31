using UnityEngine;
using System.Collections;

public class Pawn : MonoBehaviour 
{
	public bool wasMoved = false;

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
					field = Board.board[(int)actualPosition.x, (int)actualPosition.y].GetComponent<Field>();
					if (field.HoldedPiece == null)
						field.isLegal = true;
				}
            }
		}

		foreach (Vector3 direction in captureVector)
		{
			actualPosition = transform.position;
			actualPosition += direction;
			if(Game.isInRange(actualPosition))
			{
				Field field = Board.board[(int)actualPosition.x, (int)actualPosition.y].GetComponent<Field>();
				if (field.isCapturedByOpponent)
					field.isLegal = true;

			}
		}
	}

}