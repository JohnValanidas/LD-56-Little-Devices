using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) {
            Globals.mode = InteractionMode.View;
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            Globals.mode = InteractionMode.Build;
        }
    }
}
