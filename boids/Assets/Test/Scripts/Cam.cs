using System;
using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour {

    public GameObject target = null;
    public float amp = 2f;
    public float radX = 0f;
    public float radY = Mathf.PI/4;

    public float spdX = 0.01f;

    private Vector3 mPosition;
    private int counter = 0;

    // Use this for initialization
    void Start () {
        mPosition = this.transform.position;

        this.amp = mPosition.magnitude;
        this.radX = Mathf.Atan2(mPosition.x, mPosition.z);
        this.radY = Mathf.Atan2(mPosition.y, mPosition.z);
        
        //this.amp = Math.sqrt( p.x*p.x + p.y*p.y + p.z*p.z );
        //this.radX = Math.atan2(p.x, p.z);
        //this.radY = Math.atan2(p.y, p.z);            
    }
	
    // Update is called once per frame
    void Update () {
	
        //if ( target != null ){
			
            

            float amp1 	= this.amp * Mathf.Cos(this.radY);
            Vector3 tt = Vector3.zero;

            float x 	= tt.x + amp1 * Mathf.Sin( this.radX );//横
            float y		= tt.y + this.amp * Mathf.Sin(this.radY);
            float z		= tt.z + amp1 * Mathf.Cos( this.radX );//横

            mPosition.x = x;// - mPosition.x) / 20f;
            mPosition.y = y;// - mPosition.y) / 20f;
            mPosition.z = z;// - mPosition.z) / 20f;
            transform.position = mPosition;
            //transform

            transform.LookAt(Vector3.zero);

        //}

        this.radX += 0.05f * Time.deltaTime;
    }
}