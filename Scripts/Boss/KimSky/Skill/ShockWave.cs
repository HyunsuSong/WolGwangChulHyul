using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WolKwangChulHyeol.Core;
using HyunSu.Core;

public class ShockWave : Skill
{
    [SerializeField]
    private GameObject[] effectsShockWave;
    private ParticleSystem shockWaveParticleSystem;
    private ParticleSystem.Particle[] currentParticles;

    private float triggerRadius;

    private void Awake()
    {
        if (userObject == null)
        {
            base.userObject = GameObject.FindGameObjectWithTag("KimSky");
            base.skillAttribute = userObject.GetComponent<sampleAttribute>().myAttributeState;
            effectsShockWave[base.skillAttribute].SetActive(true);
            base.baseSkillDamage = userObject.GetComponent<KimSkyPassive>().GetAttackPower();
        }
        if (base.targetObject == null)
        {
            base.targetObject = GameObject.FindGameObjectWithTag("Player");
        }

        shockWaveParticleSystem = effectsShockWave[base.skillAttribute].GetComponent<ParticleSystem>();
        currentParticles = new ParticleSystem.Particle[shockWaveParticleSystem.main.maxParticles];

        SetSkillDamage();

        //애니메이션 시간보다 여유 있게, 사운드 시간도 계산하여서 처리할 것.
        StartCoroutine(base.DestroySelf(5.0f));
    }

    private void Update()
    {
        base.Update();

        ScaleUpCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            base.ApplyDamage(other.gameObject, base.skillDamage, true);
        }
    }

    //조만간 오버라이딩 적용시킬 것
    private void SetSkillDamage()
    {
        var particleSystemMain = shockWaveParticleSystem.main;

        switch (userObject.GetComponent<HealthByStone>().GetPhase())
        {
            case 2:
                base.IntensifySkillDamage(base.baseSkillDamage, 0.75f);
                particleSystemMain.duration = 5.0f;
                particleSystemMain.startLifetime = 5.0f;
                break;

            case 3:
                base.IntensifySkillDamage(base.baseSkillDamage, 0.625f);
                particleSystemMain.duration = 3.5f;
                particleSystemMain.startLifetime = 3.5f;
                break;

            case 4:
                base.IntensifySkillDamage(base.baseSkillDamage, 0.5f);
                particleSystemMain.duration = 2.0f;
                particleSystemMain.startLifetime = 2.0f;
                break;

            default:
                break;
        }
    }

    private void ScaleUpCollider()
    {
        //파티클 갯수 구하는 변수, for문으로 돌릴때 (maxParticles가 2개 이상일 경우)
        int particlesLength = shockWaveParticleSystem.GetParticles(currentParticles);

        triggerRadius = currentParticles[0].GetCurrentSize(shockWaveParticleSystem);
        GetComponent<SphereCollider>().radius = triggerRadius / 10.0f;
    }
}
