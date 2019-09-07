using UnityEngine;
using SBR;

public class FighterChannels : SBR.Channels {

    private Vector3 _movement;
    public Vector3 movement {
        get { return _movement; }
        set {
            _movement = value;
        }
    }

    private bool _attack1;
    public bool attack1 {
        get { return _attack1; }
        set {
            _attack1 = value;
        }
    }

    private int _weaponSwitch;
    public int weaponSwitch {
        get { return _weaponSwitch; }
        set {
            _weaponSwitch = value;
        }
    }


    public override void ClearInput(bool force = false) {
        base.ClearInput(force);
        _movement = new Vector3(0f, 0f, 0f);
        if (force) _attack1 = false;
        _weaponSwitch = 0;

    }
}
