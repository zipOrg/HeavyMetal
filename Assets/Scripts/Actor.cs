using UnityEngine;
using System.Collections;

public class Actor : BaseScript {
	public float actorSpeed;

	protected CapsuleCollider capsuleCollider;


	protected void Awake(){
		capsuleCollider = (CapsuleCollider)collider;
	}

	protected bool IsGrounded(){
		return Physics.Linecast(transform.position,transform.position - (Vector3.up * capsuleCollider.height/2.0f));
	}

}
