using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum InteractionMode {
    [Description("Build")]
    Build,
    [Description("View")]
    View
}

public enum BuildType {
    [Description("Malware")]
    Malware,
    [Description("Keylogger")]
    Keylogger,
    [Description("Virus")]
    Virus,
    [Description("Firewall")]
    Firewall,
    [Description("Botfarm")]
    Botfarm
}

public static class Globals {
    public static HackingResources hackingResources = new HackingResources();
    public static InteractionMode mode = InteractionMode.View;
    public static BuildType buildType = BuildType.Botfarm;
}
