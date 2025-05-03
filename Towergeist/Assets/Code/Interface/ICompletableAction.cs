using System;

namespace Actions.CompletionAnnouncement
{
    public interface ICompletableAction
    {
        /// <summary> Tracks action cmpletion.</summary>
        bool IsDone { get; }

        /// <summary>Declares Completion.</summary>
        event Action OnCompleted;
    }
}