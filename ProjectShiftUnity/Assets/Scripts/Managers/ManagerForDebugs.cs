using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManagerForDebugs : MonoBehaviour {

    public static ManagerForDebugs instance = null;

    [Header("Debug Texts")]
    public Text gameStarted;
    public Text spawningInit;
    public Text pooledItems;
    public Text waveInit;
    public Text waveNumber;
    public Text waveType;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void GameStarted()
    {
        if (!gameStarted.enabled)
            gameStarted.enabled = true;
    }

    public void SpawningInit()
    {
        if (!spawningInit.enabled)
            spawningInit.enabled = true;
    }

    public void SpawnedObjects()
    {
        if (!pooledItems.enabled)
            pooledItems.enabled = true;
    }

    public void WaveInit()
    {
        if (!waveInit.enabled)
            waveInit.enabled = true;
    }

    public void WaveNumber(int _waveNum)
    {
        if (!waveNumber.enabled)
            waveNumber.enabled = true;

        waveNumber.text = _waveNum.ToString();
    }

    public void WaveType(string _waveType)
    {
        if (!waveType.enabled)
            waveType.enabled = true;

        waveType.text = _waveType;
    }
}
