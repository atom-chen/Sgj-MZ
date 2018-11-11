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
    public enum MoveType
    {
        /// <summary>
        /// 步兵
        /// </summary>
        infantry,
        /// <summary>
        /// 骑兵
        /// </summary>
        cavalry,
    }
    [System.Serializable]
    public enum WeaponType
    {
        /// <summary>
        /// 短刀
        /// </summary>
        shortKnife,
        /// <summary>
        /// 大刀
        /// </summary>
        longKnife,
        /// <summary>
        /// 短斧
        /// </summary>
        ax,
        /// <summary>
        /// 长斧
        /// </summary>
        longAx,
        /// <summary>
        /// 长枪
        /// </summary>
        pike,
        /// <summary>
        /// 剑
        /// </summary>
        sword,
        /// <summary>
        /// 弓箭
        /// </summary>
        archery,
        /// <summary>
        /// 长棍棒
        /// </summary>
        longSticks,
        /// <summary>
        /// 短棍棒
        /// </summary>
        sticks,
        /// <summary>
        /// 双手
        /// </summary>
        dualWield,
        /// <summary>
        /// 法宝
        /// </summary>
        magic,
    }
    [System.Serializable]
    public enum EquipmentType
    {
        /// <summary>
        /// 武器
        /// </summary>
        weapon,
        /// <summary>
        /// 马
        /// </summary>
        horse,
        /// <summary>
        /// 衣服
        /// </summary>
        clothes
    }
    [System.Serializable]
    public enum ClothesType
    {
        /// <summary>
        /// 铠甲
        /// </summary>
        armor,
        /// <summary>
        /// 布衣
        /// </summary>
        commoner
    }
    public enum Mission
    {
        /// <summary>
        /// 主动出击
        /// </summary>
        initiative,
        /// <summary>
        /// 被动出击
        /// </summary>
        passive,
        /// <summary>
        /// 原地防守
        /// </summary>
        defensive
    }
}
