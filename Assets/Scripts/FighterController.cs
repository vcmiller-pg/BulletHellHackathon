using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class FighterController : PlayerController<FighterChannels> {

    public float weaponSwitchDelay = 0.5f;

    private CooldownTimer switchTimer;

    protected override void Awake() {
        base.Awake();

        AddAxisListener("Horizontal", Axis_Horizontal);
        AddAxisListener("Vertical", Axis_Vertical);
        AddAxisListener("WeaponSwitch", Axis_WeaponSwitch);
        AddButtonDownListener("Fire1", ButtonDown_Fire1);
        AddButtonUpListener("Fire1", ButtonUp_Fire1);
    }

    protected override void DoInput() {
        base.DoInput();

        bool touch;
        Vector2 touchPos;
        if (!Application.isEditor && Application.isMobilePlatform) {
            touch = Input.touchCount > 0;
            touchPos = touch ? Input.GetTouch(0).position : Vector2.zero;
        } else {
            touch = true;
            touchPos = Input.mousePosition;
        }

        if (touch) {
            Ray ray = viewTarget.camera.ScreenPointToRay(touchPos);
            Plane plane = new Plane(Vector3.up, transform.position);
            if (plane.Raycast(ray, out float enter)) {
                Vector3 targetPoint = ray.GetPoint(enter);
                channels.movement = targetPoint - transform.position;
            }
        }
    }

    public void Axis_Horizontal(float value) {
        channels.movement += Vector3.right * value;
    }

    public void Axis_Vertical(float value) {
        channels.movement += Vector3.forward * value;
    }

    public void Axis_WeaponSwitch(float value) {
        if (value != 0) {
            if (switchTimer == null) {
                switchTimer = new CooldownTimer(weaponSwitchDelay);
                channels.weaponSwitch = (int)Mathf.Sign(value);
            } else if (switchTimer.Use()) {
                channels.weaponSwitch = (int)Mathf.Sign(value);
            }
        } else {
            switchTimer = null;
        }
    }

    public void ButtonDown_Fire1() {
        channels.attack1 = true;
    }

    public void ButtonUp_Fire1() {
        channels.attack1 = false;
    }
}
