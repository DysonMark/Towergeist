using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Stat;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class ChatRoomSensor : MonoBehaviour
{
    #region Variables
    public List<string> tagWhitelist = new() { "Agent" };
    public float boredomDrainRate = 20f;
    public float tiredDrainRate = 10f;
    private SphereCollider _trigger;
    public List<GeneralAgentStats> _inside = new();
    #endregion

    #region Unity Methods
    void Awake()
    {
        _trigger = GetComponent<SphereCollider>();
        _trigger.isTrigger = true;
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!tagWhitelist.Contains(other.tag)) return;
        if (other.TryGetComponent<GeneralAgentStats>(out var stats)
         && !_inside.Contains(stats))
            _inside.Add(stats);
    }

    void OnTriggerExit(Collider other)
    {
        if (!tagWhitelist.Contains(other.tag)) return;
        if (other.TryGetComponent<GeneralAgentStats>(out var stats)
         && _inside.Contains(stats))
            _inside.Remove(stats);
    }

    void Update()
    {
        if (_inside.Count == 0) return;
        float bDrain = boredomDrainRate * Time.deltaTime;
        float tDrain = tiredDrainRate * Time.deltaTime;

        foreach (var stats in _inside.ToList())
        {
            if (stats.IsFriendly)
                stats.BoredomLevel = Mathf.Max(0f, stats.BoredomLevel - bDrain);
            else
                stats.Tiredness = Mathf.Max(0f, stats.Tiredness - tDrain);
        }
    }
    #endregion

    #region Sensor Info
    public int FriendlyCount => _inside.Count(s => s.IsFriendly);
    public int UnfriendlyCount => _inside.Count(s => !s.IsFriendly);
    #endregion
}
