using System;
using UnityEngine;

public class Cucumber : Enemy
{
    [Header("Sound Effects")]
    public AudioClip blowOutBombSound;

    AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = blowOutBombSound;
    }

    protected override void Update()
    {
        audioSource.volume = GameManager.instance.realEffectsVolume;
        base.Update();
    }

    public void BlowOutBomb() // Animation Event
    {
        try
        {
            audioSource.Play();

            targetPoint.GetComponent<Bomb>().TurnOff();
        }
        catch (NullReferenceException)
        {
            // 不进行任何行动
        }
    }
}
