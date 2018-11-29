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
    public enum Direction
    {
        left,
        right,
        leftUp,
        leftDown,
        rightUp,
        rightDown
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
    public enum ActionType
    {
        idle,
        move,
        attack,
        block,
        hert,
    }

    [System.Serializable]
    public enum SkillType
    {
        attack,//物理攻击类
        magic,//法术攻击类
        heal,//回复类
        ability,//能力增强类
        help,//战场辅助类
    }
    [System.Serializable]
    public enum RadiusType
    {
        point,//点攻击
        range,//面攻击
        direction,//穿透攻击
        sector,//扇形穿透
    }
    [System.Serializable]
    public enum FiveElements
    {
        none,
        metal,
        wood,
        water,
        fire,
        earth
    }
    public enum SkillEffectBegin
    {
        attack_start,
        attack_end,
        enemy_hert,
        action_end
    }
    [System.Serializable]
    public enum SkillEffectSpecial
    {
        none,
        /// <summary>
        /// 引导攻击
        /// </summary>
        continue_attack,
        /// <summary>
        /// 反击后反击
        /// </summary>
        attack_back_attack,
        /// <summary>
        /// 先手攻击
        /// </summary>
        force_first,
        /// <summary>
        /// 吸血
        /// </summary>
        vampire,
        /// <summary>
        /// 能力,状态变化
        /// </summary>
        status,
        /// <summary>
        /// 攻击范围扩大
        /// </summary>
        attack_distance,
        /// <summary>
        /// 反击不受任何限制
        /// </summary>
        force_back_attack,
        /// <summary>
        /// 无反攻击
        /// </summary>
        no_back_attack,
        /// <summary>
        /// 必中
        /// </summary>
        force_hit,
        /// <summary>
        /// 溅射
        /// </summary>
        quantity_plus,
        /// <summary>
        /// 对骑兵攻击加成
        /// </summary>
        horse_hert,
        /// <summary>
        /// 攻击后移动
        /// </summary>
        move_after_attack,
        /// <summary>
        /// 移动攻击
        /// </summary>
        move_and_attack,
        /// <summary>
        /// 每回合固定伤害
        /// </summary>
        bout_fixed_damage,
        /// <summary>
        /// 攻击次数
        /// </summary>
        attack_count,
        /// <summary>
        /// 反击次数
        /// </summary>
        counter_attack_count,
        /// <summary>
        /// 对攻击范围内所有人进行攻击
        /// </summary>
        attack_all_near,
        /// <summary>
        /// 回马枪
        /// </summary>
        back_thrust,
        /// <summary>
        /// 固定伤害攻击
        /// </summary>
        fixed_damage,
        /// <summary>
        /// 地形辅助
        /// </summary>
        tile,
    }

    [System.Serializable]
    public enum AidType
    {
        none,
        physicalAttack,
        magicAttack,
        physicalDefense,
        magicDefense,
        /// <summary>
        /// 混乱
        /// </summary>
        chaos,
        /// <summary>
        /// 睡眠
        /// </summary>
        sleep,
        /// <summary>
        /// 定身/麻痹
        /// </summary>
        hemp,
        /// <summary>
        /// 毒
        /// </summary>
        poison,
    }
    public enum StrategyEffectType
    {
        aid,
        status,
        animation,
        vampire
    }

}
