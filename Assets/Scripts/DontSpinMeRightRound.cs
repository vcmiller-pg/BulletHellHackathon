using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontSpinMeRightRound : MonoBehaviour {
	void LateUpdate () {
        transform.rotation = Quaternion.identity;
	}
}
