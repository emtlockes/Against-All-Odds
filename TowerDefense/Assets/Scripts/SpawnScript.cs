using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    [SerializeField]
    private int[] pathXCoordinates;
    [SerializeField]
    private int[] pathYCoordinates;

    public int[] PathXCoordinates { get => pathXCoordinates; set => pathXCoordinates = value; }
    public int[] PathYCoordinates { get => pathYCoordinates; set => pathYCoordinates = value; }


    // Start is called before the first frame update
    void Start()
    {
       
    }
}
