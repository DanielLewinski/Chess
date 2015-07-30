using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour
{
	public Material defaultMaterial;
	public Material legalMaterial;

	public bool isVacant = true;
	public bool isLegal = false;

	Renderer materialRenderer;

    void Start()
    {
        Board.board[(int)transform.position.x , (int)transform.parent.position.y] = gameObject;
		materialRenderer = gameObject.GetComponent<Renderer>();
		materialRenderer.material = defaultMaterial;
    }

	void Update ()
    {
		if (isLegal)
			materialRenderer.material = legalMaterial;
		else
			materialRenderer.material = defaultMaterial;
	
	}
}
