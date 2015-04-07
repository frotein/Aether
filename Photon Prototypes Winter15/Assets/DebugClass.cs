using UnityEngine;
using System.Collections;

public class DebugClass : MonoBehaviour {

	// Use this for initialization
	public Vector2 dbval;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		RectTransform rt = gameObject.GetComponent<RectTransform>();
		rt.SetLeftBottomPosition(new Vector2(-20.5f + dbval.x * 2.05f,-10.6f+ dbval.y * 1.7625f));
	}
}
