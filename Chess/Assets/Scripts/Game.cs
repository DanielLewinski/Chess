using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public static bool isWhitesTurn = true;
	public static uint turnsTaken = 0;
	public static uint turnOfLastCapture = 1; //or pawn movement
	public static bool isPlayerInCheck = false;
	public static bool canSwitchTurns = false;
	public static string[] players = new string[2] { "Biały", "Czarny" };
	public static string message = "";
	public static bool isThisTheEnd = false;
	public static bool isOnline = false;


	void Start () 
	{
		isWhitesTurn = true;
		turnsTaken = 0;
		turnOfLastCapture = 1;
		isPlayerInCheck = false;
		canSwitchTurns = false;
		isThisTheEnd = false;
	}
	
	void Update () 
	{
		if (canSwitchTurns)
			NextTurn();
		if (isThisTheEnd)
			StartCoroutine(TerminateGame());
	}

	public static void NextTurn()
	{
		canSwitchTurns = false;
		++turnsTaken;
		isWhitesTurn = !isWhitesTurn;
		isPlayerInCheck = false;

		

		CheckIfGameOver();
		

		Castling.LoadRow();

		Camera.main.transform.Rotate(new Vector3(0, 0, 180));
		GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
		foreach (GameObject piece in pieces)
			piece.transform.Rotate(new Vector3(0, 0, 180));
		SwapNames();

		
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
			//Debug.Log("SingleCheck");
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
		FiftyMoveRule();
		CheckMate();
	}

	static void FiftyMoveRule()
	{
		if (turnsTaken - turnOfLastCapture >= 50)
			print("Draw");
	}

	static void CheckMate()
	{
		GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
		bool isAnyMovePossible = false;

		foreach (GameObject piece in pieces)
		{
			if (!(isWhitesTurn ^ piece.GetComponent<Piece>().isWhite) && piece.GetComponent<Piece>().isAlive)
			{
				Piece consideredPiece = piece.GetComponent<Piece>();
				if (consideredPiece.isPawn)
				{
					consideredPiece.GetComponent<Pawn>().GetLegalMoves();
					if (consideredPiece.GetComponent<Pawn>().AvoidCheck(consideredPiece.GetComponent<Pawn>().legalMoves) > 0)
					{
						isAnyMovePossible = true;
						CleanBoard();
						break;
					}
				}
				else if (consideredPiece.AvoidCheck(consideredPiece.GetLegalMoves()) > 0)
				{
					isAnyMovePossible = true;
					CleanBoard();
					break;

				}
			}
		}
		if (CheckIfCheck())
			message = "Check";

		if (!isAnyMovePossible)
		{
			if (isPlayerInCheck)
				message = "Szach-Mat";
			else
				message = "Pat";

			isThisTheEnd = true;
		}
	}

	void OnGUI()
	{
		GUIStyle nameStyle = new GUIStyle(GUI.skin.GetStyle("label"));
		nameStyle.fontSize = 25;
		GUI.Label(new Rect(400, 550, 200, 40), players[0], nameStyle);
		GUI.Label(new Rect(400, 10, 200, 40), players[1], nameStyle);
		GUI.Label(new Rect(750, 200, 200, 100), message, nameStyle);

		if (message != "")
			StartCoroutine(Fading());
    }

	static IEnumerator Fading()
	{
		yield return new WaitForSeconds(1);
		message = "";
	}

	static IEnumerator TerminateGame()
	{
		yield return new WaitForSeconds(1);
		Application.LoadLevel("Menu");
	}

	static void SwapNames()
	{
		string temporaryString = players[0];
		players[0] = players[1];
		players[1] = temporaryString;
	}

	
}