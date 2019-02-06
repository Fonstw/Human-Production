using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour {

	public float viewRadius;
	public LayerMask targetMask;
	public LayerMask obstacleMask;
	public List<Transform> visibleTargets = new List<Transform>();

	public GameObject grass;

	public GameObject tube;
	public GameObject foodResource;
	public List<GameObject> tubes = new List<GameObject>();
	public List<GameObject> food = new List<GameObject>();

	void Start(){
		StartCoroutine("FindTargetsWithDelay" , .2f);
        tubes.Clear();
        food.Clear();
        visibleTargets.Clear();
	}

	IEnumerator FindTargetsWithDelay(float delay){
		while(true){
			yield return new WaitForSeconds (delay);
			FindVisibleTargets();
		}
	}
	void FindVisibleTargets(){
		visibleTargets.Clear();
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for(int i = 0; i < targetsInViewRadius.Length; i++){
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if(Vector3.Angle (transform.forward, dirToTarget) < 180){
				float dstToTarget = Vector3.Distance ( transform.position, target.position);
				if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)){
					visibleTargets.Add(target);
				}
			}
		}
	}
	public Vector3 DirFromAngle(float angleInDegrees){
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	void Update(){
		if(tubes.Count != visibleTargets.Count){
			Debug.Log("oof2");
			for(int i = 0; i < tubes.Count; i++){
				Debug.Log("oof3");
				Destroy(tubes[i]);
				Destroy(food[i]);
			}
			tubes.Clear();
			food.Clear();
			foreach(Transform visibleTarget in visibleTargets){
				Debug.Log("oof4");
				tubes.Add(Instantiate(tube,transform.position,tube.transform.rotation));
				food.Add(Instantiate(foodResource, transform.position, foodResource.transform.rotation));
			}
		}
		if(visibleTargets.Count > 0){
			for(int f = 0; f < tubes.Count; f++){
				if(tubes.Count != visibleTargets.Count){
					Debug.Log("oof");
					break;
				}

				float x = transform.position.x + (visibleTargets[f].position.x - transform.position.x)/2;
				float y = transform.position.y + (visibleTargets[f].position.y - transform.position.y)/2;
				float z = transform.position.z + (visibleTargets[f].position.z - transform.position.z)/2;
				tubes[f].transform.position = new Vector3(x,y,z);
				tubes[f].transform.LookAt(visibleTargets[f]);
				tubes[f].transform.localScale = new Vector3(tubes[f].transform.localScale.x,tubes[f].transform.localScale.y, Vector3.Distance(transform.position,visibleTargets[f].position));

				if(food[f].transform.position == visibleTargets[f].position){
					food[f].transform.position = transform.position;
				}
				food[f].transform.position = Vector3.MoveTowards(food[f].transform.position, visibleTargets[f].position, Time.deltaTime * 10);
			}
		}

		grass.transform.localScale = new Vector3((viewRadius*2)/transform.localScale.x,grass.transform.localScale.y,(viewRadius*2)/transform.localScale.z);
	}
}
