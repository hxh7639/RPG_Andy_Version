using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	void onTriggerEnter(Collider collider)
	{
		print ("Projectile hit " + collider.gameObject);
	}
}
