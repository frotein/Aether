using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdjacentGridMoves : MonoBehaviour {

	// Use this for initialization

	public Transform player1;
	public Transform player2;
	public GFGrid grid;
	public Text feedback;
	public GameObject script;
	int curPlayer;
	Vector3 movePosition;
	PhotonView photonView;
	Transform currentPlayer;


	void Start () {
		photonView = GetComponent<PhotonView> ();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*
	void OnMouseDown(){

		curPlayer = script.GetComponent<InitiateConnections>().currentPlayer;
		//Debug.Log("Current Player is Player" + curPlayer);
		if (curPlayer == 1) 
		{
			IsAdjacent(grid, transform.position, player1.position);
		} else if (curPlayer == 2) 
		{
			IsAdjacent(grid, transform.position, player2.position);
		}
		
	}
	

	void IsAdjacent(GFGrid grid, Vector3 gridPos, Vector3 playPos){
		//convert to Grid Space first

		if (curPlayer == 2) 
		{
			currentPlayer = player2;
		} else  
		{
			currentPlayer = player1;
		}


		Vector3 gridPosition = grid.WorldToGrid(gridPos);//the light we want to test
		Vector3 playerPosition = grid.WorldToGrid(playPos);//the light that was pressed
		bool isAdjacent = Mathf.Abs(gridPosition.x-playerPosition.x) <= 1.025f && Mathf.Abs(gridPosition.y-playerPosition.y) <= 1.025f;
		bool isDiagonal = 0.125f <= Mathf.Abs(gridPosition.x-playerPosition.x) && 0.125f <= Mathf.Abs(gridPosition.y-playerPosition.y) && isAdjacent;
		if ( isAdjacent && !isDiagonal )
		{
			feedback.text = "Player moves to adjacent tile.";
			movePosition = transform.position;
			photonView.RPC ("MovePlayer", PhotonTargets.All);//, currentPlayer
			//currentPlayer.position = transform.position;
		} else if (isDiagonal)
		{
			feedback.text = "You clicked a diagonal tile.";
			movePosition = transform.position;
			photonView.RPC ("MovePlayer", PhotonTargets.All);
			//currentPlayer.position = transform.position;
		} else 
		{
			feedback.text = "You clicked a tile that is not adjacent or diagonal.";
		}
	}

	[RPC]

	void MovePlayer()
	{
		//Debug.Log("Moving");
		currentPlayer.position = movePosition;
	}

	*/
	

}