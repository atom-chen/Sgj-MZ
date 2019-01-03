

namespace App.Util.Event
{
    public class SharpEvent
    {
        public delegate void AddCharacterEvent(int npcId, App.Model.ActionType actionType, App.Model.Direction direction, int x, int y);
        public event AddCharacterEvent AddCharacterHandler;
        public void DispatchAddCharacter(int npcId, App.Model.ActionType actionType, App.Model.Direction direction, int x, int y)
        {
            if (AddCharacterHandler != null)
            {
                AddCharacterHandler(npcId, actionType, direction, x, y);
            }
        }


    }
}
