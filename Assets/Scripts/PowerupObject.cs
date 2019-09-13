using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupObject : MonoBehaviour {
    public float activeDuration = 10;

    public void Activate() {
        transform.GetChild(0).gameObject.SetActive(true);
        Invoke(nameof(Deactivate), 10);
    }

    private void Deactivate() {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
