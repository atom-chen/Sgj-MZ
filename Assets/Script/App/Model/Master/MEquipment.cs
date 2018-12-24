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
        public MEquipmentValue physicalAttack = new MEquipmentValue();//物理攻击
        [JsonName(Name = "magic_attack")]
        public MEquipmentValue magicAttack = new MEquipmentValue();//魔法攻击
        public MEquipmentValue power = new MEquipmentValue();//力量
        public MEquipmentValue hp = new MEquipmentValue();//血量
        public MEquipmentValue mp = new MEquipmentValue();//MP
        public MEquipmentValue speed = new MEquipmentValue();//速度
        [JsonName(Name = "physical_defense")]
        public MEquipmentValue physicalDefense = new MEquipmentValue();//物理防御
        [JsonName(Name = "magic_defense")]
        public MEquipmentValue magicDefense = new MEquipmentValue();//魔法防御
        public MEquipmentValue knowledge = new MEquipmentValue();//技巧
        public MEquipmentValue trick = new MEquipmentValue();//谋略
        public MEquipmentValue endurance = new MEquipmentValue();//耐力
        [JsonName(Name = "moving_power")]
        public MEquipmentValue movingPower = new MEquipmentValue();//轻功／移动力
        public MEquipmentValue riding = new MEquipmentValue();//骑术
        public MEquipmentValue walker = new MEquipmentValue();//步战
        public MEquipmentValue pike = new MEquipmentValue();//长枪
        public MEquipmentValue sword = new MEquipmentValue();//短剑
        [JsonName(Name = "long_knife")]
        public MEquipmentValue longKnife = new MEquipmentValue();//大刀
        public MEquipmentValue knife = new MEquipmentValue();//短刀
        [JsonName(Name = "long_ax")]
        public MEquipmentValue longAx = new MEquipmentValue();//长斧
        public MEquipmentValue ax = new MEquipmentValue();//短斧
        [JsonName(Name = "long_sticks")]
        public MEquipmentValue longSticks = new MEquipmentValue();//棍棒
        public MEquipmentValue sticks = new MEquipmentValue();//棍棒
        public MEquipmentValue archery = new MEquipmentValue();//箭术
        [JsonName(Name = "hidden_weapons")]
        public MEquipmentValue hiddenWeapons = new MEquipmentValue();//暗器
        [JsonName(Name = "dual_wield")]
        public MEquipmentValue dualWield = new MEquipmentValue();//双手
        public MEquipmentValue magic = new MEquipmentValue();//法宝
        [JsonName(Name = "resistance_metal")]
        public MEquipmentValue resistanceMetal = new MEquipmentValue();//抗金
        [JsonName(Name = "resistance_wood")]
        public MEquipmentValue resistanceWood = new MEquipmentValue();//抗木
        [JsonName(Name = "resistance_water")]
        public MEquipmentValue resistanceWater = new MEquipmentValue();//抗水
        [JsonName(Name = "resistance_fire")]
        public MEquipmentValue resistanceFire = new MEquipmentValue();//抗火
        [JsonName(Name = "resistance_earth")]
        public MEquipmentValue resistanceEarth = new MEquipmentValue();//抗土

        [JsonName(Name = "image_index")]
        public int imageIndex;//马匹或鞋
        public int saddle;//马铠
    }
}