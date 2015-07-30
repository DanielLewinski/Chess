using UnityEngine;
using System.Collections;

public class Rook : MonoBehaviour 
{
	void Start () 
	{
	
	}
	
	void Update () 
	{
		GetLegalMoves();
	
	}

	void GetLegalMoves()
	{
		Vector3[] directions = { new Vector3( 1, 0, 0 ), new Vector3( -1, 0, 0 ), new Vector3( 0, 1, 0 ), new Vector3( 0, -1, 0 ) };
		foreach(Vector3 direction in directions)
		{
			Vector3 actualPosition = transform.position;
			actualPosition += direction;
			while(isInRange(actualPosition))
			{
				Field field = Board.board[(int)actualPosition.x, (int)actualPosition.y].GetComponent<Field>();
				if (field.isVacant)
					field.isLegal = true;
			}
		}
	}

	bool isInRange(Vector3 position)
	{
		return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
	}
}