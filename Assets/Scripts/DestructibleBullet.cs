using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBullet : MonoBehaviour {
    public void OnZeroHealth() {
        Destroy(gameObject);
    }
}
