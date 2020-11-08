using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction preAction;

        public void StartAction(IAction action)
        {
            if (action == preAction) return;

            if (preAction != null && action != preAction)
            {
                preAction.Cancel();
            }
            preAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
