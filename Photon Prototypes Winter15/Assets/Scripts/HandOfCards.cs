using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HandOfCards : MonoBehaviour {

	public RectTransform handCanvas;
	public GameObject TestCard;
	int cid;
	public Camera cam;
	List<GameObject> cards;
	Card grabbedCard;
	public Player player;
	public bool player1,grabbed;
	public Vector3 onMove;
	public GridMatrix gridM;
	public Transform viewPosition;
	Card card;
	Transform mOn;
	Vector3 onPos;
	
	// Use this for initialization
	void Start () 
	{
	cid = 0;
	cards = new List<GameObject>();
	grabbed = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(card != null)
		{
			if(!card.viewing && card.mouseOn && Input.GetMouseButtonDown(0))
			{
				grabbedCard = card;
				grabbed = true;
						
			}

			if(grabbed && Input.GetMouseButtonUp(0))
			{
				Ray mouseRay  = cam.ScreenPointToRay(Input.mousePosition);
				RaycastHit[] hits;

				hits = Physics.RaycastAll(mouseRay, 100);
				foreach(RaycastHit hit in hits)
				{
					if(hit.transform.name == "Cube")
					{
						Vector2 pos = new Vector2(hit.transform.position.x,hit.transform.position.z);
						//Debug.Log(gridM.inCastableAreaWorld(hit.transform.position,true));
						if(gridM.inCastableAreaWorld(hit.transform.position,true))
						{
							Debug.Log("can cast here");
							card.playToGridCube(gridM,hit.transform);
							break;
						}
					}
				}
			}

			if(Input.GetMouseButtonUp(0))
			{
				grabbed = false;
			}
			if(card.viewing  && Input.GetMouseButtonDown(1))
			{	
				card.viewing = false;
			}
			else
			{
				if(card.move && card.moved && Input.GetMouseButtonDown(1) && !card.wasViewing)
				{
					card.View(viewPosition);
				}
			}


		}
	}

	void FixedUpdate()
	{
		
		if(player != null)
		{
			cam = GameObject.Find("Player " + player.Id() + " Camera").GetComponent<Camera>();			
		}
		if(card != null)
		{
			card.mouseOn = false;
		}
		//Debug.Log(cam);
		if(cam != null)
		{

		Ray mouseRay  = cam.ScreenPointToRay(Input.mousePosition);
			bool hitSomething = false;
			foreach(GameObject cgo in cards)
			{
				
				Collider col = cgo.transform.GetChild(0).GetComponent<Collider>();
				//Debug.Log(col);
				RaycastHit hit;
				if(col.Raycast(mouseRay,out hit, 50))
				{
					hitSomething = true;
					//Debug.Log("going " + hit.transform.name);
					Card c = hit.transform.parent.GetComponent<Card>();
					if(c != null)
					{
						if(card!= null)
						{
							if(card.id != c.id)
							{
								if(!card.viewing)
								{
									card.move = false;
									card = c;						
									card.mouseOn = true;

									card.movePos = onMove;
									if(hit.textureCoord.x > 0.2f && hit.textureCoord. x < .8f &&
									   hit.textureCoord.y > 0.2f && hit.textureCoord. y < .8f)
									{
										card.move = true;
									}
									
								}
							}
							else
							{
								card = c;						
								card.mouseOn = true;
								card.movePos = onMove;
								if(hit.textureCoord.x > 0.2f && hit.textureCoord. x < .8f &&
								   hit.textureCoord.y > 0.2f && hit.textureCoord. y < .8f)
								{
									card.move = true;										
								}
							}
						}
						else
						{
							card = c;						
							card.mouseOn = true;
							card.movePos = onMove;
							if(hit.textureCoord.x > 0.2f && hit.textureCoord. x < .8f &&
							   hit.textureCoord.y > 0.2f && hit.textureCoord. y < .8f)
							{
								card.move = true;										
							}
						}
					}
				}
			}

			if(!hitSomething && card != null)
			{
				card.move = false;
			}
		}
		
		//Vector3 mouse = 
		
	}
	[RPC]
	void SwitchTurns()
	{

	} 
	public void DrawCard(string cardName)
	{
		//GameObject newCard = GameObject.Instantiate(card);
		
		float cardX = cards.Count * -2.5f + 7.5f;
		
		GameObject NewCard = PhotonNetwork.Instantiate(cardName, handCanvas.position + new Vector3(cardX ,0,0), handCanvas.rotation, 0);
		NewCard.transform.parent = transform;
		//NewCard.transform.localScale = new Vector3(1,1,.1f);
		Card tempCard = NewCard.GetComponent<Card>();
		tempCard.id = cid;

		cid++;
		NewCard.transform.localEulerAngles += new Vector3(0,0,0);
		
		cards.Add(NewCard);
	}
}
