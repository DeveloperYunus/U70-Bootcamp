using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] SpikeController spikeController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Green"))
            {
                spikeController.GreenSpike();
            }

            if (gameObject.CompareTag("Blue"))
            {
                spikeController.BlueSpike();
            }

            if (gameObject.CompareTag("Red"))
            {
                spikeController.RedSpike();
            }

            if (gameObject.CompareTag("Gray"))
            {
                spikeController.GraySpike();
            }
        }
    }
}
