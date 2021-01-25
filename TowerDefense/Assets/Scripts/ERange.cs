using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERange : MonoBehaviour
{
    public Monster animMonster;

    [SerializeField]
    private string projectileType;

    [SerializeField]
    private float projectileSpeed;

    private Tower target;

    private Vector3 targetPos;

    private bool attackPlayer;

    private PlayerMovement player;

    private SpriteRenderer mySpriteRenderer;

    private List<Tower> towers = new List<Tower>();

    private bool canAttack = true;

    private float attackTimer;

    [SerializeField]
    private float attackCooldown;

    public Tower Target { get => target; set => target = value; }

    public Vector3 TargetPos { get => targetPos; set => targetPos = value; }

    public float ProjectileSpeed { get => projectileSpeed; }

    // Start is called before the first frame update
    void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        animMonster = gameObject.GetComponentInParent<Monster>();

        TargetPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        if (towers.Count > 1)
        {
            //updates target if there is more than one monster in range
            SelectTarget(towers);

        }

        Attack();
    }

    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }

    public void SelectTarget(List<Tower> towers)
    {
        if (towers.Count > 1)
        {
            int rInt = Random.Range(0, towers.Count - 1);

            Target = towers[rInt];
        }
        else if (towers.Count == 1)
        {
            Target = towers[0];
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
        if (canAttack)
        {
            if (attackPlayer)
            {
                TargetPos = player.transform.position;

                Shoot();

                canAttack = false;
            }
            else if (Target != null)
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
        if (other.tag == "Tower")
        {
            towers.Add(other.GetComponentInChildren<Tower>());

            SelectTarget(towers);

        }

        if (other.tag == "Player")
        {
            attackPlayer = true;

            player = other.GetComponent<PlayerMovement>();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Tower")
        {
            towers.Remove(other.GetComponentInChildren<Tower>());
            SelectTarget(towers);
            TargetPos = Vector3.zero;
        }

        if (other.tag == "Player")
        {
            attackPlayer = false;

            player = null;

            TargetPos = Vector3.zero;
        }
    }
}