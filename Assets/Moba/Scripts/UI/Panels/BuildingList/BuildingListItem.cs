using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingListItem : MonoBehaviour
{

    int mId;

    public void SetData(Transform item, int id, SpawnPoint sp){
        mId = id;
        Image img_icon = item.Find("Button/img_item").GetComponent<Image>();
        Text txt_cost = item.Find("Button/txt_cost").GetComponent<Text>();
        Text txt_name = item.Find("Button/txt_name").GetComponent<Text>();
        txt_name.text = sp.buildingName;
        txt_cost.text = sp.GetCurrentPrice().ToString();
        Text txt_warning = item.Find("Button/txt_warning").GetComponent<Text>();
        EventTrigger trigger = item.Find("Button").GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => {
            PlayerController_III.instance.SelectPreBuilding(mId);
        });
        trigger.triggers.Add(entry);
        Sprite sprite = ResourcesManager.Instance.GetBuildingFullIconById(id + 1);
        if(sprite!=null){
            img_icon.sprite = ResourcesManager.Instance.GetBuildingFullIconById(id + 1);
        }
    }

}
