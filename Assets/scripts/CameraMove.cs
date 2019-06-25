using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//todo:заблокировать движение за границы поля, добавить вращение камеры
public class CameraMove : MonoBehaviour {

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var translation = Vector3.zero;
        translation += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        GetComponent<Camera>().transform.position += translation;
	}
}
