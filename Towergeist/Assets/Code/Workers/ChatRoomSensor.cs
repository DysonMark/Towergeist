using System.Collections.Generic;
using UnityEngine;
using Stat;

public class ChatRoomSensor : MonoBehaviour
{
    private readonly List<GeneralAgentStats> agentsInRoom = new();

    private void OnTriggerEnter(Collider other)
    {
        var agent = other.GetComponent<GeneralAgentStats>();
        if (agent != null && agent.IsFriendly)
        {
            if (!agentsInRoom.Contains(agent))
                agentsInRoom.Add(agent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var agent = other.GetComponent<GeneralAgentStats>();
        if (agent != null)
        {
            agentsInRoom.Remove(agent);
        }
    }

    public List<GeneralAgentStats> GetFriendlyAgentsInRoom()
    {
        agentsInRoom.RemoveAll(a => a == null); 
        return new List<GeneralAgentStats>(agentsInRoom);
    }
}