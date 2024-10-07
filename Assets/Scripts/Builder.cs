using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Builder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var trigger = GetComponent<EventTrigger>();

        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnClickDelegate((PointerEventData)data); });

        trigger.triggers.Add(entry);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnClickDelegate(PointerEventData data)
    {
        Debug.Log("Builder Clicked!");
    }
}
