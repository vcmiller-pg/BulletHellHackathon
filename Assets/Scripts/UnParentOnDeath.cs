using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnParentOnDeath : MonoBehaviour {
    public GameObject target;

    private void OnDestroy() {
        target.transform.parent = null;
    }
}
