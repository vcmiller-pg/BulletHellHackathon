using UnityEngine;
using SBR;

public class FighterChannels : SBR.Channels {
    public FighterChannels() {
        RegisterInputChannel("movement", new Vector3(0, 0, 0), true);
        RegisterInputChannel("attack1", false, false);
        RegisterInputChannel("weaponSwitch", 0, true);

    }
    

    public Vector3 movement {
        get {
            return GetInput<Vector3>("movement");
        }

        set {
            SetVector("movement", value);
        }
    }

    public bool attack1 {
        get {
            return GetInput<bool>("attack1");
        }

        set {
            SetInput("attack1", value);
        }
    }

    public int weaponSwitch {
        get {
            return GetInput<int>("weaponSwitch");
        }

        set {
            SetInt("weaponSwitch", value);
        }
    }

}
