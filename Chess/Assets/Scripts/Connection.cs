using UnityEngine;
using System.Collections;

public class Connection : MonoBehaviour
{

	private string remoteIp = "127.0.0.1";
	private int remotePort = 25000;
	private int listenPort = 25000;
	private bool useNAT = false;

	public GameObject whiteSet;
	public GameObject blackSet;
	public GameObject board;
	public bool canCreateBoard = false;

	// Use this for initialization 
	void Start()
	{

	}

	// Update is called once per frame 
	void Update()
	{
		if(GameObject.FindGameObjectsWithTag("Set").Length == 2 && canCreateBoard)
		{
			canCreateBoard = false;
			Network.Instantiate(board, new Vector3(0, 0, 1), Quaternion.identity, 0);
		}
	}

	void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Disconnected)
		{ //Not connected

			if (GUI.Button(new Rect(10, 10, 100, 30), "Connect"))
			{
				//Connecting to the server
				Debug.Log(remoteIp);
				Debug.Log(remotePort);
				NetworkConnectionError ne = Network.Connect(remoteIp, remotePort);
				Debug.Log(ne.ToString());
				Debug.Log(Network.peerType);
			}
			if (GUI.Button(new Rect(10, 50, 100, 30), "Start Server"))
			{
				//Creating server
				Network.InitializeServer(32, listenPort, useNAT);

			}

			remoteIp = GUI.TextField(new Rect(120, 10, 100, 20), remoteIp);
			remotePort = int.Parse(GUI.TextField(new Rect(230, 10, 40, 20), remotePort.ToString()));
		}
		else
		{
			//Getting your ip address and port
			string ipaddress = Network.player.ipAddress;
			string port = Network.player.port.ToString();

			GUI.Label(new Rect(140, 20, 250, 40), "IP Address:" + ipaddress + ":" + port);
			if (GUI.Button(new Rect(10, 10, 100, 50), "Disconnect"))
			{
				//Disconnect from the server
				Network.Disconnect(200);
			}
		}

	}

	void OnConnectedToServer()
	{
		Debug.Log("Connected to Server");
		Network.Instantiate(blackSet, new Vector3(0, 0, 0), Quaternion.identity, 0);
	}

	void OnPlayerConnected()
	{
		Debug.Log("Player connected");
		Network.Instantiate(whiteSet, new Vector3(0, 0, 0), Quaternion.identity, 0);
		canCreateBoard = true;
	}

	void OnServerInitialized()
	{
		Debug.Log("Server");
	}

	
}