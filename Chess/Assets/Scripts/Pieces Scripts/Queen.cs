using UnityEngine;
using System.Collections;

public class Queen : MonoBehaviour 
{
	Vector3[] directions =
	{
		new Vector3(1, 0, 0),
		new Vector3(-1, 0, 0),
		new Vector3(0, 1, 0),
		new Vector3(0, -1, 0),
		new Vector3(1,1,0),
		new Vector3(1,-1,0),
		new Vector3(-1,-1,0),
		new Vector3(-1,1,0)
	};
	Piece piece;

	void Start()
	{
		piece = gameObject.GetComponent<Piece>();
		piece.moveDirections = directions;
		piece.isRepetitive = true;
	}

	void Update () 
	{
	
	}
}