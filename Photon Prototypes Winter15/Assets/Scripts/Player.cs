using UnityEngine;
using System.Collections;

// The players various stats and attributes
public class Player : MonoBehaviour
{
	// what number player you are
	int id;
	// Playeers health, starts at 20
	public int health;
	public bool myTurn;
	

	// percentage from 0% - 100%, increases 10% per turn. once full focus increaes bt 1, gain 1 aether per turn;
	public int connection;

	// Aether, gain thorough varous effercts, used to improve various cards /  activate abilities
	public int aether;
	public GameObject gameObject;
	public HandOfCards hand;
	public GridMatrix gridM;
	// the amount of cards currently in your hand
	public int handSize;
	public Camera cam;
	
	public void Start()
	{
		hand = GetComponent<HandOfCards>();
		hand.player = this;
		hand.cam = cam;
		hand.gridM = gridM;
		health = 20;
		connection = 10;
		aether = 5;
	}

	public void Update()
	{
		if(hand.gridM == null)
		{
			if(gridM != null)
			{
				hand.gridM = gridM;
			}
		}
	}

	public void setId(int i)
	{
		id = i;
	}

	public int Id() { return id; }


}
