using UnityEngine.UI;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] WaveSpawner spawner;
    [SerializeField] Animator waveAnimator;
    [SerializeField] Text waveCountdownText;
    [SerializeField] Text waveCountText;
    private WaveSpawner.SpawnState previousState;
    
    void Start()
    {
        if(spawner == null)
        {
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            this.enabled = false;
        }
        if (waveCountdownText == null)
        {
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            this.enabled = false;
        }
    }

    private void Update()
    {
        switch (spawner.State)
        {
            case WaveSpawner.SpawnState.COUNTING: UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING: UpdateSpawningUI();
                break;
        }
        previousState = spawner.State;
    }

    public void UpdateCountingUI()
    {
        if(previousState != WaveSpawner.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCoundown", true);
        }
        waveCountdownText.text = ((int)spawner.WaveCountdown + 1).ToString();
    }

    public void UpdateSpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
           
            waveAnimator.SetBool("WaveCoundown", false);
            waveAnimator.SetBool("WaveIncoming", true);

            waveCountText.text = spawner.NextWave.ToString();
        }
    }
}
