using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleNPCs : MonoBehaviour
{
    [SerializeField] GameObject pieGuy;
    [SerializeField] GameObject oven;
    [SerializeField] GameObject pie;
    // Start is called before the first frame update
    void Start()
    {
        HandleQuests.PieGuy = pieGuy;
        HandleQuests.Oven = oven;
        HandleQuests.Pie = pie;
        pieGuy.SetActive(false);
        oven.SetActive(false);
        pie.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
