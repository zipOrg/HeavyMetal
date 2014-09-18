using UnityEngine;
using System.Collections;



public class Player : Actor  {

	private const float HORIZONTAL_INPUT_RANGE = 2.0f;


	public float timeToReachMaxSpeed;
	public float initialJumpForce;
	public float jumpForceDampingRatio;


	private float inputHorizontalAxis = 0.0f;
	private float inputHorizontalAxisLastFrame = 0.0f;
	private float smoothHorizontalAxis = 0.0f;
	private float smoothingSpeed = 0.0f;

	private Vector3 desiredMovementSpeed = Vector3.zero;
	private Vector3 movementDelta = Vector3.zero;



	private void Awake(){
		base.Awake();
		smoothingSpeed = HORIZONTAL_INPUT_RANGE/timeToReachMaxSpeed;

	}

	private void Update () {
		HandleInput();
		SmoothHorizontalAxis();
		CalculateHorizontalMovement();
	}

	//this goes in normal update
	private void CalculateHorizontalMovement(){
		desiredMovementSpeed.x = smoothHorizontalAxis * actorSpeed;
		movementDelta = (desiredMovementSpeed - rigidbody.velocity);
		movementDelta.y = 0.0f;
		movementDelta.z = 0.0f;
	}



	private void SmoothHorizontalAxis(){

		if((Mathf.Abs((smoothHorizontalAxis+HORIZONTAL_INPUT_RANGE) - (inputHorizontalAxis+HORIZONTAL_INPUT_RANGE)) > Time.smoothDeltaTime)){
			if(smoothHorizontalAxis > inputHorizontalAxis){
				smoothHorizontalAxis -= Time.smoothDeltaTime * smoothingSpeed;

			}
			else if(smoothHorizontalAxis < inputHorizontalAxis){
				smoothHorizontalAxis += Time.smoothDeltaTime * smoothingSpeed;
			}
		}
		else{
			smoothHorizontalAxis = inputHorizontalAxis;
		}
	}

	private void HandleInput(){
		inputHorizontalAxis = Input.GetAxisRaw("Horizontal");

		if(Input.GetButtonDown("Jump")){
			if(IsGrounded()){
				Jump();
			}
		}
	}

	protected void setCurrentDirection(EnumDirection newDirection){
		if(currentDirection != newDirection){
			currentDirection = newDirection;
		}
	}


	//this goes in fixed update
	private void ApplyHorizontalMovement(){
		rigidbody.AddForce(movementDelta,ForceMode.VelocityChange);
	}

	private void FixedUpdate() {
		ApplyHorizontalMovement();
	}

	private void Jump(){
		StartCoroutine(JumpCoroutine());
	}

	private IEnumerator JumpCoroutine(){
		float currentJumpForce = initialJumpForce;

		while(Input.GetButton("Jump")){
			rigidbody.AddForce(Vector3.up * currentJumpForce);
			currentJumpForce *= jumpForceDampingRatio;
			yield return new WaitForEndOfFrame();
		}
	}



	private void OnGUI(){

		ShowDebugData("current horizontal velocity",rigidbody.velocity.x);
		ShowDebugData("current horizontal axis",inputHorizontalAxis);
		ShowDebugData("current horizontal axis",smoothHorizontalAxis);
	}

	private void ShowDebugData(string propertyName, object propertyValue){
		GUILayout.Label(propertyName + " : " + propertyValue);
	}


}
