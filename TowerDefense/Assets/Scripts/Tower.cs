using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public AnimateTower aTower;

    [SerializeField]
    private string projectileType;

    [SerializeField]
    private float projectileSpeed;

    private Monster target;

    private Vector3 targetPos;

    private SpriteRenderer mySpriteRenderer;

    private List<Monster> monsters = new List<Monster>();

    private bool canAttack = true;

    private float attackTimer;

    [SerializeField]
    private float attackCooldown;

    public Monster Target { get => target; set => target = value; }

    public Vector3 TargetPos { get => targetPos; set => targetPos = value; }

    public float ProjectileSpeed { get => projectileSpeed; }

    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        aTower = gameObject.GetComponentInParent<AnimateTower>();
    }

    // Update is called once per frame
    void Update()
    {
        if(monsters.Count > 1)
        {
            //updates target if there is more than one monster in range
            SelectTarget(monsters);
            
        }

        Attack();
    }

    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }

    public void SelectTarget(List<Monster> monsters)
    {
        List<float> monsterProgress = new List<float>();

        int mostProgress = 0;

        if (monsters.Count > 1)
        {
            foreach (Monster m in monsters)
            {
                monsterProgress.Add(m.Progress);
            }

            for (int i = 0; i < monsterProgress.Count - 1; i++)
            {
                if (monsterProgress[mostProgress] <= monsterProgress[mostProgress + 1])
                {
                    mostProgress++;
                }
            }

            Target = monsters[mostProgress];
        } else if (monsters.Count == 1)
        {
            Target = monsters[0];
        }
        else
        {
            Target = null;
        }
    }

    private void Attack()
    {

        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        if (Target != null && Target.IsActive)
        {
            if (canAttack)
            {
                TargetPos = Target.transform.position;
                Shoot();

                canAttack = false;
            }
        }
    }

    private void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();

     
        projectile.transform.position = transform.position + new Vector3(0, 0, 0);
        
        
        projectile.Initialize(this);

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            monsters.Add(other.GetComponent<Monster>());

            SelectTarget(monsters);
         
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag== "Monster")
        {
            monsters.Remove(other.GetComponent<Monster>());
            SelectTarget(monsters);
            
        }
    }
}
