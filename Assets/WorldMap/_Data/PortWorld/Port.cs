using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;

public class Port : MonoBehaviour
{
    public Coordinates coordinates;
    public float rate = 1;
    public double dis = 0;
    public LocationManager locationManager;

    private void FixedUpdate() {
        this.UpdateData();
    }

    public void UpdateData(){
        if(this.coordinates == null) return;
        Vector3 pos = coordinates.convertCoordinateToVector(0);
        transform.position = pos;
        dis = coordinates.DistanceFromOtherGPSCoordinate(locationManager.currentLocation) * rate;
    }
}
