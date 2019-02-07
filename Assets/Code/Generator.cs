using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

	public float viewRadius;
	public LayerMask targetMask;
	public LayerMask obstacleMask;
	public List<Transform> visibleTargets = new List<Transform>();

	public GameObject ElectricityParticle;
	public List<GameObject> particles = new List<GameObject>();
	public float electricSpeed;

	void Start(){
		StartCoroutine("FindTargetsWithDelay" , .2f);
        particles.Clear();
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

	void LateUpdate(){
		if(particles.Count != visibleTargets.Count){
			//Debug.Log("oof2");
			for(int i = 0; i < particles.Count; i++){
				//Debug.Log("oof3");
				Destroy(particles[i]);
			}
			particles.Clear();
			foreach(Transform visibleTarget in visibleTargets){
				//Debug.Log("oof4");
				particles.Add(Instantiate(ElectricityParticle,transform.position,transform.rotation));
			}
		}
		if(visibleTargets.Count > 0){
			for(int f = 0; f < particles.Count; f++){
				if(particles.Count != visibleTargets.Count){
					//Debug.Log("oof");
					break;
				}
	
				if(particles[f].transform.position == visibleTargets[f].position){
					particles[f].GetComponent<Electricity>().go = false;
				}
				if(particles[f].transform.position == transform.position){
					particles[f].GetComponent<Electricity>().go = true;
				}

				if(particles[f].GetComponent<Electricity>().go){
					Debug.Log("Particle : " + particles[f] + " is going to target : " + visibleTargets[f]);
					particles[f].transform.position = Vector3.MoveTowards(particles[f].transform.position, visibleTargets[f].position, Time.deltaTime * electricSpeed);
				} else {
					particles[f].transform.position = Vector3.MoveTowards(particles[f].transform.position, transform.position, Time.deltaTime * electricSpeed);
				}
			}
		}

		ParticleSystem.ShapeModule psShape = GetComponentInChildren<ParticleSystem>().shape;
		ParticleSystem.EmissionModule psEmis = GetComponentInChildren<ParticleSystem>().emission;
		psShape.radius = viewRadius / transform.localScale.x;
		psEmis.rateOverTime = viewRadius * 600;
	}
}
