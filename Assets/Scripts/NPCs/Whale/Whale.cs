using UnityEngine;

public class Whale : Enemy
{
    [Header("Special Settings")]
    public GameObject bombPrefab;
    public float swellScale;
    public int maxSwallowedBombCount;
    public float releaseBombForce;

    [Header("Sound Effects")]
    public AudioClip swallowBombSound;

    AudioSource audioSource;

    int swallowedBombCount = 0;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = swallowBombSound;
    }

    bool isBombReleased;
    protected override void Update()
    {
        audioSource.volume = GameManager.instance.realEffectsVolume;
        if (isDead)
        {
            ReleaseSwallowedBomb();
        }
        base.Update();
    }

    public void SwalowBomb() // Animation Event
    {
        audioSource.Play();
        targetPoint.gameObject.SetActive(false);
        if (++swallowedBombCount > maxSwallowedBombCount)
        {
            isDead = true;
            ReleaseSwallowedBomb();
        }

        transform.localScale *= swellScale;
        GetComponent<Rigidbody2D>().mass += bombPrefab.GetComponent<Rigidbody2D>().mass;
    }

    public void ReleaseSwallowedBomb()
    {
        if (isBombReleased) return;
        for (int i = 1; i <= swallowedBombCount; ++i)
        {
            float theta = Mathf.PI * i / (swallowedBombCount + 1);
            var dir = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
            var bomb = Instantiate(bombPrefab, transform.position + new Vector3(dir.x, dir.y, transform.position.z), transform.rotation);
            bomb.GetComponent<Rigidbody2D>().AddForce(dir * releaseBombForce, ForceMode2D.Impulse);
        }
        isBombReleased = true;
    }
}
