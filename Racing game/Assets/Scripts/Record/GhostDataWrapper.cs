using System.Collections.Generic;
using UnityEngine;

public class GhostDataWrapper
{
    public string profile { get; }
    public string level { get; }

    public string carName {  get; }
    public List<GhostData> ghostData { get; }

    public GhostDataWrapper(string profile, string level, string carName, List<GhostData> ghostData)
    {
        this.profile = profile;
        this.level = level;
        this.carName = carName;
        this.ghostData = ghostData;
    }
}
