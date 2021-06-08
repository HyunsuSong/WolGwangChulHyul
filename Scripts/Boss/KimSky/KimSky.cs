using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//김갑천의 AI 로직 스크립트임
public class KimSky : MonoBehaviour
{
    [SerializeField]
    private float skyAttackDelay = 5.0f;
    [SerializeField]
    private float skyRotationSpeed = 1.0f;
    [SerializeField]
    private float waitTimeSecond = 30.0f;
    [SerializeField]
    private float waitTimeThird = 20.0f;
    [SerializeField]
    private float waitTimeFourth = 15.0f;
    [SerializeField]
    private float skyViewRange = 15.0f;

    private GameObject playerObject;
    private KimSkyAnimList skyAnimList;

    private bool isSkyBorn = false;
    private bool canUpdate = true;
    public bool endCoroutine = false;

    private Vector3 spawnPosition = Vector3.zero;
    private Vector3 playerPosition;
    private float betweenLength;

    private float attackSuccessTimer = 0.0f;
    private Dictionary<string, int>[] skillsByPhase = new Dictionary<string, int>[4];

    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        spawnPosition = transform.position;
        transform.position = new Vector3(transform.position.x, -1000.0f, transform.position.z);

        if (GetComponent<KimSkyAnimList>() != null)
        {
            skyAnimList = GetComponent<KimSkyAnimList>();
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
        get
        {
            return endCoroutine;
        }

        set
        {
            endCoroutine = value;
        }
    }

    public bool IsSkyBorn()
    {
        return isSkyBorn;
    }

    //초기 페이즈별 스킬 확률
    private void SetSkillLIst()
    {
        //낫 공격의 경우 70%임. 확률 수정하고 싶을 경우 이곳을 수정하면 됨
        //각 페이즈별 모든 스킬의 확률 합은 100.
        skillsByPhase[0] = new Dictionary<string, int>();
        skillsByPhase[0].Add("AnimScytheAttack", 70);
        skillsByPhase[0].Add("AnimFireEnergyBall", 30);
        skillsByPhase[0].Add("AnimShockWave", 0);
        skillsByPhase[0].Add("AnimEnergyBeam", 0);

        skillsByPhase[1] = new Dictionary<string, int>();
        skillsByPhase[1].Add("AnimScytheAttack", 40);
        skillsByPhase[1].Add("AnimFireEnergyBall", 30);
        skillsByPhase[1].Add("AnimShockWave", 0);
        skillsByPhase[1].Add("AnimEnergyBeam", 30);

        skillsByPhase[2] = new Dictionary<string, int>();
        skillsByPhase[2].Add("AnimScytheAttack", 40);
        skillsByPhase[2].Add("AnimFireEnergyBall", 30);
        skillsByPhase[2].Add("AnimShockWave", 0);
        skillsByPhase[2].Add("AnimEnergyBeam", 30);

        skillsByPhase[3] = new Dictionary<string, int>();
        skillsByPhase[3].Add("AnimScytheAttack", 40);
        skillsByPhase[3].Add("AnimFireEnergyBall", 35);
        skillsByPhase[3].Add("AnimShockWave", 0);
        skillsByPhase[3].Add("AnimEnergyBeam", 25);
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
                skyAnimList.StartCoroutine("AnimBorn");
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

    //일정 시간 이상 맞지 않았을 경우 스킬들 확률 조정하는 구간
    private void CheckConditionalSkills()
    {
        attackSuccessTimer += Time.deltaTime;

        switch(GetComponent<HealthByStone>().GetPhase())
        {
            //case 2는 2페이즈, 3은 3페이즈, 4는 4페이즈
            case 2:
                if (attackSuccessTimer >= waitTimeSecond)
                {
                    //확률 수정하고 싶을 경우 = 뒤에 있는 숫자 수정
                    //모든 스킬의 확률 합은 100.
                    skillsByPhase[1]["AnimScytheAttack"] = 35;
                    skillsByPhase[1]["AnimFireEnergyBall"] = 25;
                    skillsByPhase[1]["AnimShockWave"] = 15;
                    skillsByPhase[1]["AnimEnergyBeam"] = 25;
                    attackSuccessTimer = 0.0f;
                }
                break;

            case 3:
                if (attackSuccessTimer >= waitTimeThird)
                {
                    skillsByPhase[2]["AnimScytheAttack"] = 35;
                    skillsByPhase[2]["AnimFireEnergyBall"] = 25;
                    skillsByPhase[2]["AnimShockWave"] = 15;
                    skillsByPhase[2]["AnimEnergyBeam"] = 25;
                    attackSuccessTimer = 0.0f;
                }
                break;

            case 4:
                if (attackSuccessTimer >= waitTimeFourth)
                {
                    skillsByPhase[3]["AnimScytheAttack"] =30;
                    skillsByPhase[3]["AnimFireEnergyBall"] = 25;
                    skillsByPhase[3]["AnimShockWave"] = 30;
                    skillsByPhase[3]["AnimEnergyBeam"] = 15;
                    attackSuccessTimer = 0.0f;
                }
                break;

            default:
                break;
        }
        
    }

    //공격 성공했을 경우 원래대로 스킬 확률이 돌아오는 로직

    public void AttackSuccess()
    {
        attackSuccessTimer = 0.0f;

        switch (GetComponent<HealthByStone>().GetPhase())
        {
            case 2:
                skillsByPhase[1]["AnimScytheAttack"] =  40;
                skillsByPhase[1]["AnimFireEnergyBall"] = 30;
                skillsByPhase[1]["AnimShockWave"] = 0;
                skillsByPhase[1]["AnimEnergyBeam"] = 30;
                break;

            case 3:
                skillsByPhase[2]["AnimScytheAttack"] = 40;
                skillsByPhase[2]["AnimFireEnergyBall"] = 30;
                skillsByPhase[2]["AnimShockWave"] = 0;
                skillsByPhase[2]["AnimEnergyBeam"] = 30;
                break;

            case 4:
                skillsByPhase[3]["AnimScytheAttack"] = 40;
                skillsByPhase[3]["AnimFireEnergyBall"] = 35;
                skillsByPhase[3]["AnimShockWave"] = 0;
                skillsByPhase[3]["AnimEnergyBeam"] = 25;
                break;

            default:
                break;
        }
    }

    private void SetActiveUI()
    {
        PlayerUI kimSkyUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();

        kimSkyUI.SetActiveUI();
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
