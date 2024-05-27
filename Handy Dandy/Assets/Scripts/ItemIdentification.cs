using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIdentification : MonoBehaviour
{
    public enum ListOfPossibleTags{Hot, Cold, Animal, Fruit, Child, Grippable, Big}
    [SerializeField] public string name;
    [SerializeField] public int id;
    [SerializeField] public ListOfPossibleTags[] tags;
    [SerializeField] public string description;

    public bool containsTag(ListOfPossibleTags t)
    {
        foreach(ListOfPossibleTags tag in tags)
        {
            if(tag == t) return true;
        }

        return false;
    }
}
