using UnityEngine;
using System.Collections;


//git commit
public class Player : Actor  {


	private float inputHorizontalAxis = 0.0f;
	private float inputHorizontalAxisLastFrame = 0.0f;


	private Vector3 desiredMovementSpeed = Vector3.zero;
	private Vector3 movementDelta = Vector3.zero;


	public float timeToReachMaxSpeed;
	public float timeToRachFullStopFromMaxSpeed;
	public AnimationCurve accelerationCurve;

	private float currentDirectionMovementTime;
	private float directionMovementTimeAtDirectionChange;
	private float currentAcceleration;

	private void Update () {
		CalculateHorizontalMovement();
		if(Input.GetButtonDown("Jump")){
			if(IsGrounded()){
				Jump();
			}
		}
	}

	//this goes in normal update
	private void CalculateHorizontalMovement(){
		inputHorizontalAxis = Input.GetAxis("Horizontal");

		if(inputHorizontalAxis != inputHorizontalAxisLastFrame){
			if(inputHorizontalAxisLastFrame == 0.0f){

				if(inputHorizontalAxis < 0.0f){
					print ("started moving left");
				}
				else{
					print ("started moving right");
				}
				//currentDirectionMovementTime *= -1.0f;
			}
			else{
				if(inputHorizontalAxis == 0.0f){
					print ("movement stopped");
				}
			}
			inputHorizontalAxisLastFrame = inputHorizontalAxis;
		}

		if(inputHorizontalAxis == 0.0f){
			if(currentDirectionMovementTime > 0.0f){
				currentDirectionMovementTime -= Time.deltaTime;
			}

			else{
				currentDirectionMovementTime = 0.0f;
			}

		}
		else{
			if(currentDirectionMovementTime < timeToReachMaxSpeed){
				currentDirectionMovementTime += Time.deltaTime;
			}
			else{
				currentDirectionMovementTime = timeToReachMaxSpeed;
			}
		}

		//currentAcceleration =  accelerationCurve.Evaluate(currentDirectionMovementTime/timeToReachMaxSpeed); //.tutej
		currentAcceleration = currentDirectionMovementTime/timeToReachMaxSpeed;


		desiredMovementSpeed.x = inputHorizontalAxis * actorSpeed * currentAcceleration;
		desiredMovementSpeed.z = 0.0f;
		movementDelta = (desiredMovementSpeed - rigidbody.velocity);
		movementDelta.y = 0.0f;
		movementDelta.z = 0.0f;
		CanMoveInDirection(desiredMovementSpeed/actorSpeed * 0.6f);
	}



	//this goes in fixed update
	private void ApplyHorizontalMovement(){
		rigidbody.AddForce(movementDelta,ForceMode.VelocityChange);
	}

	private void FixedUpdate() {
		ApplyHorizontalMovement();
	}

	private void Jump(){
		//rigidbody.AddForce(Vector3.up*1000.0f,ForceMode.Impulse);
		StartCoroutine(JumpCoroutine());
	}

	private IEnumerator JumpCoroutine(){
		float initialJumpForce = 4000.0f;

		while(Input.GetButton("Jump")){

			rigidbody.AddForce(Vector3.up * initialJumpForce);
			initialJumpForce *= 0.9f;

			yield return new WaitForEndOfFrame();
		}
	}



	private void OnGUI(){
		ShowDebugData("current direction movement time",currentDirectionMovementTime);
		ShowDebugData("current horizontal velocity",rigidbody.velocity.x);
		ShowDebugData("current acceleration",currentAcceleration);
	}

	private void ShowDebugData(string propertyName, object propertyValue){
		GUILayout.Label(propertyName + " : " + propertyValue);
	}


}
