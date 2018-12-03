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
        public int physicalAttack;//物理攻击
        [JsonName(Name = "magic_attack")]
        public int magicAttack;//魔法攻击
        public int power;//力量
        public int hp;//血量
        public int mp;//MP
        public int speed;//速度
        [JsonName(Name = "physical_defense")]
        public int physicalDefense;//物理防御
        [JsonName(Name = "magic_defense")]
        public int magicDefense;//魔法防御
        public int knowledge;//技巧
        public int trick;//谋略
        public int endurance;//耐力
        [JsonName(Name = "moving_power")]
        public int movingPower;//轻功／移动力
        public int riding;//骑术
        public int walker;//步战
        public int pike;//长枪
        public int sword;//短剑
        [JsonName(Name = "long_knife")]
        public int longKnife;//大刀
        public int knife;//短刀
        [JsonName(Name = "long_ax")]
        public int longAx;//长斧
        public int ax;//短斧
        [JsonName(Name = "long_sticks")]
        public int longSticks;//棍棒
        public int sticks;//棍棒
        public int archery;//箭术
        [JsonName(Name = "hidden_weapons")]
        public int hiddenWeapons;//暗器
        [JsonName(Name = "dual_wield")]
        public int dualWield;//双手
        public int magic;//法宝
        [JsonName(Name = "resistance_metal")]
        public int resistanceMetal;//抗金
        [JsonName(Name = "resistance_wood")]
        public int resistanceWood;//抗木
        [JsonName(Name = "resistance_water")]
        public int resistanceWater;//抗水
        [JsonName(Name = "resistance_fire")]
        public int resistanceFire;//抗火
        [JsonName(Name = "resistance_earth")]
        public int resistanceEarth;//抗土

        [JsonName(Name = "image_index")]
        public int imageIndex;//马匹或鞋
        public int saddle;//马铠
    }
}