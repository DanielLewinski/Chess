using UnityEngine;
using System.Collections;

public class Bishop : MonoBehaviour 
{
	Vector3[] directions =
	{
		new Vector3(1,1,0),
		new Vector3(1,-1,0),
		new Vector3(-1,-1,0),
		new Vector3(-1,1,0)
	};
	Piece piece;

	void Start()
	{
		piece = gameObject.GetComponent<Piece>();
		piece.directions = directions;
		piece.isRepetitive = true;
	}

	void Update () 
	{
	
	}
}