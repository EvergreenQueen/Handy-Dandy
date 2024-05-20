using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIdentification : MonoBehaviour
{
    public enum ListOfPossibleTags{Hot, Cold, Animal, Fruit, Child, Grippable, Big}
    [SerializeField] string name;
    [SerializeField] public int id;
    [SerializeField] ListOfPossibleTags[] tags;
    [SerializeField] string description;

    public bool containsTag(ListOfPossibleTags t)
    {
        foreach(ListOfPossibleTags tag in tags)
        {
            if(tag == t) return true;
        }

        return false;
    }
}
