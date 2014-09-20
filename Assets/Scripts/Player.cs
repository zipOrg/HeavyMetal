using UnityEngine;
using System.Collections;



public class Player : Actor  {

	private const float HORIZONTAL_INPUT_RANGE = 2.0f;



	public float timeToReachMaxSpeed;
	public float jumpHeight;
	public float minJumpThreshold;
	public Animator playerAnimator;


	private float inputHorizontalAxis = 0.0f;
	private float inputHorizontalAxisLastFrame = 0.0f;
	private float smoothHorizontalAxis = 0.0f;
	private float smoothingSpeed = 0.0f;

	private Vector3 desiredMovementSpeed = Vector3.zero;
	private Vector3 movementDelta = Vector3.zero;

	private bool isAttacking;


	private void Awake(){
		base.Awake();
		smoothingSpeed = HORIZONTAL_INPUT_RANGE/timeToReachMaxSpeed;
		isAttacking = false;
	}

	private void Update () {
		HandleInput();
		SmoothHorizontalAxis();
		CalculateHorizontalMovement();
		RotateCharacter();
		HandleAnimations();
	}

	private void HandleAnimations(){
		playerAnimator.SetFloat("Blend",Mathf.Abs(rigidbody.velocity.x)/actorSpeed);
		playerAnimator.SetBool("Grounded",IsGrounded());
	}

	private void FixedUpdate() {
		ApplyHorizontalMovement();
	}

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
		if(!isAttacking){
			if(CanMoveInDirection(new Vector3(Input.GetAxisRaw("Horizontal"),0,0))){
				inputHorizontalAxis = Input.GetAxisRaw("Horizontal");
			}
			else{
				inputHorizontalAxis = 0.0f;
				SetActorHorizontalVelocity(0.0f);
			}
		}
		else{
			inputHorizontalAxis = 0.0f;
			SetActorHorizontalVelocity(0.0f);
		}



		if(Input.GetButtonDown("Jump")){
			if(IsGrounded()){
				Jump();
			}
		}

		if(Input.GetButtonDown("Fire1") && IsGrounded()){
			isAttacking = true;
			playerAnimator.SetTrigger("Attack Trigger");
		}
	}

	private void RotateCharacter(){
		if(Mathf.Abs(inputHorizontalAxis) > 0.1f){
			if(inputHorizontalAxis < 0.0f){
				transform.eulerAngles = new Vector3(0.0f,270.0f,0.0f);
			}
			else{
				transform.eulerAngles = new Vector3(0.0f,90.0f,0.0f);
			}
		}


	}

	private void ApplyHorizontalMovement(){
		rigidbody.AddForce(movementDelta,ForceMode.VelocityChange);
	}

	private void Jump(){
		StartCoroutine(JumpCoroutine());
	}

	private IEnumerator JumpCoroutine(){

		float initialVelocity = Mathf.Sqrt(jumpHeight * -2.0f * Physics.gravity.y);
		rigidbody.AddForce(initialVelocity*Vector3.up,ForceMode.VelocityChange);

		yield return new WaitForSeconds(minJumpThreshold);

		while(rigidbody.velocity.y >= 0.0f){
			if(!Input.GetButton("Jump")){
				SetActorVerticalVelocity(rigidbody.velocity.y * 0.90f);
			}
			yield return new WaitForEndOfFrame();
		}
	}





	private void OnGUI(){
		ShowDebugData("current horizontal velocity",rigidbody.velocity.x);
		ShowDebugData("current horizontal axis",inputHorizontalAxis);
		ShowDebugData("current horizontal axis",smoothHorizontalAxis);
		ShowDebugData("current vertical velocity",rigidbody.velocity.y);
	}

	private void ShowDebugData(string propertyName, object propertyValue){
		GUILayout.Label(propertyName + " : " + propertyValue);
	}

	public void AttackFinished(){
		print ("finisz");
		isAttacking = false;
	}

}
