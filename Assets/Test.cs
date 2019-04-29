using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {

        transform.up = target.transform.position - transform.position;
        //transform.LookAt(target.transform);
        //transform.Rotate(90, 0, 0);
    }
}
