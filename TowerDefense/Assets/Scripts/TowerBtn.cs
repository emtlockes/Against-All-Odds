using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private Sprite sprite; 

    public GameObject TowerPrefab { get => towerPrefab;}

    public Sprite Sprite { get => sprite; }
    public int Price { get => price; }

    [SerializeField]
    private int price;

    [SerializeField]
    private TextMeshProUGUI manaText;

    private void Start()
    {
        manaText.text = price + "<color=#00FF00>$</color>";
    }
}
