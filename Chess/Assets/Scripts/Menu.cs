using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	bool wasStartClicked = false;

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}

	void OnGUI()
	{
		GUIStyle titleStyle = new GUIStyle(GUI.skin.GetStyle("label"));
		titleStyle.fontSize = 70;
		GUI.Label(new Rect(350, 100, 250, 200), "Szachy", titleStyle);
		
		GUIStyle buttonStyle = new GUIStyle(GUI.skin.GetStyle("button"));
		
		
		if(wasStartClicked)
		{
			buttonStyle.fontSize = 30;
			Game.players[0] = GUI.TextField(new Rect(350, 250, 200, 50), Game.players[0], 10, buttonStyle);
			Game.players[1] = GUI.TextField(new Rect(350, 350, 200, 50), Game.players[1], 10, buttonStyle);
			if (GUI.Button(new Rect(570, 300, 70, 60), "OK", buttonStyle))
				Application.LoadLevel("Main");
		}
		else
		{
			buttonStyle.fontSize = 50;
			if (GUI.Button(new Rect(375, 300, 200, 100), "Start", buttonStyle))
				wasStartClicked = true;
		}
	}
}