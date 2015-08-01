using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public static bool isWhitesTurn = true;
	public static uint turnsTaken = 0;
	public static bool isPlayerInCheck = false;

	void Start () 
	{
		CheckIfCheck();
	}
	
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			foreach (GameObject piece in GameObject.Find("KingWhite").GetComponent<Piece>().FindAttackers())
				Destroy(piece);
		}
	}

	public static void NextTurn()
	{
		++turnsTaken;
		isWhitesTurn = !isWhitesTurn;
		isPlayerInCheck = false;

		CheckIfGameOver();

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

	public static bool CheckIfCheck()
	{
		Piece king;
		if (isWhitesTurn)
			king = GameObject.Find("KingWhite").GetComponent<Piece>();
		else
			king = GameObject.Find("KingBlack").GetComponent<Piece>();

		List<GameObject> attackers = king.FindAttackers();
	
		if (attackers.Count > 0)
		{
			isPlayerInCheck = true;
			Debug.Log("SingleCheck");
			return true;
		}
		else
		{
			isPlayerInCheck = false;
			return false;
		}
	}

	static void CheckIfGameOver()
	{
		GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
		bool isAnyMovePossible = false;
		foreach(GameObject piece in pieces)
		{
			if (!(isWhitesTurn ^ piece.GetComponent<Piece>().isWhite))
			{
				Piece consideredPiece = piece.GetComponent<Piece>();
				if(consideredPiece.isPawn)
				{
					consideredPiece.GetComponent<Pawn>().GetLegalMoves();
					if (consideredPiece.GetComponent<Pawn>().AvoidCheck(consideredPiece.GetComponent<Pawn>().legalMoves) > 0)
					{
						isAnyMovePossible = true;
						break;
					}
				}
				else if(consideredPiece.AvoidCheck( consideredPiece.GetLegalMoves()) > 0)
				{
					isAnyMovePossible = true;
					break;
				}
			}   
		}
		CheckIfCheck();
		if (!isAnyMovePossible)
		{
			if (isPlayerInCheck)
				print("CheckMate"); //TODO: Do this on GUI
			else
				print("StaleMate");
		}
	}
}