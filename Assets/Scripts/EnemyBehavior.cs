using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public Vector2 targetPoint;
	float maxSpeed;
	float currentSpeed;
	float acceleration;
	float angularRotation = 0.3f;
	float curTimer;

	Vector3 currentRotation;

	// Use this for initialization
	void Start () {
		maxSpeed = 40;
		currentSpeed = 0;
		acceleration = 15;
		curTimer = 3.0f;
		targetPoint = new Vector2(200, -100);


		currentRotation = new Vector3(0,0,0);
			//new Vector2(Random.Range(this.transform.position.x - 50, this.transform.position.x+50),
		              //            Random.Range (this.transform.position.y-50, this.transform.position.y+50));
	}
	
	// Update is called once per frame
	void Update () {

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



	}
}
