using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class EnemyBehavior : MonoBehaviour {

	public Vector2 targetPoint;
	float maxSpeed;
	public float currentSpeed;
	float acceleration;
	float angularRotation = 0.3f;
	float curTimer;
	float deathTimer = .33f;

	public AudioClip explosion;

	Vector3 currentRotation;

	int health;

	// Use this for initialization
	void Start () {
		maxSpeed = 40;
		currentSpeed = 0;
		acceleration = 15;
		curTimer = 3.0f;
		targetPoint = new Vector2(200, -100);

		health = Random.Range(3, 6);

		currentRotation = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {

		if(this.tag == "Dead")
		{
			deathTimer--;
			if(deathTimer <= 0.0f)
				GameObject.Destroy(this.gameObject);
		
		}
		else
		{
		Vector2 differenceVector = new Vector2(targetPoint.x- this.transform.position.x,
		                                       targetPoint.y- this.transform.position.y);
		if(currentSpeed < maxSpeed && currentSpeed < differenceVector.magnitude)
		{
			currentSpeed += (acceleration*Time.deltaTime);
		}
		if(currentSpeed > differenceVector.magnitude && differenceVector.magnitude < maxSpeed)
			currentSpeed = differenceVector.magnitude;




	
		float angle = Mathf.Atan2(differenceVector.y , differenceVector.x);
		
		angle = angle*Mathf.Rad2Deg+90;


		currentRotation = Vector3.Lerp(currentRotation, new Vector3(0,0,angle), Time.deltaTime * angularRotation);
		transform.eulerAngles = currentRotation; 



		differenceVector = differenceVector.normalized;
		

		this.transform.position = new Vector2(this.transform.position.x + differenceVector.x*currentSpeed*Time.deltaTime, 
		                                      this.transform.position.y + differenceVector.y*currentSpeed*Time.deltaTime);

		curTimer-=Time.deltaTime;
		if(curTimer <= 0.0f)
		{

			targetPoint = new Vector2(Random.Range(this.transform.position.x - 5000, this.transform.position.x+5000),
		    	                      Random.Range (this.transform.position.y-5000, this.transform.position.y+5000));

			curTimer = Random.Range (4.0f, 10.0f);
		}




		//Check missile collision

		GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

		List<GameObject> toBeDeleted = new List<GameObject>();

		Bounds tileBoundingBox = this.renderer.bounds;
		Vector3 tileExtents = tileBoundingBox.extents;
		
		Rect tileBoundingRect = new Rect(tileBoundingBox.center.x-tileExtents.x,
		                                 tileBoundingBox.center.y-tileExtents.y, tileExtents.x*1.15f, tileExtents.y*1.35f);


		foreach(GameObject missile in projectiles)
		{
			Bounds missileBoundingBox = missile.renderer.bounds;
			Vector3 missileExtents = missileBoundingBox.extents;
			
			Rect missileBoundingRect = new Rect(missileBoundingBox.center.x-missileExtents.x,
			                                  missileBoundingBox.center.y-missileExtents.y, 
			                                  missileExtents.x*1.8f, missileExtents.y*1.8f);

			bool isIntersecting = doesIntersect(missileBoundingRect,tileBoundingRect);
			
			if(isIntersecting)
			{
				Debug.Log ("Missile Hit");
				toBeDeleted.Add(missile);
				health--;

					AudioSource.PlayClipAtPoint(explosion, this.transform.position);
			}

		}

		foreach(GameObject bad in toBeDeleted)
		{
			GameObject.Destroy(bad);
		}

		if(this.health <= 0)
		{
			gameObject.tag = "Dead";
		}
		}

	}

	bool doesIntersect(Rect player, Rect obj)
	{
		Vector2 playerTopLeft = new Vector2(player.center.x-player.width/2, player.center.y+player.height/2);
		Vector2 playerBottomRight = new Vector2(player.center.x+player.width/2, player.center.y-player.height/2);
		
		Vector2 objectTopLeft = new Vector2(obj.center.x-obj.width/2, obj.center.y+obj.height/2);
		Vector2 objectBottomRight = new Vector2(obj.center.x+obj.width/2, obj.center.y-obj.height/2);
		
		
		
		return (playerTopLeft.x < objectBottomRight.x && playerTopLeft.y > objectBottomRight.y) &&
			(playerBottomRight.x > objectTopLeft.x && playerBottomRight.y < objectTopLeft.y);
	}
}
