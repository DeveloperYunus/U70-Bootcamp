using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] GameObject[] _blades;
    [SerializeField] GameObject _gate;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                _gate.GetComponent<Animator>().SetBool("Start", true);
                gameObject.GetComponent<Animator>().SetBool("Start", true);
                for (int i = 0; i < _blades.Length; i++)
                {
                    _blades[i].GetComponent<Animator>().SetBool("Start", true);
                }
            }
        }
    }
}
