using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Rank {Conduit,Soldier,Direct}; 
public enum Movement {AdjecentOnly, Diagonal_Only, L_Formation };
public class Card : MonoBehaviour {

	// Use this for initialization
	public Rank rank; 
	public int health;
	public List<int> connection;
	public int range;
	public bool played;
	//[HideInInspector] // Hides var below
	public bool mouseOn;
	//[HideInInspector]
	public bool viewing;
	//[HideInInspector]
	public bool moved,move;
	[HideInInspector]
	public int id;
	//[HideInInspector]
	public Vector3 movePos;
	[HideInInspector]
	Vector3 oldPos;
	Quaternion oldRot;
	[HideInInspector]
	public GameObject oldGame;
	[HideInInspector]
	public bool wasViewing;
	void Start () {
	wasViewing = false;
	played = false;
	//movePos = new Vector3(0,.1f,.1f);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		if(!played)
		{


			if(move && !moved && mouseOn)
			{
				if(PhotonNetwork.player.ID == 1)
				transform.position+=movePos;
				else
				transform.position-=movePos;
				moved = true;
			}
			else if(!move && moved && !viewing)
			{
				if(PhotonNetwork.player.ID == 1)
				transform.position-=movePos;
				else
				transform.position+=movePos;

				move = false;
				moved = false;
			}

			if(wasViewing && !viewing)
			{
				transform.position = oldPos;
				transform.rotation = oldRot;
				wasViewing = false;
			}

		}


	}

	public void View(Transform t)
	{
		if(PhotonNetwork.player.ID == 1)
		oldPos = transform.position - movePos;
		else
		oldPos = transform.position + movePos;

		oldRot = transform.rotation;

		
		transform.eulerAngles = t.eulerAngles + new Vector3(180,0,180);
		transform.position = t.position ;
		//transform.parent = oldP;
		viewing = true;
		wasViewing = true;
	}

	public void playToGrid(GridMatrix gm, Vector2 gridpos)
	{

	}

	public void playToGridCube(GridMatrix gm, Transform cube)
	{
		Vector2 gridPos = gm.worldToGrid(cube.position);
		
		if(rank == Rank.Conduit)
		{
			GridBox newArea = new GridBox(new Vector2(cube.position.x,cube.position.z),new Vector2(range,range));
			gm.AddToPlayersCastArea(newArea);
		}
		transform.parent = cube;
		transform.Find("Model").gameObject.SetActive(true);
		transform.localPosition = new Vector3(0,0,0);
		transform.Find("Model").localPosition = new Vector3(0,1,0);
		transform.Find("card 1").gameObject.SetActive(false);
		Debug.Log("played card");
		played = true;
	}

	public void playToGridWorld(GridMatrix gm, Player p,Vector3 worldpos)
	{
		Vector2 gridPos = gm.worldToGrid(new Vector3(worldpos.x,worldpos.z));

	}


}
