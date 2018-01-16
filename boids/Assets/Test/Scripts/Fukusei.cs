using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fukusei : MonoBehaviour
{


	public GameObject obj;
	public int num;
	
	private MaterialPropertyBlock _property;
	
	// Use this for initialization
	void Start ()
	{

		_property = new MaterialPropertyBlock();
		
		for (int i = 0; i < num; i++)
		{
			var instance = Instantiate(obj);
			instance.transform.position = new Vector3(
				Random.Range(-50f,50f),
				0,
				Random.Range(-50f,50f)
			);
			float r = Random.Range(0.0f, 1.0f);
			float g = Random.Range(0.0f, 1.0f);
			float b = Random.Range(0.0f, 1.0f);			
			
			//shaderに渡す
			_property.SetColor("_Col",new Color(r,g,b));
			_property.SetFloat("_DT", i / 1000f);
			
			var renderer = instance.GetComponent<MeshRenderer>();
			renderer.SetPropertyBlock(_property);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
