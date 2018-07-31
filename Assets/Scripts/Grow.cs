using UnityEngine;

public class Grow : MonoBehaviour {
    public float growSpeed = 10;
    public float startSize;
    private Vector3 originalScale;

	// Use this for initialization
	void Start () {
        originalScale = transform.localScale;
        transform.localScale = new Vector3(startSize, startSize, startSize);
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.MoveTowards(transform.localScale, originalScale, growSpeed * Time.deltaTime);
	}
}
