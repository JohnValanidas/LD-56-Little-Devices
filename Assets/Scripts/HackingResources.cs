
using System;
using System.Collections.Generic;

public class HackingResources {
    public float ram = 20;
    public float cpu = 3000;
    public float temp = 3000;
    public float bandwidth = .54f;
    public float virusStrength = .0f;
    public float data = .0f;

    public List<Tuple<string, float>> getStats() {
        List<Tuple<string, float>> list = new List<Tuple<string, float>>();
        list.Add(new Tuple<string, float>("Ram", ram));
        list.Add(new Tuple<string, float>("cpu", cpu));
        list.Add(new Tuple<string, float>("temp", temp));
        list.Add(new Tuple<string, float>("bandwidth", bandwidth));
        list.Add(new Tuple<string, float>("virusStrength", virusStrength));
        list.Add(new Tuple<string, float>("data", data));
        return list;
    }
}
