using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    [SerializeField] GameObject[] _spikes,_spikesFloor;
    [SerializeField] GameObject _gate;


    int i = 0;

    public void GreenSpike()
    {
        if (i == 0)
        {
            _spikes[i].GetComponent<BoxCollider>().enabled= false;
            _spikesFloor[i].GetComponent<Animator>().SetBool("Start", true);
            i++;
        }
        else
        {
            _spikes[0].GetComponent<Animator>().SetBool("Start", true);
        }
    }

    public void BlueSpike()
    {
        if (i == 1)
        {
            _spikes[i].GetComponent<BoxCollider>().enabled = false;
            _spikesFloor[i].GetComponent<Animator>().SetBool("Start", true);
            i++;
        }
        else
        {
            _spikes[1].GetComponent<Animator>().SetBool("Start", true);
        }
    }

    public void RedSpike()
    {
        if (i == 2)
        {
            _spikes[i].GetComponent<BoxCollider>().enabled = false;
            _spikesFloor[i].GetComponent<Animator>().SetBool("Start", true);
            i++;
        }
        else
        {
            _spikes[2].GetComponent<Animator>().SetBool("Start", true);
        }
    }

    public void GraySpike()
    {
        if (i == 3)
        {
            _spikes[i].GetComponent<BoxCollider>().enabled = false;
            _spikesFloor[i].GetComponent<Animator>().SetBool("Start", true);
            _gate.GetComponent<Animator>().SetBool("Start", true);
            i++;
        }
        else
        {
            _spikes[3].GetComponent<Animator>().SetBool("Start", true);
        }
    }

}
