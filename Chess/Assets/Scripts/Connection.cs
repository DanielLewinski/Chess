using UnityEngine;
using System.Collections;

public class Connection : MonoBehaviour
{

	string remoteIp = "127.0.0.1";
	int remotePort = 25000;
	int listenPort = 25000;
	bool useNAT = false;

	public GameObject whiteSet;
	public GameObject blackSet;
	public GameObject board;
	public bool canCreateBoard = false;

	void Start()
	{

	}

	void Update()
	{
		if(GameObject.FindGameObjectsWithTag("Set").Length == 2 && canCreateBoard)
		{
			canCreateBoard = false;
			Network.Instantiate(board, new Vector3(0, 0, 1), Quaternion.identity, 0);
			Game.RotateBoard();
		}
	}

	void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Disconnected)
		{ 
			if (GUI.Button(new Rect(10, 10, 100, 30), "Dołącz"))
			{
				NetworkConnectionError ne = Network.Connect(remoteIp, remotePort);
			}
			if (GUI.Button(new Rect(10, 50, 100, 30), "Hostuj serwer"))
			{
				Network.InitializeServer(2, listenPort, useNAT);
			}

			remoteIp = GUI.TextField(new Rect(120, 10, 100, 20), remoteIp);
			remotePort = int.Parse(GUI.TextField(new Rect(230, 10, 40, 20), remotePort.ToString()));
		}
		else
		{
			string ipaddress = Network.player.ipAddress;
			string port = Network.player.port.ToString();

			GUI.Label(new Rect(140, 20, 250, 40), "Adres IP:" + ipaddress + ":" + port);
			if (GUI.Button(new Rect(10, 10, 100, 50), "Rozłącz"))
			{
				Network.Disconnect(200);
				Game.isThisTheEnd = true;
			}
		}

	}

	void OnConnectedToServer()
	{
		Game.isOnline = true;
		Network.Instantiate(blackSet, new Vector3(0, 0, 0), Quaternion.identity, 0);
		canCreateBoard = true;
	}

	void OnPlayerConnected()
	{
		Network.Instantiate(whiteSet, new Vector3(0, 0, 0), Quaternion.identity, 0);	
	}

	void OnPlayerDisconnected()
	{
		Game.isThisTheEnd = true;
		Network.Disconnect(200);
	}

	void OnDisconnectedFromServer()
	{
		Game.isThisTheEnd = true;
	}

	void OnServerInitialized()
	{
		Game.isOnline = true;
	}

	
}