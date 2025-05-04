using System;
using System.Linq;
using UnityEngine;
using JW.Grid.GOAP.Actions;
using Stat;
using Movement;
using Actions.Chat;
using Actions.CompletionAnnouncement;
using System.Collections.Generic;
using Agents.Goals;

public class ActionYell : ActionBase, ICompletableAction
{
    #region Variables
    private GeneralAgentStats _stats;
    private ChatRoomSensor _chatSensor;
    private AreaMover _mover;
    private float _timer;
    private bool _hasArrived;
    private GeneralAgentStats _victim;
    #endregion

    #region Public Functions
    public bool IsDone { get; private set; }
    public event Action OnCompleted;

    public override List<Type> GetSupportedGoals() => new() { typeof(GoalRest) };
    public override float GetCost() => 1f;

    public override void OnActivated()
    {
        _stats ??= GetComponent<GeneralAgentStats>();
        _chatSensor ??= FindObjectOfType<ChatRoomSensor>();
        _mover ??= GetComponent<AreaMover>();

        if (_stats.IsFriendly || _mover == null)
        {
            Complete();
            return;
        }

        Transform chatSpot = _mover.chattingArea;
        _mover.OnArrived += OnArrived;

        if (Vector3.Distance(transform.position, chatSpot.position) < 0.5f)
            OnArrived();
        else
            _mover.MoveTo(AreaMover.Destination.ChattingArea);
    }

    public override void OnTick(float deltaTime)
    {
        if (!_hasArrived || _victim == null) return;

        _timer += deltaTime;
        if (_timer >= 2f)
            Complete();
    }

    public override void OnDeactivated()
    {
        _mover.OnArrived -= OnArrived;
        _stats.IsBeingYelledAt = false;
        if (_victim != null)
            _victim.IsBeingYelledAt = false;
    }
    #endregion

    #region Private Functions
    #region Yell Logic
    private void OnArrived()
    {
        _mover.OnArrived -= OnArrived;
        _hasArrived = true;
        _timer = 0f;

        if (_stats.BoredomLevel <= 0f && !_chatSensor._inside.Any(s => s.IsFriendly))
        {
            _stats.CanSleep = true;
            Complete();
            return;
        }

        _victim = _chatSensor._inside
            .FirstOrDefault(s => s.IsFriendly
                              && Vector3.Distance(transform.position, s.transform.position) <= 1f);

        if (_victim != null)
        {
            GetComponent<YellInteraction>()?.ShowYell();
            _victim.IsBeingYelledAt = true;
            _victim.ResetBoredom();
            _victim.GetComponent<YellInteraction>()?.ShowYell();
        }
        else
        {
            _stats.CanSleep = true;
            Complete();
        }
    }
    #endregion
    private void Complete()
    {
        _mover.OnArrived -= OnArrived;
        IsDone = true;
        OnCompleted?.Invoke();
    }
    #endregion
}