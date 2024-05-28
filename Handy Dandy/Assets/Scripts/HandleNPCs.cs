using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleNPCs : MonoBehaviour
{
    [SerializeField] GameObject pieGuy;
    [SerializeField] GameObject oven;
    // Start is called before the first frame update
    void Start()
    {
        HandleQuests.PieGuy = pieGuy;
        HandleQuests.Oven = oven;
        pieGuy.SetActive(false);
        oven.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
