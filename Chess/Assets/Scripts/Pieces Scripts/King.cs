using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class King : MonoBehaviour 
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
		piece.isRepetitive = false;
		piece.isKing = true;
	}

	void Update () 
	{
	
	}
}