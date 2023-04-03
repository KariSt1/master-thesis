using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryData
{
    public List<Vector3> trajectoryPoints;

    public TrajectoryData() {
        trajectoryPoints = new List<Vector3>();
    }

    public void AddTrajectoryPoint(Vector3 point) {
        trajectoryPoints.Add(point);
    }
}
