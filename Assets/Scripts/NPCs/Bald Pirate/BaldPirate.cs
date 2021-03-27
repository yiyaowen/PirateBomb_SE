using UnityEngine;

public class BaldPirate : Enemy
{
    [Header("Special Settings")]
    public float kickForce;

    [Header("Sound Effects")]
    public AudioClip kickOffBombSound;

    AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = kickOffBombSound;
    }

    protected override void Update()
    {
        audioSource.volume = GameManager.instance.realEffectsVolume;
        base.Update();
    }

    public void KickOffBomb()
    {
        if (targetPoint.CompareTag("Bomb"))
        {
            audioSource.Play();

            int dir = targetPoint.transform.position.x < transform.position.x ? -1 : 1;
            targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1) * kickForce, ForceMode2D.Impulse);
        }
    }
}
