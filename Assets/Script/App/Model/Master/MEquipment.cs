using System.Collections;
using System.Collections.Generic;
using App.Model.Common;
using JsonFx;

namespace App.Model.Master
{
    [System.Serializable]
    public class MEquipment : MBase
    {
        public MEquipment()
        {
        }
        public int star;
        public string name;//
        public int qualification;//品质
        [JsonName(Name = "weapon_type")]
        public WeaponType weaponType;//武器类型
        /// <summary>
        /// 武器相性
        /// 大于0时需要跟武将相性相同才能装备
        /// </summary>
        public int compatibility;
        [JsonName(Name = "move_type")]
        public MoveType moveType;//移动类型
        [JsonName(Name = "clothes_type")]
        public ClothesType clothesType;//衣服类型
        [JsonName(Name = "physical_attack")]
        public MEquipmentValue physicalAttack;//物理攻击
        [JsonName(Name = "magic_attack")]
        public MEquipmentValue magicAttack;//魔法攻击
        public MEquipmentValue power;//力量
        public MEquipmentValue hp;//血量
        public MEquipmentValue mp;//MP
        public MEquipmentValue speed;//速度
        [JsonName(Name = "physical_defense")]
        public MEquipmentValue physicalDefense;//物理防御
        [JsonName(Name = "magic_defense")]
        public MEquipmentValue magicDefense;//魔法防御
        public MEquipmentValue knowledge;//技巧
        public MEquipmentValue trick;//谋略
        public MEquipmentValue endurance;//耐力
        [JsonName(Name = "moving_power")]
        public MEquipmentValue movingPower;//轻功／移动力
        public MEquipmentValue riding;//骑术
        public MEquipmentValue walker;//步战
        public MEquipmentValue pike;//长枪
        public MEquipmentValue sword;//短剑
        [JsonName(Name = "long_knife")]
        public MEquipmentValue longKnife;//大刀
        public MEquipmentValue knife;//短刀
        [JsonName(Name = "long_ax")]
        public MEquipmentValue longAx;//长斧
        public MEquipmentValue ax;//短斧
        [JsonName(Name = "long_sticks")]
        public MEquipmentValue longSticks;//棍棒
        public MEquipmentValue sticks;//棍棒
        public MEquipmentValue archery;//箭术
        [JsonName(Name = "hidden_weapons")]
        public MEquipmentValue hiddenWeapons;//暗器
        [JsonName(Name = "dual_wield")]
        public MEquipmentValue dualWield;//双手
        public MEquipmentValue magic;//法宝
        [JsonName(Name = "resistance_metal")]
        public MEquipmentValue resistanceMetal;//抗金
        [JsonName(Name = "resistance_wood")]
        public MEquipmentValue resistanceWood;//抗木
        [JsonName(Name = "resistance_water")]
        public MEquipmentValue resistanceWater;//抗水
        [JsonName(Name = "resistance_fire")]
        public MEquipmentValue resistanceFire;//抗火
        [JsonName(Name = "resistance_earth")]
        public MEquipmentValue resistanceEarth;//抗土

        [JsonName(Name = "image_index")]
        public int imageIndex;//马匹或鞋
        public int saddle;//马铠
    }
}