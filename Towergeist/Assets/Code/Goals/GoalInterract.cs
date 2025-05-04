using UnityEngine;
using Stat;
using JW.Grid.GOAP.Goals;
using JW.Grid.Sensors;

[RequireComponent(typeof(BaseSensor))]
[RequireComponent(typeof(GeneralAgentStats))]
public class GoalInteract : GoalBase
{
    #region Variables
    [Tooltip("Priority when a valid interaction partner is in range.")]
    [SerializeField] private int interactPriority = 30;

    private BaseSensor _sensor;
    private GeneralAgentStats _stats;
    #endregion

    public override int CalculatePriority()
    {
        _stats ??= GetComponent<GeneralAgentStats>();
        _sensor ??= GetComponent<BaseSensor>();

        if (!_sensor.IsTriggered) return -1;

        var otherGO = _sensor.TriggeredObject;
        if (otherGO == null) return -1;

        var otherStats = otherGO.GetComponent<GeneralAgentStats>();
        if (otherStats == null) return -1;

        if (_stats.IsFriendly && otherStats.IsFriendly)
        {
            return interactPriority;
        }

        if (!_stats.IsFriendly && otherStats.IsFriendly)
        {
            return interactPriority + 10;
        }
        return -1;
    }

    public override bool CanRun()
    {
        return CalculatePriority() > 0;
    }
}