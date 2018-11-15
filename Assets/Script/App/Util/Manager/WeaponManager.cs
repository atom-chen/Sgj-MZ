using System;
using App.Model;
using UnityEngine;

namespace App.Util.Manager
{
    public class WeaponManager
    {
        /// <summary>
        /// 枪剑类兵器
        /// </summary>
        public static bool IsPike(WeaponType weaponType)
        {
            return weaponType == WeaponType.pike || weaponType == WeaponType.sword;
        }
        /// <summary>
        /// 斧类兵器
        /// </summary>
        public static bool IsAx(WeaponType weaponType)
        {
            return weaponType == WeaponType.ax || weaponType == WeaponType.longAx;
        }
        /// <summary>
        /// 刀类兵器
        /// </summary>
        public static bool IsKnife(WeaponType weaponType)
        {
            return weaponType == WeaponType.longKnife || weaponType == WeaponType.shortKnife;
        }
        /// <summary>
        /// 长兵器
        /// </summary>
        public static bool IsLongWeapon(WeaponType weaponType)
        {
            return weaponType == WeaponType.longKnife || weaponType == WeaponType.longAx || weaponType == WeaponType.pike || weaponType == WeaponType.longSticks;
        }
        /// <summary>
        /// 短兵器
        /// </summary>
        public static bool IsShortWeapon(WeaponType weaponType)
        {
            return weaponType == WeaponType.shortKnife || weaponType == WeaponType.ax || weaponType == WeaponType.sword || weaponType == WeaponType.sticks;
        }
        /// <summary>
        /// 双兵器
        /// </summary>
        public static bool IsDualWeapon(WeaponType weaponType)
        {
            return weaponType == WeaponType.dualWield;
        }
        /// <summary>
        /// 远程类兵器
        /// </summary>
        public static bool IsArcheryWeapon(WeaponType weaponType)
        {
            return weaponType == WeaponType.archery;
        }


        public static string GetWeaponTypeAction(WeaponType weaponType, ActionType actionType)
        {
            if (IsArcheryWeapon(weaponType))
            {
                return "archery";
            }
            else if (IsLongWeapon(weaponType))
            {
                if (IsPike(weaponType) && actionType == ActionType.attack)
                {
                    return "pike";
                }
                return "long";
            }
            else if (IsDualWeapon(weaponType))
            {
                return "dual";
            }
            else
            {
                /*if (IsPike(weaponType) && actionType == ActionType.attack)
                {
                    return "pike";
                }*/
                return "short";
            }
        }
    }
}
