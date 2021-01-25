using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Tower

    private Tower parent;

    private AnimateTower parentAnim;

    //Enemy

    private ERange eparent;

    private Monster eparentAnim;

    private Vector3 targetPos;

    private Vector3 startPos;


    //all of these are for muzzleflash

    private Sprite defaultSprite;

    
    public Sprite muzzleFlash;

    public int framesToFlash = 3;

    public float destroyTime = 3;

    private SpriteRenderer spriteRend;


    // Start is called before the first frame update
    void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRend.sprite;

    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    public void Initialize(Tower parent)
    {
        StartCoroutine(FlashMuzzleFlash());

        this.parent = parent;
        this.parentAnim = parent.aTower;
        this.targetPos = parent.TargetPos;
        this.startPos = transform.position;
        
        spriteRend.sortingOrder = parentAnim.GetComponent<SpriteRenderer>().sortingOrder + 1;
      
        Vector2 dir = targetPos - startPos;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    public void Initialize(ERange parent)
    {
        StartCoroutine(FlashMuzzleFlash());

        this.eparent = parent;
        this.eparentAnim = parent.animMonster;
        this.targetPos = parent.TargetPos;
        this.startPos = transform.position;

        Vector2 dir = targetPos - startPos;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void MoveToTarget()
    {
        if (targetPos != null)
        {
            if (parent == null)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * eparent.ProjectileSpeed);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * parent.ProjectileSpeed);
            }
        }
    }

    IEnumerator FlashMuzzleFlash()
    {
        spriteRend.sprite = muzzleFlash;

        for (int i = 0; i < framesToFlash; i++)
        {
            yield return 0;
        }

        spriteRend.sprite = defaultSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Monster")
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }
}
