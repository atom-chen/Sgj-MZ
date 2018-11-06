using System;
namespace App.Model
{
    public enum Belong
    {
        self,
        friend,
        enemy
    }
    public enum BattleMode
    {
        none,
        show_move_tiles,
        moving,
        move_end,
        actioning,
        move_after_attack

    }
}
