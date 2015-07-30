using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour 
{
    public static GameObject[,] board = new GameObject[8,8];

	void Start () 
	{

	}

	void Update () 
	{
		Destroy(board[0, 0]);
    }
}