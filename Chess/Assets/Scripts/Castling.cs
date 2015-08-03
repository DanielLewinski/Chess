using UnityEngine;
using System.Collections;


//Every rook has to be named Rook"InsertColor" for this to work

public class Castling : MonoBehaviour 
{
	static bool isQueensideAllowed = false;
	static bool isKingsideAllowed = false;
	static string suffix;

	static uint rowIndex;
	static Field[] row = new Field[8];

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}

	public static void LoadRow()
	{
		if (Game.isWhitesTurn)
		{
			rowIndex = 0;
			suffix = "White";
		}
		else
		{
			rowIndex = 7;
			suffix = "Black";
		}

		for (uint iterator = 0; iterator < 8; ++iterator)
			row[iterator] = Board.board[iterator, rowIndex].GetComponent<Field>();

		isQueensideAllowed = CheckCastling(new uint[5] {4,3,2,1,0 });
		isKingsideAllowed = CheckCastling(new uint[4] { 4, 5, 6, 7 });
	}

	static bool CheckCastling(uint[] positions) //loads fields, starting from king, ending on rook
	{
		if (row[positions[0]].HoldedPiece == null || row[positions[positions.Length - 1]].HoldedPiece == null)
			return false;
		else
		{
			if (row[positions[0]].HoldedPiece.name != ("King" + suffix) || row[positions[positions.Length - 1]].HoldedPiece.name != ("Rook" + suffix))
				return false;
			else
			{
				if (row[positions[0]].HoldedPiece.GetComponent<Piece>().wasMoved || row[positions[positions.Length - 1]].HoldedPiece.GetComponent<Piece>().wasMoved)
					return false;
				else
				{
					return areFieldsAvailable(positions);
				}
			}
		}
	}

	static bool areFieldsAvailable(uint[] positions)
	{
		if(Game.CheckIfCheck())
			return false;
		else
		{
			for(uint iterator = 1; iterator < positions.Length - 1; ++iterator)
			{
				if (row[positions[iterator]].HoldedPiece != null)
					return false;
				else if(iterator < 3)
				{
					Vector3 originalPosition = row[positions[0]].HoldedPiece.transform.position;
					row[positions[0]].HoldedPiece.transform.position = row[positions[iterator]].transform.position;
					if (Game.CheckIfCheck())
					{
						row[positions[0]].HoldedPiece.transform.position = originalPosition;
						return false;
					}
					row[positions[0]].HoldedPiece.transform.position = originalPosition;
				}
			}
			return true;
		}
	}

	void OnGUI()
	{
		GUIStyle buttonStyle = new GUIStyle(GUI.skin.GetStyle("button"));
		buttonStyle.fontSize = 18;
		if (isQueensideAllowed)
			if (GUI.Button(new Rect(750, 50, 150, 50), "Roszada długa", buttonStyle))
				DoCastling(0,3,2);

		if (isKingsideAllowed)
			if (GUI.Button(new Rect(750, 100, 150, 50), "Roszada krótka", buttonStyle))
				DoCastling(7, 5, 6);

		if (GUI.Button(new Rect(750, 500, 150, 50), "Koniec gry", buttonStyle))
		{
			Game.message = Game.players[0] + " poddał się";
			Game.isThisTheEnd = true;
		}
    }

	void DoCastling(uint rookBegin, uint rookEnd, uint kingEnd)
	{
		row[4].HoldedPiece.transform.position = new Vector3(row[kingEnd].transform.position.x, row[kingEnd].transform.position.y, 0);
		row[kingEnd].HoldedPiece = row[4].HoldedPiece;
		row[4].HoldedPiece = null;

		row[rookBegin].HoldedPiece.transform.position = new Vector3(row[rookEnd].transform.position.x, row[rookEnd].transform.position.y, 0);
		row[rookEnd].HoldedPiece = row[rookBegin].HoldedPiece;
		row[rookBegin].HoldedPiece = null;

		Game.canSwitchTurns = true;
	}
}