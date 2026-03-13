using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GOA.WorldMap;
using NTFunctions_old;

namespace NTFunctions_old
{
    public class FPSDisplay : LoadBehaviour
    {
        public TextMeshProUGUI fpsDisPlay;
        public TextMeshProUGUI FpsDisPlay{
            get {
                this.LoadFpsDisPlay();
                return this.fpsDisPlay;
            }
        }


        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadFpsDisPlay();
        }

        protected void LoadFpsDisPlay(){
            if(this.fpsDisPlay != null) return;
            this.fpsDisPlay = transform.GetComponent<TextMeshProUGUI>();
        }

        public float pollingTime = 1f;
        public float time = 0;
        public int frameCount = 0;
        protected override void Update() {
            time += Time.deltaTime;
            frameCount++;
            if(this.time >= this.pollingTime){
                int frameRate = Mathf.RoundToInt(this.frameCount / time);
                this.FpsDisPlay.text = "FPS "+ frameRate.ToString();
                time -= pollingTime;
                frameCount = 0;
            }
        }
    }
}