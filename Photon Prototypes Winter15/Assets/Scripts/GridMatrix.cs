using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GridMatrix : Photon.MonoBehaviour 
{

	// Use this for initialization
	public InitiateConnections connection;
	public Player player;
	public GFGrid grid;
	public Card[,] gridMatrix;
	public GameObject panel,panel2;
	public bool playersTurn;
	int index;
	bool once;
	List<GridBox> playerCastArea; // this players cast area
	List<GridBox> oPlayerCastArea; // the other players cast area

	void Start () 
	{
	index = 0;
	once = false;
	playerCastArea = new List<GridBox>();
	oPlayerCastArea = new List<GridBox>();

	
	
	//panel = connection.gameBoard.transform.Find("full_grid/Grid_Canvas/Panel").gameObject;
	
	}
	public void initPanels()
	{
		
		foreach(GridBox gb in playerCastArea)
		{
			if(gb.panel == null)
			gb.CreatePanel(panel,this);
		}

		foreach(GridBox gb in oPlayerCastArea)
		{
			if(gb.panel == null)
			gb.CreatePanel(panel,this);
		}
	}
	public void startup()
	{
		GridBox gb1 = new GridBox(panel);

		GridBox gb2 =new GridBox(panel2);

		Debug.Log(PhotonNetwork.player.ID);
		if(PhotonNetwork.player.ID == 2)
		{
		Debug.Log("I Am player 2");
		playersTurn = false;
		oPlayerCastArea.Add(gb2);
		playerCastArea.Add(gb1);
		}
		else
		{
		playersTurn = true;
		oPlayerCastArea.Add(gb1);
		playerCastArea.Add(gb2);
		}

		PhotonView view =  PhotonView.Find(6);
		
		
		gridMatrix = new Card[20,12];
	}
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

	}
	public void SetPlayer(Player p)
	{
		player = p;
	}
	// Update is called once per frame
	void Update () 
	{
		
		if(playerCastArea != null && !once && panel != null)
		{
			//initPanels();
			once = true;
		}
		if(Input.GetMouseButtonDown(0))
		{
			
			Camera cam = player.cam;//(connection.isPlayer1sTurn()) ? player1.cam : player2.cam;
			Ray r = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] hits= Physics.RaycastAll(r, 100);
			foreach(RaycastHit hit in hits)
			{
				if(hit.transform.name == "Cube")
				{
					Vector3 gPos = grid.WorldToGrid(hit.transform.position);
					//Debug.Log(gPos);
					//Debug.Log(inCastableArea(new Vector2(hit.transform.position.x,hit.transform.position.z),playersTurn));					
					break;
				}
			}
			
		}
	}
	// is a grid  point in the castable area for a certain player? 
	public bool inCastableArea(Vector2 gridPos, bool playersTurn = true)
	{
		List<GridBox> gbs;
		
		if(playersTurn)
		{
			gbs = playerCastArea;
		}
		else
		{
			//gbs = playerCastArea;	
			return false; // you cant cast when its not your turn
		}

		foreach(GridBox gb in gbs)
		{
			if(gb.isIn(grid.GridToWorld(gridPos)))
			return true;
		}

		return false;
	}


public Vector2 worldToGrid(Vector3 worldPos)
{
	Vector3 pos = grid.WorldToGrid(worldPos);
	Vector2 gridPos = new Vector2(pos.x,pos.y);
	Debug.Log(gridPos + " " + pos);
	return gridPos;
} 

public Vector3 gridToWorld(Vector2 gridPos)
{
	Vector3 pos = grid.GridToWorld(gridPos);
	//Vector3 worldPos = new Vector2(pos.x,pos.y);
	//Debug.Log(gridPos + " " + pos);
	return pos;
} 
public void AddToPlayersCastArea(GridBox gb)
{
	playerCastArea.Add(gb);
	initPanels();
}
public bool inCastableAreaWorld(Vector3 worldPos, bool playersTurn = true)
	{
		List<GridBox> gbs;
		//Debug.Log(gridPos);
		if(playersTurn)
		{
			gbs = playerCastArea;
		}
		else
		{
			return false; // you cant cast when its not your turn
		}

		foreach(GridBox gb in gbs)
		{
			if(gb.isIn(worldPos))
			return true;
		}

		return false;
	}


}


public class GridBox
{
	public Vector2 center,size; // minimun and maximum points in world space
	Vector2 min,max;
	public GameObject panel;
	int index;
	
	public GridBox(Vector2 center, Vector2 size)
	{
		this.center = center;	
		this.size = size;
		min = center - (size / 2f);

	}
	public GridBox(GameObject pan)
	{
		panel = pan;
	}

	public bool isIn(Vector3 pos)
	{
		if(panel != null)
		{
			Collider col = panel.GetComponent<Collider>();
			Debug.Log(pos + " " + col.bounds.center);
			if(col != null)
			return col.bounds.Contains(new Vector3(pos.x,panel.transform.position.y,pos.z));
			else return false;
		}
		return false;
		//return (pos.x >= min.x) && (pos.x <= max.x) && (pos.y >= min.y) && (pos.y <= max.y);
	}
	public void CreatePanel(GameObject pnl,GridMatrix gm)
	{
		if(panel ==null)
		{
		panel = PhotonNetwork.Instantiate("panel", pnl.transform.position, Quaternion.identity,0);
		panel.transform.localScale = new Vector3(panel.transform.localScale.x * (2 * size.x + 1), panel.transform.localScale.y * (2 * size.y + 1),1);
		panel.transform.SetParent(pnl.transform.parent,false);
		//panel.transform.eulerAngles = new Vector3(0,0,0);
		//panel.transform.localPosition = new Vector3(panel.transform.localPosition.x,panel.transform.localPosition.y,0);
		//RectTransform rt = panel.GetComponent<RectTransform>();
		panel.transform.position = new Vector3(center.x,pnl.transform.position.y,center.y);
		
		//panel.transform.localPosition = new Vector3(panel.transform.position.x,panel.transform.position.y,0);
		//rt.eulerAngles = new Vector3(-90,180,180);
		//Debug.Log("gridbox is " + min + " to " + max);
		
		//new Vector2(-20.5f + min.x * 2.05f,-10.6f + min.y * 1.7625f));	
		
		//Debug.Log(rt.rect.height+ " " + height);
		
		index++;
		}
		//rt.SetSize(size);
		 
	}	
}

public static class RectTransformExtensions
{
    public static void SetDefaultScale(this RectTransform trans) {
        trans.localScale = new Vector3(1, 1, 1);
    }
    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec) {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    public static Vector2 GetSize(this RectTransform trans) {
        return trans.rect.size;
    }
    public static float GetWidth(this RectTransform trans) {
        return trans.rect.width;
    }
    public static float GetHeight(this RectTransform trans) {
        return trans.rect.height;
    }

    public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos) {
        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
    }

    public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos) {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos) {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos) {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos) {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetSize(this RectTransform trans, Vector2 newSize) {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }
    public static void SetWidth(this RectTransform trans, float newSize) {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }
    public static void SetHeight(this RectTransform trans, float newSize) {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }
}
