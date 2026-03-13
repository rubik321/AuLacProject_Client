using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    public List<Material> skyMaterials;

    public int index = 0;
    public float countDelay = 0;

    void Start()
    {
    
    }

    // Update is called once per frame
    private void FixedUpdate() {

        this.countDelay -= Time.fixedDeltaTime;
        if(this.countDelay > 0) return;
        if(this.countDelay <= 0) RenderSettings.skybox = skyMaterials[this.index % this.skyMaterials.Count];
        this.index ++;
        this.countDelay = 2;
    }
}
