using UnityEngine;
using System.Collections;

public class Actor : BaseScript {




	public float actorSpeed;
	public float skinWidth;

	protected CapsuleCollider capsuleCollider;

	protected void Awake(){
		capsuleCollider = (CapsuleCollider)collider;
	}

	protected bool IsGrounded(){
		return Physics.Linecast(transform.position + capsuleCollider.center ,(transform.position + capsuleCollider.center) - (Vector3.up * capsuleCollider.height/2.0f));
	}

	protected bool IsObstacleInDirection(Vector3 direction){
		Ray topRay = new Ray(transform.position + capsuleCollider.center + Vector3.up * 0.6f,direction);
		Ray midRay = new Ray(transform.position + capsuleCollider.center, direction);
		Ray bottomRay = new Ray(transform.position + capsuleCollider.center - Vector3.up * 0.6f,direction);
		Debug.DrawRay(transform.position + capsuleCollider.center + Vector3.up * 0.6f,direction);
		Debug.DrawRay(transform.position + capsuleCollider.center, direction);
		Debug.DrawRay(transform.position + capsuleCollider.center - Vector3.up * 0.6f,direction);
		float length = capsuleCollider.radius + skinWidth;
		return !Physics.Raycast(topRay,length) && !Physics.Raycast(midRay,length) && !Physics.Raycast(bottomRay,length);
	}

	protected void SetActorVerticalVelocity(float verticalVelocity){
		Vector3 tmp = rigidbody.velocity;
		tmp.y = verticalVelocity;
		rigidbody.velocity = tmp;
	}
	
	protected void SetActorHorizontalVelocity(float horizontalVelocity){
		Vector3 tmp = rigidbody.velocity;
		tmp.x = horizontalVelocity;
		rigidbody.velocity = tmp;
	}



}
