using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	
	public int shootInc = 4;
	private int shootTime = 0;

	public GameObject bullet;
	public string side = "right";

	public bool canRotate = false;
	public bool isMobile = false;
	public AudioClip pewpew;
	AudioSource playpew;
	
	void Start () {
		playpew = gameObject.AddComponent<AudioSource>() as AudioSource;
        shootTime = shootInc;
	}
	
	void FixedUpdate () {

        if (shootTime > 0)
        {
            shootTime--;
        }
		if(canRotate){
			var mouse = Input.mousePosition;
			var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
			var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
			var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, angle-90);
		}
		
	}
	
	public void Fire(string side) {
		if ((this.side.ToLower().Equals(side.ToLower()) ) && (shootTime <= 0)) {
			Instantiate (bullet, transform.position + new Vector3 (0, 0, 1f), transform.rotation);
			playpew.PlayOneShot (pewpew);
            shootTime = shootInc;
        }
	}

	public void EnemyFire(GameObject target) {

		if (shootTime <= 0) {
			var targ = target.transform.position;
			var turr = transform.position;
			var offset = new Vector2(targ.x - turr.x, targ.y - turr.y);
			var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            angle -= 90;
            if (canRotate) { transform.rotation = Quaternion.Euler(0, 0, angle); } //else { transform.rotation = GetComponentInParent<Transform>().rotation;  }
			Instantiate (bullet, transform.position + new Vector3 (0, 0, 1f), transform.rotation);
			playpew.PlayOneShot (pewpew);
            shootTime = shootInc;
        }

	}
	
	
	
}
