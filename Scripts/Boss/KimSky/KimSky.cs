using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//김갑천의 AI 로직 스크립트임
public class KimSky : MonoBehaviour
{
    [SerializeField]
    private float skyViewRange = 15.0f;
    [SerializeField]
    private float skyAttackDelay = 5.0f;
    private float skyRotationSpeed = 3.0f;

    private GameObject playerObject;
    private KimSkyAnimList skyAnimList;

    private bool isSkyBorn = false;
    private bool canUpdate = true;
    private bool endCoroutine = false;

    private Vector3 spawnPosition = Vector3.zero;
    private Vector3 playerPosition;
    private float betweenLength;

    private float attackSuccessTimer = 0.0f;
    private bool canShockWave = false;

    Dictionary<string, int>[] skillsByPhase = new Dictionary<string, int>[4];

    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        spawnPosition = transform.position;
        transform.position = new Vector3(transform.position.x, -1000.0f, transform.position.z);

        if (GetComponent<KimSkyAnimList>() != null)
        {
            skyAnimList = GetComponent<KimSkyAnimList>();
        }
        else
        {
            Debug.Break();
        }

        SetSkillLIst();
    }

    void Update()
    {
        if (canUpdate)
        {
            if (GetComponent<HealthByStone>().GetIsDead())
            {
                canUpdate = false;
                GetComponent<KimSkyPassive>().CanUpdate = true;

                StopAllCoroutines();
                GetComponent<Animator>().SetTrigger("isDead");
            }
            else
            {
                if (FindPlayer())
                {
                    CheckConditionalSkills();

                    if (endCoroutine)
                    {
                        StartCoroutine("PatternByPhase");
                        Debug.Log("코루틴 진입");
                    }
                }
            }
        }
    }

    public float GetSkyAttackDelay()
    {
        return skyAttackDelay;
    }

    public bool EndCoroutine
    {
        get { return endCoroutine; }
        set { endCoroutine = value; }
    }

    public bool IsSkyBorn()
    {
        return isSkyBorn;
    }

    private void SetSkillLIst()
    {
        skillsByPhase[0] = new Dictionary<string, int>();
        skillsByPhase[0].Add("Anim_ScytheAttack", 70);
        skillsByPhase[0].Add("Anim_FireEnergyBall", 30);

        skillsByPhase[1] = new Dictionary<string, int>();
        skillsByPhase[1].Add("Anim_ScytheAttack", 60);
        skillsByPhase[1].Add("Anim_FireEnergyBall", 40);
        skillsByPhase[1].Add("Anim_ShockWave", 0);

        skillsByPhase[2] = new Dictionary<string, int>();
        skillsByPhase[2].Add("Anim_ScytheAttack", 50);
        skillsByPhase[2].Add("Anim_FireEnergyBall", 50);

        skillsByPhase[3] = new Dictionary<string, int>();
        skillsByPhase[3].Add("Anim_ScytheAttack", 0);
        skillsByPhase[3].Add("Anim_FireEnergyBall", 0);
    }

    private bool FindPlayer()
    {
        playerPosition = playerObject.transform.position;
        betweenLength = Mathf.Sqrt(Mathf.Pow(transform.position.x - playerPosition.x, 2) + Mathf.Pow(transform.position.z - playerPosition.z, 2));

        if (betweenLength <= skyViewRange)
        {
            if (!isSkyBorn)
            {
                GetComponent<Animator>().SetTrigger("isBorn");
                transform.position = spawnPosition;
                StartCoroutine(skyAnimList.Anim_Born());
                isSkyBorn = true;
            }
            else
            {
                GetComponent<Animator>().SetBool("isBattle", true);

                Vector3 toTarget = playerPosition - transform.position;
                Vector3 toLook = Vector3.Slerp(new Vector3(transform.forward.x, 0.0f, transform.forward.z),
                                               new Vector3(toTarget.normalized.x, 0.0f, toTarget.normalized.z),
                                               skyRotationSpeed * Time.deltaTime);

                transform.rotation = Quaternion.LookRotation(toLook, Vector3.up);
            }
            return true;
        }
        else
        {
            GetComponent<Animator>().SetBool("isBattle", false);

            return false;
        }
    }

    //해당 스킬쪽에서 못함 ㅇㅇ.
    //스킬 리스트를 수정해야 하기 때문임.
    private void CheckConditionalSkills()
    {
        attackSuccessTimer += Time.deltaTime;
        Debug.Log(attackSuccessTimer);
        switch(GetComponent<HealthByStone>().GetPhase())
        {
            case 2:
                if (attackSuccessTimer >= 30.0f)
                {
                    skillsByPhase[1]["Anim_ScytheAttack"] = 55;
                    skillsByPhase[1]["Anim_FireEnergyBall"] = 35;
                    skillsByPhase[1]["Anim_ShockWave"] = 10;
                }
                break;

            case 3:
                if (attackSuccessTimer >= 20.0f)
                {
                    skillsByPhase[2]["Anim_ScytheAttack"] = 55;
                    skillsByPhase[2]["Anim_FireEnergyBall"] = 35;
                    skillsByPhase[2]["Anim_ShockWave"] = 10;
                }
                break;

            case 4:
                if (attackSuccessTimer >= 15.0f)
                {
                    skillsByPhase[3]["Anim_ScytheAttack"] =45;
                    skillsByPhase[3]["Anim_FireEnergyBall"] = 25;
                    skillsByPhase[3]["Anim_ShockWave"] = 30;
                }
                break;

            default:
                break;
        }
        
    }

    public void AttackSuccess()
    {
        attackSuccessTimer = 0.0f;

        //skillsByPhase[1]["Anim_ScytheAttack"] = 60;
        //skillsByPhase[1]["Anim_FireEnergyBall"] = 40;
        //skillsByPhase[1]["Anim_ShockWave"] = 0;
    }

    private void SetActiveUI()
    {
        //PlayerUI kimSkyUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();

        //kimSkyUI.SetActiveUI();
    }

    IEnumerator PatternByPhase()
    {
        endCoroutine = false;

        yield return new WaitForSeconds(0.0f);
        Debug.Log(GetComponent<HealthByStone>().GetPhase() + "페이즈 패턴");

        int num = Random.Range(1, 101);
        Debug.Log("확률 계산 완료" + num.ToString());

        int min = 0, max = 0;

        foreach (KeyValuePair<string, int> skill in skillsByPhase[GetComponent<HealthByStone>().GetPhase() - 1])
        {
            max += skill.Value;
            Debug.Log("현재 min 값 " + min.ToString());
            Debug.Log("현재 max 값 " + max.ToString());
            if (min <= num && num <= max)
            {
                Debug.Log("당첨 성공, " + skill.Key + " 스킬 실행");
                skyAnimList.StartCoroutine(skill.Key);
                yield break;
            }
            else
            {
                min = skill.Value;
                Debug.Log("당첨 실패, 다음 스킬로 검사함");
            }
        }

        endCoroutine = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, skyViewRange);
    }
}
