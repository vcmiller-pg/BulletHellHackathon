using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameBounds : MonoBehaviour {
    public Bounds bounds;

    private void Update() {
        Util.DrawDebugBounds(bounds, Color.red);
    }
}
