using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoverItemUI : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    PlayerControls controller;
    public GameObject hoverItemUI;
    public TMP_Text itemInfoText;
    GameObject currentItem = null;
    public List<ItemIdentification.ListOfPossibleTags> tags;

    [Header("Sprites")]
    [SerializeField] List<Sprite> tagIcons;

    [Header("Tag Renderers")]
    [SerializeField] List<Image> renderers;  

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.lookingAtObject) currentItem = controller.hit.collider.gameObject;
        else currentItem = null;

        drawUI();
    }

    public void drawUI() {
        if(currentItem != null)
        {
            // draw shit
            hoverItemUI.SetActive(true);
            ItemIdentification itemInfo = currentItem.GetComponent<ItemIdentification>();

            itemInfoText.text = itemInfo.name + "\n" + itemInfo.description;

            tags.Clear();
            foreach(var tag in itemInfo.tags)
            {
                tags.Add(tag);
            }
        }
        else
        {
            hoverItemUI.SetActive(false);
        }
        drawTags();
    }

    void drawTags() {
        int numTags = tags.Count;
        int remainingRenderers = renderers.Count - numTags;
        Debug.Log("remaining renderers = " + remainingRenderers);

        int i = 0;
        for(i = i; i < numTags; i++)
        {
            ItemIdentification.ListOfPossibleTags tag = tags[i];
            Image renderer = renderers[i];

            int tagId = (int)tag;
            renderer.sprite = tagIcons[tagId + 1]; // gotta add 1 bc the first (0 index) is empty tag
        }

        for(i = i; i < remainingRenderers; i++)
        {
            Image renderer = renderers[i];

            renderer.sprite = tagIcons[0]; // set to the blank tag icon
        }
    }
}
