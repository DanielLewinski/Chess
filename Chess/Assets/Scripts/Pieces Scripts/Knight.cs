using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour 
{
	Vector3[] directions =
	{
		new Vector3(1,2,0),
		new Vector3(2,1,0),
		new Vector3(2,-1,0),
		new Vector3(1,-2,0),
		new Vector3(-1,-2,0),
		new Vector3(-2,-1,0),
		new Vector3(-2,1,0),
		new Vector3(-1,2,0)
	};
	Piece piece;

	void Start () 
	{
		piece = gameObject.GetComponent<Piece>();
		piece.directions = directions;
		piece.isRepetitive = false;
	}
	
	void Update () 
	{
	
	}
}