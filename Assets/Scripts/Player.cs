using UnityEngine;
using System.Collections;

public class Player : Actor  {

	private float inputVerticalAxis = 0.0f;
	private float inputHorizontalAxis = 0.0f;
	private Vector3 desiredMovementSpeed = Vector3.zero;
	private Vector3 movementDelta = Vector3.zero;

	public float maxJumpHeight;



	private void Update () {
		inputHorizontalAxis = Input.GetAxis("Horizontal");
		inputVerticalAxis = Input.GetAxis("Vertical");

		if(Input.GetButtonDown("Jump")){
			if(IsGrounded()){
				Jump();
			}
		}
	}

	private void FixedUpdate() {
		desiredMovementSpeed.x = inputHorizontalAxis * actorSpeed;
		desiredMovementSpeed.z = 0.0f;

		movementDelta = desiredMovementSpeed - rigidbody.velocity;
		movementDelta.y = 0.0f;
		movementDelta.z = 0.0f;

		rigidbody.AddForce(movementDelta,ForceMode.VelocityChange);
	}

	private void Jump(){
		StartCoroutine(JumpCoroutine());
	}

	private IEnumerator JumpCoroutine(){
		float jumpStartYPosition = transform.position.y;
		float jumpEndYPosition = jumpStartYPosition + maxJumpHeight;

		while(Input.GetButton("Jump")){
			if(transform.position.y < jumpEndYPosition){
				rigidbody.AddForce(Vector3.up * rigidbody.mass * 20.0f);
			}
			else{
				break;
			}
			yield return new WaitForFixedUpdate();
		}
	}


}
