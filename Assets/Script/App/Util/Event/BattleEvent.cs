
using System.Collections.Generic;
using App.Model;
using App.View.Map;

namespace App.Util.Event
{
    public class BattleEvent
    {
        public delegate void EventHandler(List<VTile> tiles, Belong belong);
        public event EventHandler HandlerTiles;
        public void DispatchEventTiles(List<VTile> tiles, Belong belong)
        {
            if (HandlerTiles != null)
            {
                HandlerTiles(tiles, belong);
            }
        }
    }
}
