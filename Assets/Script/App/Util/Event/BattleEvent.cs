
using System.Collections.Generic;
using App.Model;
using App.View.Map;

namespace App.Util.Event
{
    public class BattleEvent
    {
        public delegate void EventHandler(List<VTile> tiles, Belong belong);
        public event EventHandler HandlerMovingTiles;
        public void DispatchEventMovingTiles(List<VTile> tiles, Belong belong)
        {
            if (HandlerMovingTiles != null)
            {
                HandlerMovingTiles(tiles, belong);
            }
        }

        public event EventHandler HandlerAttackTiles;
        public void DispatchEventAttackTiles(List<VTile> tiles, Belong belong)
        {
            if (HandlerAttackTiles != null)
            {
                HandlerAttackTiles(tiles, belong);
            }
        }
    }
}
