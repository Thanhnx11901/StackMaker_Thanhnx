using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform StartPoint;
    public GameObject openChest;
    public GameObject closeChest;
    public ParticleSystem particleSystem1;
    public ParticleSystem particleSystem2;

    private void Start()
    {
        openChest.SetActive(false);
        closeChest.SetActive(true);
    }

    public void AcitveChest()
    {
        openChest.SetActive(true);
        closeChest.SetActive(false);
    }
    public void PlayParticleSystem()
    {
        particleSystem1.Play();
        particleSystem2.Play(); 
    }

}
