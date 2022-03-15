using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip bGM;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bGM;

        if (audioSource != null)
        {
            audioSource?.Play();
        }
    }

    private void OnEnable()
    {
        PlayerShooting.OnFire += PlayerWeapon_OnFire;
    }

    private void PlayerWeapon_OnFire(AudioClip sfx)
    {
        audioSource.PlayOneShot(sfx);
    }




}
