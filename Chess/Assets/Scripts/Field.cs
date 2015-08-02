using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour
{
	public Material defaultMaterial;
	public Material legalMaterial;

	public bool isLegal = false;

	public GameObject HoldedPiece = null;

	Renderer materialRenderer;

    void Start()
    {
        Board.board[(int)transform.position.x , (int)transform.parent.position.y] = gameObject;
		materialRenderer = gameObject.GetComponent<Renderer>();
		materialRenderer.material = defaultMaterial;
		InitVacancy();
    }

	void Update ()
    {
		if (isLegal)
			materialRenderer.material = legalMaterial;
		else
			materialRenderer.material = defaultMaterial;
	
	}

	public void InitVacancy()
	{
		GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
		foreach (GameObject piece in pieces)
			if (piece.transform.position.x == transform.position.x && piece.transform.position.y == transform.position.y)
			{
				HoldedPiece = piece;
				break;
			}
	}

}
