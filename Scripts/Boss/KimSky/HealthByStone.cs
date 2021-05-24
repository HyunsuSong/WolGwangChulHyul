using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WolKwangChulHyeol.Core;

public class HealthByStone : MonoBehaviour
{
    private SkyStoneInfo skyStoneInfo = null;

    private float skyMaxHealth;
    private float skyCurrentHealth;
    private float skyTempHealth = 0.0f;

    [SerializeField]
    private int skyPhase = 1;
    private bool isDead = false;

    void Awake()
    {
        if (skyStoneInfo == null)
        {
            skyStoneInfo = GameObject.Find("KimSkyStoneInfo").GetComponent<SkyStoneInfo>();
        }

        SetHealth();
    }

    void Update()
    {
        if (!GetIsDead())
        {
            UpdateHealth();
            SetPhase();
        }
    }

    private void SetHealth()
    {
        for (int i = 0; i < skyStoneInfo.bossKimSkyStoneMaxCount; i++)
        {
            if (skyStoneInfo.bossKimSkyStone[i] != null)
            {
                skyMaxHealth += skyStoneInfo.bossKimSkyStone[i].GetComponent<Health>().MHealthCurrentPoints;
            }
        }

        skyCurrentHealth = skyMaxHealth;
    }
    private void UpdateHealth()
    {
        skyTempHealth = 0.0f;

        for (int i = 0; i < skyStoneInfo.bossKimSkyStoneMaxCount; i++)
        {
            if (skyStoneInfo.bossKimSkyStone[i] != null)
            {
                skyTempHealth += skyStoneInfo.bossKimSkyStone[i].GetComponent<Health>().MHealthCurrentPoints;
            }
        }
        skyCurrentHealth = Mathf.Max(skyTempHealth, 0);

        if (skyCurrentHealth <= 0.0f)
        {
            isDead = true;
        }
    }
    private void SetPhase()
    {
        skyPhase = (skyStoneInfo.bossKimSkyStoneMaxCount + 1) - skyStoneInfo.bossKimSkyStoneCurCount;
    }
    public int GetPhase()
    {
        return skyPhase;
    }
    public float GetSkyCurrentHealth()
    {
        return skyCurrentHealth;
    }
    public float GetSkyMaxHealth()
    {
        return skyMaxHealth;
    }
    public bool GetIsDead()
    {
        return isDead;
    }
}
