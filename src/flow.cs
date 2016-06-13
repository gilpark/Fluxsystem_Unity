using UnityEngine;
using System.Collections;

public class flow : MonoBehaviour {
	Vector3 location;
	Vector3 acceleration = new Vector3(0,0,0);
	Vector3 velocity = new Vector3(0,0,0);

	public float maxforce;    // Maximum steering force
  	public float maxspeed;    // Maximum speed
	public bool dead = false;
	public int ageMax = 300;
	int age = 0;
	public bool aging = false;
	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		
		rb = gameObject.GetComponent<Rigidbody> ();
		//location = transform.position;
		location = rb.position;
		age = ageMax ;
	}
	
	// Update is called once per frame
	void Update () {
		velocity = velocity + acceleration;
		velocity =Vector3.ClampMagnitude(velocity, maxspeed);

		//transform.position = transform.position + velocity;
		rb.MovePosition(rb.position + velocity);
		acceleration = acceleration * 0;
		if (aging) {
			age--;
			if (age < 0)
				dead = true;
		}
			
	}
	void follow(Cell cell){
		Vector3 desired = cell.direction;
		desired = desired * maxspeed;
		Vector3 steer = desired - velocity;
		steer = Vector3.ClampMagnitude(steer,maxforce);
		applyForce(steer);
		//print("on the flow! - dsrd :" + desired + " steer : "+steer);
	}
	void applyForce(Vector3 force){
		acceleration = acceleration + force;
		//print("acceleration :" + acceleration );
	}
	void OnTriggerEnter(Collider other){

		if(other.tag =="flux"){
			Cell c = other.GetComponent<Cell>();
			follow(c);
			if (dead) {
				Destroy (gameObject);
				Destroy (this);	
			}
		}

		if(other.tag =="destroy"){
			aging = true;
		}
	}
}
