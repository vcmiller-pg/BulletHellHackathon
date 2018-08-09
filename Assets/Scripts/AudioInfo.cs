using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

[System.Serializable]
public class AudioInfo {
    public AudioClip clip;
    public AudioClip[] clips;
    public bool randomized;
    public float volume = 1;
    public float pitch = 1;
    public float spaital = 0;
    public bool loop = false;
    public float playCooldown = 0;
    public string cooldownId;

    private CooldownTimer playTimer;
    private static Dictionary<string, float> lastPlayTimes = new Dictionary<string, float>();

    private bool CanPlay() {
        if (playCooldown == 0) {
            return true;
        } else if (string.IsNullOrEmpty(cooldownId)) {
            if (playTimer == null) {
                playTimer = new CooldownTimer(playCooldown);
            }
            return playTimer.Use();
        } else {
            if (!lastPlayTimes.ContainsKey(cooldownId) || Time.time - lastPlayTimes[cooldownId] > playCooldown) {
                lastPlayTimes[cooldownId] = Time.time;
                return true;
            } else {
                return false;
            }
        }
    }

    public void Play(Vector3 point, Transform attach = null) {
        if (clip && CanPlay()) {
            Util.PlayClipAtPoint(clip, point, volume, spaital, loop, attach).pitch = pitch;
        }
    }
}
