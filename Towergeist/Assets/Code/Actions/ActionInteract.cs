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
    public int chatRestAmount = 10;
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
            _stats.BoredomLevel = Mathf.Max(0f, _stats.BoredomLevel - chatRestAmount);
            _stats.ChatPriorityModifier += chatPriorityBoost;
            _popup.ShowChat();
            other.GetComponent<SpritePopup>()?.ShowChat();
        }
        else if (!_stats.IsFriendly && otherStats.IsFriendly)
        {
            _target = otherStats;
            _stats.ChatPriorityModifier = Mathf.Max(0, _stats.ChatPriorityModifier - chatPriorityPenalty);
            _popup.ShowYell();
            other.GetComponent<SpritePopup>()?.ShowYell();
            otherStats.IsBeingYelledAt = true;
        }

        Invoke(nameof(Complete), 3f);
    }

    public override void OnTick(float dt) { }

    public override void OnDeactivated()
    {
        CancelInvoke(nameof(Complete));
        _stats.IsBeingYelledAt = false;
        if (_target != null)
            _target.IsBeingYelledAt = false;
        _stats.ChatPriorityModifier = 0;
    }
    #endregion

    #region Private Methods
    private void Complete()
    {
        IsDone = true;
        OnCompleted?.Invoke();
    }
    #endregion
}
