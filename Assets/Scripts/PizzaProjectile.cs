using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaProjectile : Projectile {
    private BoxCollider box;
    private PizzaMesh[] meshes;

	// Use this for initialization
	void Start () {
        box = FindObjectOfType<PlayerShip>()?.GetComponentInChildren<BoxCollider>();
        meshes = GetComponentsInChildren<PizzaMesh>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (!box || !box.enabled) {
            return;
        }

        foreach (var mesh in meshes) {
            if (Intersects(mesh)) {
                OnHitCollider(box, box.transform.position);
            }
        }
	}

    private bool Intersects(PizzaMesh mesh) {
        Vector3[] points = new Vector3[4];

        Vector3 pos = box.center + box.transform.position;
        Vector3 extent = box.size / 2.0f;

        points[0] = pos + new Vector3(extent.x, 0, extent.z);
        points[1] = pos + new Vector3(-extent.x, 0, extent.z);
        points[2] = pos + new Vector3(extent.x, 0, -extent.z);
        points[3] = pos + new Vector3(-extent.x, 0, -extent.z);

        Vector3 fwdAxis = transform.forward;
        fwdAxis.y = 0;
        fwdAxis = fwdAxis.normalized;
        fwdAxis = Quaternion.AngleAxis(mesh.angle, Vector3.up) * fwdAxis;

        float cosAngle = Mathf.Cos(mesh.arc * Mathf.Deg2Rad / 2.0f);

        // First check for points from the box that are inside the pizza projectile.
        // There are two conditions for this: the point must be within the range of the arc,
        // and the point's distance to the pizza origin must be within its current distance range.
        float rMax = mesh.elapsedTime * mesh.speed;
        float rMin = Mathf.Max(rMax - mesh.splitSize, 0.0f);
        foreach (var point in points) {
            Vector3 toPoint = point - transform.position;
            toPoint.y = 0;

            float dot = Vector3.Dot(toPoint.normalized, fwdAxis);
            if (dot > cosAngle) {
                float dist2 = Vector3.SqrMagnitude(point - transform.position);
                if (dist2 > rMin * rMin && dist2 < rMax * rMax) {
                    return true;
                }
            }
        }

        // If no points in the box were inside the arc, now check if corners of the arc are inside the box.
        Vector3 boxOrigin = points[0];
        Vector3 boxAxis1 = points[1] - points[0];
        Vector3 boxAxis2 = points[2] - points[0];
        float limit1 = boxAxis1.sqrMagnitude;
        float limit2 = boxAxis2.sqrMagnitude;

        foreach (var point in mesh.corners) {
            Vector3 pointInBox = point - boxOrigin;
            float dot1 = Vector3.Dot(pointInBox, boxAxis1);
            float dot2 = Vector3.Dot(pointInBox, boxAxis2);

            if (dot1 >= 0 && dot2 >= 0 && dot1 <= limit1 && dot2 <= limit2) {
                return true;
            }
        }

        return false;
    }
}
