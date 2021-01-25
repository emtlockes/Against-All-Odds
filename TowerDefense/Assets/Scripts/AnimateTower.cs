using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTower : MonoBehaviour
{
    private Tower towerRange;

    private Animator myAnimator;

    public void Start()
    {
        towerRange = gameObject.GetComponentInChildren<Tower>();
        myAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (towerRange.Target == null)
        {
            myAnimator.SetBool("Attacking", false);
        }
        if (towerRange.Target != null) {
            myAnimator.SetBool("Attacking", true);
        }
    }
}
