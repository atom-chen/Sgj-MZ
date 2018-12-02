
using App.Model.Common;

namespace App.Model.Master
{
    /**
     * 静态变量类
     */
    [System.Serializable]
    public class MConstant : MBase
    {
        public MConstant()
        {
        }
        /// <summary>
        /// AP恢复时间
        /// </summary>
        public int recover_ap_time;
        /// <summary>
        /// 世界地图用tile的ID
        /// </summary>
        public int world_map_id = 1;
        /// <summary>
        /// 默认鞋
        /// </summary>
        public int shoe_default_index = 4;
        /// <summary>
        /// 教学长度
        /// </summary>
        public int tutorial_end = 3;
        /// <summary>
        /// 教学召唤抽卡
        /// </summary>
        public int tutorial_gacha = 4;
        /// <summary>
        /// 教学WorldId
        /// </summary>
        public int tutorial_world_id = 2;
        /// <summary>
        /// 女英雄头像ID范围
        /// </summary>
        public int[] female_heads;
        /// <summary>
        /// 玩家创立英雄的ID范围
        /// </summary>
        public int[] user_characters;
        /// <summary>
        /// AI判断是否治疗的HP比例
        /// </summary>
        public float weak_hp = 0.2f;
        /// <summary>
        /// 野生特技适用地形
        /// </summary>
        public int[] tile_wild = new int[] { 3, 4, 6, 7 };
        /// <summary>
        /// 水性特技适用地形
        /// </summary>
        public int[] tile_swim = new int[] { 5 };
        /// <summary>
        /// 残血状态
        /// </summary>
        public float is_pant = 0.2f;
        /// <summary>
        /// 地图线
        /// </summary>
        public int tile_line = 0;
    }
}