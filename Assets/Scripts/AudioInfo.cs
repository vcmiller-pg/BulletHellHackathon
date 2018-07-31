using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

[System.Serializable]
public class AudioInfo {
    public AudioClip clip;
    public float volume = 1;
    public float pitch = 1;
    public float spaital = 0;
    public bool loop = false;
    public float playCooldown = 0;

    private CooldownTimer playTimer;

    public void Play(Vector3 point, Transform attach = null) {
        if (playTimer == null && playCooldown > 0) {
            playTimer = new CooldownTimer(playCooldown);
        }
        
        if (clip && (playTimer == null || playTimer.Use())) {
            Util.PlayClipAtPoint(clip, point, volume, spaital, loop, attach).pitch = pitch;
        }
    }
}
