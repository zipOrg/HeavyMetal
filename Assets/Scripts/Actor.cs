﻿using UnityEngine;
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

	protected bool CanMoveInDirection(Vector3 direction){
		Ray topRay = new Ray(transform.position + Vector3.up * 0.6f,direction);
		Ray midRay = new Ray(transform.position, direction);
		Ray bottomRay = new Ray(transform.position - Vector3.up * 0.6f,direction);
		Debug.DrawRay(transform.position + Vector3.up * 0.6f,direction);
		Debug.DrawRay(transform.position, direction);
		Debug.DrawRay(transform.position - Vector3.up * 0.6f,direction);
		return true;
	}

}