using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaMesh : MonoBehaviour {
    public float arc = 20;
    public float angle = 0;
    public int numPoints = 10;
    public float speed = 1;
    public float splitSize = 10;
    public float uvScale = 0.1f;

    public float elapsedTime { get; private set; }

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private MeshFilter mf;
    private MeshCollider mc;

	// Use this for initialization
	void Start () {
        mesh = new Mesh();

        vertices = new Vector3[numPoints * 4];
        uvs = new Vector2[numPoints * 4];

        mesh.vertices = vertices;
        mesh.uv = uvs;

        int[] indices = new int[numPoints * 8];
        for (int i = 0; i < numPoints - 1; i++) {
            indices[i * 4 + 0] = i * 2 + 0;
            indices[i * 4 + 1] = i * 2 + 1;
            indices[i * 4 + 2] = i * 2 + 3;
            indices[i * 4 + 3] = i * 2 + 2;


            indices[i * 4 + 0 + numPoints * 4] = i * 2 + 1 + numPoints * 2;
            indices[i * 4 + 1 + numPoints * 4] = i * 2 + 0 + numPoints * 2;
            indices[i * 4 + 2 + numPoints * 4] = i * 2 + 2 + numPoints * 2;
            indices[i * 4 + 3 + numPoints * 4] = i * 2 + 3 + numPoints * 2;
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.SetIndices(indices, MeshTopology.Quads, 0);

        mf = GetComponent<MeshFilter>();
        mc = GetComponent<MeshCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;

        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
        Vector2 uvDir = new Vector2(dir.x, dir.z) * uvScale;

        float angleStep = arc / (numPoints - 1);
        float angleStart = -arc / 2.0f;

        for (int i = 0; i < numPoints; i++) {
            float angle = angleStart + angleStep * i;
            Vector3 v = Quaternion.AngleAxis(angle, Vector3.up) * dir;
            Vector2 uv = new Vector2(v.x, v.z) * uvScale;

            float size = elapsedTime * speed;
            vertices[i * 2 + 1] = v * size;
            uvs[i * 2 + 1] = uv * size - uvDir * size;

            float heExpand = Mathf.Max(size - splitSize, 0);
            vertices[i * 2] = v * heExpand;
            uvs[i * 2] = -uvDir * size + uv * heExpand;

            vertices[i * 2 + 0 + numPoints * 2] = vertices[i * 2 + 0] - Vector3.up;
            vertices[i * 2 + 1 + numPoints * 2] = vertices[i * 2 + 1] - Vector3.up;
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.RecalculateBounds();

        mf.sharedMesh = mesh;
        mc.sharedMesh = mesh;
	}
}
