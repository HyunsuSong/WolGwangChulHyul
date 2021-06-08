using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyunSu.Core;

public class SkillList : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabScytheAttack = null;
    [SerializeField]
    private GameObject slotScytheAttack = null;
    [SerializeField]
    private GameObject prefabMainEnergyBall = null;
    [SerializeField]
    private GameObject slotEnergyBall = null;
    [SerializeField]
    private GameObject prefabShockWave = null;
    [SerializeField]
    private GameObject prefabEnergyBeam = null;

    private int activeEnergyBeamAttribute = 0;
    private float energyBeamOldEulerAngleX = 0.0f;

    private void Awake()
    {
        energyBeamOldEulerAngleX = prefabEnergyBeam.GetComponent<EnergyBeam>().MyOldEulerAngleX;
    }

    private void ScytheAttack()
    {
        GameObject _scytheAttack = Instantiate(prefabScytheAttack, slotScytheAttack.transform.position, slotScytheAttack.transform.rotation);
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

    private void FireShockWave()
    {
        StartCoroutine(ShockWaveRoutine(transform.GetComponent<HealthByStone>().GetPhase() - 1));
    }

    private void TurnOnEnergyBeam()
    {
        activeEnergyBeamAttribute = GetComponent<sampleAttribute>().myAttributeState;
        prefabEnergyBeam.SetActive(true);
        prefabEnergyBeam.GetComponent<EnergyBeam>().effectsEnergyBeam[activeEnergyBeamAttribute].SetActive(true);

    }

    private void TurnOffEnergyBeam()
    {
        prefabEnergyBeam.SetActive(false);
        prefabEnergyBeam.GetComponent<EnergyBeam>().effectsEnergyBeam[activeEnergyBeamAttribute].SetActive(false);
        prefabEnergyBeam.transform.eulerAngles = new Vector3(energyBeamOldEulerAngleX, prefabEnergyBeam.transform.eulerAngles.y, prefabEnergyBeam.transform.eulerAngles.z);
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

    IEnumerator ShockWaveRoutine(int WaveCount)
    {
        for(int i=0;i< WaveCount;i++)
        {
            Instantiate(prefabShockWave, transform.position, transform.rotation);

            yield return new WaitForSeconds(0.7f);
        }
    }
}
