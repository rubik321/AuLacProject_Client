using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTPortalsFX2_AxisScaleByTime : PortalsFX2_AxisRotateByTime
{
    public bool useScale = false;
    public float speedScale = 1f;
    public Vector3 minScale = new Vector3(0,0,0);
    public Vector3 maxScale = new Vector3(0,0,0);

    private int capX = 1;
    private int capY = 1;
    private int capZ = 1;

    protected override void Update() {
        base.Update();
        this.Scale();
    }

    public void Scale(){
        if(!this.useScale) return;
        float x = transform.localScale.x + this.speedScale*Time.deltaTime * capX;
        float y = transform.localScale.y + this.speedScale*Time.deltaTime * capY;
        float z = transform.localScale.z + this.speedScale*Time.deltaTime * capZ;

        if(x >= this.maxScale.x || x < this.minScale.x){
            this.capX = - this.capX;
            x += this.speedScale*Time.deltaTime * capX;
        }
        if(y >= this.maxScale.y || y < this.minScale.y){
            this.capY = - this.capY;
            y += this.speedScale*Time.deltaTime * capY;
        }
        if(z >= this.maxScale.z || x < this.minScale.z){
            this.capZ = - this.capZ;
            z +=this.speedScale*Time.deltaTime * capZ;
        }
        transform.localScale = new Vector3(x, y, z);
    }
}
