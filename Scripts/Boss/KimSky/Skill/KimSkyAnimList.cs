using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimSkyAnimList : MonoBehaviour
{
    private KimSky kimSkyAiScript;
    private Animator kimSkyAnimator;

    private void Awake()
    {
        kimSkyAiScript = GetComponent<KimSky>();
        kimSkyAnimator = GetComponent<Animator>();
    }

    public IEnumerator Anim_Born()
    {
        yield return new WaitForSeconds(4.0f);
        GetComponent<KimSkyPassive>().CanUpdate = true;
        GetComponent<KimSky>().EndCoroutine = true;
    }

    public IEnumerator Anim_ScytheAttack()
    {
        kimSkyAnimator.SetTrigger("skill_02");
        Debug.Log("낫 공격 시작");

        yield return new WaitForSeconds(1.933f);
        yield return new WaitForSeconds(kimSkyAiScript.GetSkyAttackDelay());
        GetComponent<KimSky>().EndCoroutine = true;
    }

    public IEnumerator Anim_FireEnergyBall()
    {
        kimSkyAnimator.SetTrigger("skill_03");
        Debug.Log("에너지 볼 발사 시작");

        yield return new WaitForSeconds(1.900f);
        yield return new WaitForSeconds(kimSkyAiScript.GetSkyAttackDelay());
        GetComponent<KimSky>().EndCoroutine = true;
    }

    public IEnumerator Anim_ShockWave()
    {
        //충격파 타이머를 0으로 바꿔줘야 함.
        //졸려서 잠 이게 최신.
        kimSkyAnimator.SetTrigger("skill_04");
        Debug.Log("충격파 시작");

        yield return new WaitForSeconds(3.333f);
        yield return new WaitForSeconds(kimSkyAiScript.GetSkyAttackDelay());
        GetComponent<KimSky>().EndCoroutine = true;
    }

    public IEnumerator AnimBerserk()
    {
        //특정 시점마다 들어가는 코루틴이기 때문에 EndCoroutine 판별의 필요x
        GetComponent<Animator>().SetTrigger("skill_07");
        yield return new WaitForSeconds(2.333f);
    }

    public IEnumerator AnimChangeAttribute()
    {
        //특정 시점마다 들어가는 코루틴이기 때문에 EndCoroutine 판별의 필요x
        GetComponent<Animator>().SetTrigger("skill_01");
        yield return new WaitForSeconds(1.333f);
    }
}
