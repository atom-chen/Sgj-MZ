

namespace App.Util.Event
{
    public class SharpEvent
    {
        public delegate void AddCharacterEvent(int npcId, App.Model.ActionType actionType, App.Model.Direction direction, int x, int y, bool isPlayer);
        public event AddCharacterEvent AddCharacterHandler;
        public void DispatchAddCharacter(int npcId, App.Model.ActionType actionType, App.Model.Direction direction, int x, int y, bool isPlayer)
        {
            if (AddCharacterHandler != null)
            {
                AddCharacterHandler(npcId, actionType, direction, x, y, isPlayer);
            }
        }

        public delegate void SetNpcActionEvent(int npcId, App.Model.ActionType actionType);
        public event SetNpcActionEvent SetNpcActionHandler;
        public void DispatchSetNpcAction(int npcId, App.Model.ActionType actionType)
        {
            if (SetNpcActionHandler != null)
            {
                SetNpcActionHandler(npcId, actionType);
            }
        }

        public delegate void MoveNpcEvent(int npcId, int x, int y);
        public event MoveNpcEvent MoveNpcHandler;
        public void DispatchMoveNpc(int npcId, int x, int y)
        {
            if (MoveNpcHandler != null)
            {
                MoveNpcHandler(npcId, x, y);
            }
        }

    }
}
