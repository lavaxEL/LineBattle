using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitInfoControl : MonoBehaviour
{
    public GameObject infoBlock;
    public GameObject infoSellingButton;
    public Text name_;
    public Text stat;
    public Text desc;
    public Image image;

    public void ShowImage(bool sell, Creature creature, Sprite sprite)
    {
        name_.text = creature.Name;
        string stats = "";
        stats += Main.settings.language != Settings.Language.rus ? "damage: " : "урон: ";
        stats += creature.Damage.ToString();
        stats += Main.settings.language != Settings.Language.rus ? "\nhealth: " : "\nздоровье: ";
        stats += creature.Hp.ToString();
        stat.text = stats;
        desc.text = creature.Description;
        image.sprite = sprite;
        if (sell)
            infoSellingButton.SetActive(true);
        else
            infoSellingButton.SetActive(false);
        infoBlock.SetActive(true);
    }
    private void Update()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        if (Input.GetMouseButtonDown(0))
        {
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 0)
            {
                if (raycastResults[0].gameObject == infoBlock)
                {
                    infoBlock.SetActive(false);
                }
            }
        }
    }

}
