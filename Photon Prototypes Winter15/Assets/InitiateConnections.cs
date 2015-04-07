using UnityEngine;
using System.Collections;
using System.Collections.Generic; 	

public class InitiateConnections : Photon.MonoBehaviour {

	public Camera player1Cam;
	public Camera player2Cam;
	public PhotonView photonView;
	public HandOfCards p1hand,p2hand;
	public GridMatrix grid;
	public bool myTurn;
	Player player;
	// Use this for initialization
	Queue<string> messages;
	void Start () 
	{
		photonView = GetComponent<PhotonView> ();
		messages = new Queue<string> (4);
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

	}

	void OnJoinedLobby()
	{
		RoomOptions ro = new RoomOptions (){isVisible = true, maxPlayers = 2};
		PhotonNetwork.JoinOrCreateRoom ("ProtoTest", ro, TypedLobby.Default);
		Debug.Log("You are in the lobby");
	}

	 public void OnJoinedRoom()
    {
		if(!PhotonNetwork.player.name.Contains("Player"))
		{
			PhotonNetwork.player.name = "Player " + PhotonNetwork.room.playerCount ;
			Debug.Log("Initiated " + PhotonNetwork.player.name);
		}
		else
		{
			Debug.Log("other player joined");
		}
		photonView.RPC("SomeoneJoined", PhotonTargets.All, PhotonNetwork.player.name);

	}

	 public virtual void OnConnectedToMaster()
    {
        if (PhotonNetwork.networkingPeer.AvailableRegions != null) Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
      //  Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
       // PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 4 }, null);
		Debug.Log("Failed to join room");
    }

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    [RPC]
    void SomeoneJoined(string owner)
    {
    	if(owner == PhotonNetwork.player.name) // if I ran this
    	{
    		Debug.Log("I have joined the Game!");
    		
    		//Debug.Log(grid.player);
    		if(owner.Contains("2")) // I am player 2
    		{
    			myTurn = false;
    			player = player2Cam.transform.parent.gameObject.AddComponent<Player>();
    			player.setId(2);
    			player.myTurn = myTurn;
    			player.hand = player.transform.gameObject.GetComponent<HandOfCards>();
    			player.hand.cam = player2Cam;
    			player.hand.player = player;
    			player.hand.DrawCard("card");
    			player.cam = player2Cam;
    			p2hand.player = player;
    			p1hand.enabled = false;
    			player.gridM = grid;
    			//grid.player1 = player;
    			grid.startup();
    			//grid.initPanels();
    			player1Cam.gameObject.SetActive(false);
    			player2Cam.gameObject.SetActive(true);
    		}
    		else // I am player 1
    		{
    			myTurn = true; 			
    			player = player1Cam.transform.parent.gameObject.AddComponent<Player>();
    			player.setId(1);
    			player.myTurn = myTurn;
    			player.cam = player1Cam;
    			player.gridM = grid;
    			p1hand.player = player;
    			p2hand.enabled = false;
    		} 

    		grid.SetPlayer(player);
    	}
    	else // if another player ran this
    	{
    		player.hand.DrawCard("card");
    		Debug.Log(owner+ " has joined the game");
    		grid.startup();
    		grid.initPanels();
    	}

    }

 	[RPC]
 	void SwitchTurns()
 	{
 		myTurn = !myTurn;
 		if(myTurn)
 		{
 		player.hand.DrawCard("card");
 		Debug.Log("its my turn!");
 		grid.playersTurn = true;
 		}
 	} 

 	public void callFromTurnButton()
	{
		if(myTurn)
		{
		photonView.RPC ("SwitchTurns", PhotonTargets.Others);
		myTurn = false;
		grid.playersTurn = false;
		}
	}  

}
