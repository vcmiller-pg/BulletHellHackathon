using SBR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningWeapon : ShipWeapon {
    private ParticleSystem effect;
    private ParticleSystem.EmissionModule emission;
    private ParticleSystem.Particle[] particlesBuffer;
    private Collider[] spherecastBuffer;
    private Transform[] currentChain;
    public float damagePerSecond = 30;
    public ParticleSystem[] chain;
    public float chainRadius;
    public LayerMask hitMask;
    public float range = 100;

    protected override void Awake() {
        base.Awake();
        effect = GetComponent<ParticleSystem>();
        emission = effect.emission;
        particlesBuffer = new ParticleSystem.Particle[effect.main.maxParticles];
        spherecastBuffer = new Collider[8];
        currentChain = new Transform[chain.Length + 1];
    }

    protected override void DoOutput(FighterChannels channels) {
        emission.enabled = channels.attack1;

        if (channels.attack1) {
            float hitDistance = range;
            bool didHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, hitMask);
            if (didHit) {
                hitDistance = hit.distance;

                hit.collider.Damage(damagePerSecond * Time.deltaTime);
            }

            SetDistance(hitDistance);
            Vector3 hitPos = transform.position;

            if (didHit) {
                hitPos = hit.transform.position;
                currentChain[0] = hit.transform.root;
            }
            for (int i = 0; i < currentChain.Length; i++) {
                currentChain[i] = null;
            }
            for (int i = 0; i < chain.Length; i++) {
                if (didHit) {
                    didHit = false;

                    int chainHitCount = Physics.OverlapSphereNonAlloc(hitPos, chainRadius, spherecastBuffer, hitMask);
                    if (chainHitCount > 0) {
                        Transform chainHitObject = null;
                        float minDist = float.PositiveInfinity;
                        for (int j = 0; j < chainHitCount; j++) {
                            if (Array.IndexOf(currentChain, spherecastBuffer[j].transform.root) >= 0) {
                                continue;
                            }

                            float chainDist = Vector3.SqrMagnitude(hitPos - spherecastBuffer[j].transform.position);
                            if (chainDist < minDist) {
                                minDist = chainDist;
                                chainHitObject = spherecastBuffer[j].transform;
                            }
                        }

                        if (chainHitObject) {
                            chainHitObject.Damage(damagePerSecond * Time.deltaTime);
                            currentChain[i + 1] = chainHitObject;
                            chain[i].gameObject.SetActive(true);
                            chain[i].transform.position = hitPos;
                            chain[i].transform.forward = chainHitObject.position - hitPos;
                            var main2 = chain[i].main;
                            main2.startLifetime = Mathf.Sqrt(minDist) / main2.startSpeed.constant;
                            didHit = true;
                            hitPos = chainHitObject.position;
                        }
                    }
                }

                if (!didHit) {
                    chain[i].gameObject.SetActive(false);
                }
            }
        } else {
            foreach (var item in chain) {
                item.gameObject.SetActive(false);
            }
        }
    }

    private void SetDistance(float distance) {

        var main = effect.main;
        main.startLifetime = distance / main.startSpeed.constant;

        int count = effect.GetParticles(particlesBuffer);
        int rem = 0;
        for (int i = 0; i < count; i++) {
            if (rem > 0) {
                particlesBuffer[i] = particlesBuffer[i + rem];
            }

            var p = particlesBuffer[i];
            float timeAlive = p.startLifetime - p.remainingLifetime;
            if (timeAlive > main.startLifetime.constant) {
                count--;
                rem++;

                if (i < count) {
                    particlesBuffer[i] = particlesBuffer[i + rem];
                }
            }
        }

        if (rem > 0) {
            effect.SetParticles(particlesBuffer, count);
        }
    }
}
