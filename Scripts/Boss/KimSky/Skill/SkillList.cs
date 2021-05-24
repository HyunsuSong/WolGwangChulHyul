using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillList : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefabScytheAttack = null;
    [SerializeField]
    private GameObject _slotScytheAttack = null;
    [SerializeField]
    private GameObject prefabMainEnergyBall = null;
    [SerializeField]
    private GameObject slotEnergyBall = null;

    private void Awake()
    {
        
    }

    private void ScytheAttack()
    {
        GameObject _scytheAttack = Instantiate(_prefabScytheAttack, _slotScytheAttack.transform.position, _slotScytheAttack.transform.rotation);
        _scytheAttack.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f), Space.Self);
    }
    private void SettingEnergyBall()
    {
        Instantiate(prefabMainEnergyBall, slotEnergyBall.transform.position, slotEnergyBall.transform.rotation);
    }

    private void FireEnergyBall()
    {
        if (transform.GetComponent<HealthByStone>().GetPhase() == 4)
        {
            StartCoroutine(EnergyBallRoutine(0.3f));
        }
        else
        {
            StartCoroutine(EnergyBallRoutine(0.0f));
        }
    }

    IEnumerator EnergyBallRoutine(float seconds)
    {
        MainEnergyBall[] mainEnergyballs = FindObjectsOfType<MainEnergyBall>();
        SubEnergyBall[] subEnergyballs = FindObjectsOfType<SubEnergyBall>();

        foreach (MainEnergyBall main in mainEnergyballs)
        {
            if (main != null && !main.GetComponent<MainEnergyBall>().IsFire)
            {
                main.GetComponent<MainEnergyBall>().IsFire = true;
            }
        }

        yield return new WaitForSeconds(seconds);

        for (int i = subEnergyballs.Length - 1; i >= 0; i--)
        {
            if (subEnergyballs[i] != null && !subEnergyballs[i].GetComponent<SubEnergyBall>().IsFire)
            {
                subEnergyballs[i].GetComponent<SubEnergyBall>().IsFire = true;
                yield return new WaitForSeconds(seconds);
            }
        }
    }
}
