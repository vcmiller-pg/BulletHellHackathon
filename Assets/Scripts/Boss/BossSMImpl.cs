using UnityEngine;
using SBR;
using System.Collections.Generic;
using System.Linq;

public class BossSMImpl : BossSM {
    public GameObject[] guns;
    public GameObject[] lasers;

    public BossShieldArm arm1;
    public BossShieldArm arm2;

    public float cancelDamage = 200;

    private bool arm1Damaging;
    private bool arm2Damaging;
    private new FighterChannels channels;

    private Animator animator;

    private AnimationState _animation;
    private new AnimationState animation {
        get { return _animation; }
        set {
            if (value != _animation) {
                animator.Play(value.ToString());
                _animation = value;
            }
        }
    }

    public override void Initialize() {
        base.Initialize();

        animator = GetComponent<Animator>();
        channels = base.channels as FighterChannels;
    }

    protected override void StateEnter_Spawn() {
        animation = AnimationState.Spawn;
    }

    protected override void StateEnter_ArmAttack() {
        animation = AnimationState.ArmAttack;
    }

    protected override void StateEnter_BeamAttack() {
        animation = AnimationState.BeamAttack;
    }

    protected override void StateEnter_SpawnGuns() {
        foreach (var gun in guns) {
            Instantiate(gun, gun.transform.position, gun.transform.rotation).SetActive(true);
        }
    }

    protected override void StateEnter_SpawnLasers() {
        foreach (var laser in lasers) {
            Instantiate(laser, laser.transform.position, laser.transform.rotation).SetActive(true);
        }
    }

    protected override void State_ArmAttack() {
        if (arm1Damaging && arm1.damageTaken >= cancelDamage) {
            animation = AnimationState.ArmAttackFail1;
        } else if (arm2Damaging && arm2.damageTaken >= cancelDamage) {
            animation = AnimationState.ArmAttackFail2;
        }
    }

    protected override bool TransitionCond_ArmAttack_BeamAttack() {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime == 1;
    }

    public void AnimEvent_LeftArm(int value) {
        if (arm1) {
            arm1.shielded = value > 0;
            arm1Damaging = value < 0;
        }
    }

    public void AnimEvent_RightArm(int value) {
        if (arm2) {
            arm2.shielded = value > 0;
            arm2Damaging = value < 0;
        }
    }

    public enum AnimationState {
        Spawn, BeamAttack, ArmAttack, ArmAttackFail1, ArmAttackFail2
    }
}
