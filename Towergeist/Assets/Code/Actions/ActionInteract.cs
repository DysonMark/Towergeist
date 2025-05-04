using System;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using JW.Grid.GOAP.Actions;
using JW.Grid.GOAP.Goals;
using JW.Grid.Sensors;
using Actions.Chat;
using Actions.CompletionAnnouncement;

[RequireComponent(typeof(BaseSensor))]
[RequireComponent(typeof(GeneralAgentStats))]
public class ActionInteract : ActionBase, ICompletableAction
{
    #region Variables
    public float chatRestAmount = 0.2f; // Boredom decrease rate
    public int chatPriorityBoost = 20;
    public int chatPriorityPenalty = 15;
    private BaseSensor _sensor;
    private GeneralAgentStats _stats;
    private SpritePopup _popup;
    private GeneralAgentStats _target;
    #endregion

    #region Public Methods
    public bool IsDone { get; private set; }
    public event Action OnCompleted;

    public override List<Type> GetSupportedGoals() => new() { typeof(GoalInteract) };
    public override float GetCost() => 1f;

    public override void OnActivated()
    {
        _stats ??= GetComponent<GeneralAgentStats>();
        _sensor ??= GetComponent<BaseSensor>();
        _popup ??= GetComponent<SpritePopup>();

        var other = _sensor.TriggeredObject;
        if (other == null) { Complete(); return; }

        var otherStats = other.GetComponent<GeneralAgentStats>();
        if (otherStats == null) { Complete(); return; }

        if (_stats.IsFriendly && otherStats.IsFriendly)
        {
            _stats.DecreaseBoredom(Time.deltaTime * chatRestAmount);
            _stats.BoredomLevel += chatPriorityBoost;
            _popup.ShowChat();
            other.GetComponent<SpritePopup>()?.ShowChat();
        }
        else if (!_stats.IsFriendly && otherStats.IsFriendly)
        {
            _target = otherStats;
            _stats.BoredomLevel = Mathf.Max(0, _stats.BoredomLevel - chatPriorityPenalty);
            _popup.ShowYell();
            other.GetComponent<SpritePopup>()?.ShowYell();
            otherStats.IsBeingYelledAt = true;
        }
        if (_stats.IsFriendly)
        {
            Invoke(nameof(Complete), 3f);
        }
        else
        {
            Complete();
        }

    }

    public override void OnTick(float dt)
    {
        if (_target != null && !_target.gameObject.activeInHierarchy)
        {
            // Debug.LogWarning($"{name}: Target vanished mid-yell. Cleaning up.");
            Complete();
        }
    }

    public override void OnDeactivated()
    {
        CancelInvoke(nameof(Complete));
        _stats.IsBeingYelledAt = false;
        if (_target != null)
            _target.IsBeingYelledAt = false;
        _stats.BoredomLevel = 0;
    }
    #endregion

    #region Private Methods
    private void Complete()
    {
        // Debug.Log($"{name}: Interaction complete.");
        IsDone = true;
        OnCompleted?.Invoke();
    }

    #endregion
}
