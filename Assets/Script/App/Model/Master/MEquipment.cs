using System.Collections;
using System.Collections.Generic;
using App.Model.Common;

namespace App.Model.Master
{
    [System.Serializable]
    public class MEquipment : MBase
    {
        public MEquipment()
        {
        }
        public string name;//
        public int qualification;//品质
        public WeaponType weapon_type;//武器类型
        /// <summary>
        /// 武器相性
        /// 大于0时需要跟武将相性相同才能装备
        /// </summary>
        public int compatibility;
        public MoveType move_type;//移动类型
        public ClothesType clothes_type;//衣服类型
        public int physical_attack;//物理攻击
        public int magic_attack;//魔法攻击
        public int power;//力量
        public int hp;//血量
        public int mp;//MP
        public int speed;//速度
        public int physical_defense;//物理防御
        public int magic_defense;//魔法防御
        public int knowledge;//技巧
        public int trick;//谋略
        public int endurance;//耐力
        public int moving_power;//轻功／移动力
        public int riding;//骑术
        public int walker;//步战
        public int pike;//长枪
        public int sword;//短剑
        public int long_knife;//大刀
        public int knife;//短刀
        public int long_ax;//长斧
        public int ax;//短斧
        public int long_sticks;//棍棒
        public int sticks;//棍棒
        public int archery;//箭术
        public int hidden_weapons;//暗器
        public int dual_wield;//双手
        public int magic;//法宝
        public int resistance_metal;//抗金
        public int resistance_wood;//抗木
        public int resistance_water;//抗水
        public int resistance_fire;//抗火
        public int resistance_earth;//抗土

        public int image_index;//马匹或鞋
        public int saddle;//马铠
    }
}