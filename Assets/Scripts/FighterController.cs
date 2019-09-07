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
