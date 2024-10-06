using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DisplayResources : MonoBehaviour
{
    private TMP_Text _text;
    private int index = 0;

    private List<Tuple<string, float>> stats;
    void Start()
    {
        _text = GetComponentInParent<TMP_Text>();
        stats = Globals.hackingResources.getStats();
        StartCoroutine(DisplayStats());
    }

    IEnumerator DisplayStats() {
        while (true) {
            var item =  stats[index++];

            StringBuilder builder = new StringBuilder();
            builder.Append(item.Item1).Append(": ").Append(item.Item2);
            _text.SetText(builder.ToString());
            
            if (index == stats.Count) {
                index = 0;
            }

            yield return new WaitForSeconds(2);
        }
    }
}
