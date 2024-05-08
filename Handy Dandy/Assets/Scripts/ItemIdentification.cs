using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIdentification : MonoBehaviour
{
    enum ListOfPossibleTags{Hot, Cold, Animal, Fruit, Child, Grippable, Big}
    [SerializeField] string name;
    [SerializeField] ListOfPossibleTags[] tags;
    [SerializeField] string description;
}
