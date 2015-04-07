using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerTrigger : MonoBehaviour {

	public Text feedback;
	int playerNum;
	public GameObject script;
	public Player player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*
	void OnTriggerEnter(Collider col)
	{
		//Debug.Log(gameObject.name + " Has hit " + col.gameObject.name);
		playerNum = script.GetComponent<Prototype1_ConnectAndJoin>().currentPlayer;
		if(playerNum == 1 && col.name == "Player2")
		{
			Destroy(col.gameObject);
			feedback.text = "You have killed " + col.gameObject.name;
		}
		if(playerNum == 2 && col.name == "Player1")
		{
			Destroy(col.gameObject);
			feedback.text = "You have killed " + col.gameObject.name;
		}
	} 
	*/
}
