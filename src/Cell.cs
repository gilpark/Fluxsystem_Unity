using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
	public Vector3 direction, size;
	public bool isPassable = true;
	public bool isGoal = false;
	public bool ready = false;
	public bool block = false;
	public int[]  neighbors = new int[4];
	public int id, cols, rows;
	public float cost;

	//public int north,east,south,west;
	void Start () {
		gameObject.tag = "flux";
	}

	 public void reset(){
	 	cost = 0;
	 	isGoal = false;
	 	isPassable = true;
	}
	
	// Update is called once per frame
	void Update () {
		ready = true;
	}

	void OnTriggerEnter(Collider other){

		if(other.tag =="block"){
			block = true;
			isPassable = false;		
		}

		if(other.tag =="goal"){
			isGoal = true;
			cost = 0;
		}
	}

	
	void OnDrawGizmos(){
		Gizmos.matrix = transform.localToWorldMatrix;
		BoxCollider box = gameObject.GetComponent<BoxCollider>();	
		if(Flux.flux.GetComponent<Flux>().debug){
			Gizmos.color = new Color(0,(float)(cost * 0.1),0,0.8F);
			Gizmos.DrawCube(box.center, size);
			Gizmos.color = new Color((float)(cost * 0.1),(float)(cost * 0.1),0,0.8F);
			Gizmos.DrawLine(box.center, box.center+direction);
		}
		//Debug.DrawLine(box.center, box.center+direction,Color.red);
	}
}
