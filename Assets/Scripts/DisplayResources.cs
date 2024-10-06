using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DisplayResources : MonoBehaviour
{
    private TMP_Text _text;
    private int _index = 0;
    private string _statLine = ""; 

    private List<Tuple<string, float>> stats;
    void Start()
    {
        _text = GetComponentInParent<TMP_Text>();
        stats = Globals.hackingResources.getStats();
        StartCoroutine(DisplayStats());
    }

    private void Update() {
        // probably should only update this stuff when things get triggered instead of every frame
        StringBuilder builder = new StringBuilder();
        builder.Append(_statLine).Append("\n").Append("MODE: ").Append(Globals.mode);
        if (Globals.mode == InteractionMode.Build) {
            builder.Append("\n").Append("TOWER TYPE: ").Append(Globals.buildType);
        }
        _text.SetText(builder.ToString());
    }

    IEnumerator DisplayStats() {
        while (true) {
            var item =  stats[_index++];

            StringBuilder builder = new StringBuilder();
            builder.Append(item.Item1).Append(": ").Append(item.Item2);
            _statLine = builder.ToString();
            
            if (_index == stats.Count) {
                _index = 0;
            }

            yield return new WaitForSeconds(2);
        }
    }
}
