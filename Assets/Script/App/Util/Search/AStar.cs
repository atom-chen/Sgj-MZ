using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View.Map;
using App.Model.Character;

namespace App.Util.Search
{
    /// <summary>
    /// A星搜索
    /// </summary>
    public class AStar
    {
        //private App.Controller.Common.CBaseMap cBaseMap;
        //private MBaseMap mBaseMap;
        //private VBaseMap vBaseMap;
        //private App.Model.Master.MBaseMap baseMapMaster;
        private List<VTile> path = new List<VTile>();
        private List<VTile> open = new List<VTile>();
        private VTile endNode;
        public AStar()
        {
            //cBaseMap = controller;
            //mBaseMap = model;
            //vBaseMap = view;
        }
        private void Init(List<MCharacter> characters)
        {
            for (int i = 0; i < Global.tileUnits.Count; i++)
            {
                List<VTile> childs = Global.tileUnits[i];
                for (int j = 0; j < childs.Count; j++)
                {
                    childs[j].SearchInit();
                }
            }
            if(characters != null)
            {
                characters.ForEach(character => {
                    Global.tileUnits[character.coordinate.y][character.coordinate.x].isRoad = false;
                });
            }
            open.Clear();
        }
        private bool IsWay(VTile checkPoint)
        {
            return checkPoint.isRoad;
        }
        private void SetPath(VTile startNode)
        {
            VTile pathNode = endNode;
            while (!pathNode.coordinate.Equals(startNode.coordinate))
            {
                path.Insert(0, pathNode);
                pathNode = pathNode.parentNode;
            }
        }
        /// <summary>
        /// 计算每个节点
        /// </summary>
        /// <param name="neighboringNode">Neighboring node.</param>
        /// <param name="centerNode">Center node.</param>
        private void Calculation(VTile neighboringNode, VTile centerNode)
        {
            if (neighboringNode.isChecked)
            {
                return;
            }
            int g = centerNode.G + 1;
            if (neighboringNode.isOpen && neighboringNode.G < g)
            {
                return;
            }
            //如果该节点未在开放列表里，或者路径更优
            neighboringNode.G = g;
            CalculationGHF(neighboringNode);
            neighboringNode.parentNode = centerNode;
            SetToOpen(neighboringNode, !neighboringNode.isOpen);
        }
        private void CalculationGHF(VTile node)
        {
            node.H = Global.mapSearch.GetDistance(node, endNode);
            node.F = node.G + node.H;
        }
        /// <summary>
        /// 加入开放列表
        /// </summary>
        /// <param name="newNode">节点</param>
        /// <param name="newFlg">是否已经加进开放列表</param>
        private void SetToOpen(VTile newNode, bool newFlg)
        {
            int new_index;
            if (newFlg)
            {
                newNode.isOpen = true;
                int new_f = newNode.F;
                open.Add(newNode);
                new_index = open.Count - 1;
            }
            else
            {
                new_index = newNode.nodeIndex;
            }
            while (true)
            {
                //找到父节点
                int f_note_index = Mathf.FloorToInt(new_index * 0.5f);
                if (f_note_index <= 0)
                {
                    break;
                }
                //如果父节点的F值较大，则与父节点交换
                if (open[new_index].F >= open[f_note_index].F)
                {
                    break;
                }
                VTile obj_note = open[f_note_index];
                open[f_note_index] = open[new_index];
                open[new_index] = obj_note;
                open[f_note_index].nodeIndex = f_note_index;
                open[new_index].nodeIndex = new_index;
                new_index = f_note_index;
            }
        }
        /// <summary>
        /// 取开放列表里的最小值
        /// </summary>
        /// <returns>The open node.</returns>
        private VTile GetMinOpenNode()
        {
            VTile change_note;
            //将第一个节点，即F值最小的节点取出，最后返回
            VTile obj_note = open[1];
            open[1] = open[open.Count - 1];
            open[1].nodeIndex = 1;
            open.RemoveAt(open.Count - 1);
            int this_index = 1;
            while (true)
            {
                var left_index = this_index * 2;
                var right_index = this_index * 2 + 1;
                if (left_index >= open.Count) break;
                if (left_index == open.Count - 1)
                {
                    //当二叉树只存在左节点时，比较左节点和父节点的F值，若父节点较大，则交换
                    if (open[this_index].F <= open[left_index].F)
                    {
                        break;
                    }
                    change_note = open[left_index];
                    open[left_index] = open[this_index];
                    open[this_index] = change_note;
                    open[left_index].nodeIndex = left_index;
                    open[this_index].nodeIndex = this_index;
                    this_index = left_index;
                }
                else if (right_index < open.Count)
                {
                    //找到左节点和右节点中的较小者
                    if (open[left_index].F <= open[right_index].F)
                    {
                        //比较左节点和父节点的F值，若父节点较大，则交换
                        if (open[this_index].F <= open[left_index].F)
                        {
                            break;
                        }
                        change_note = open[left_index];
                        open[left_index] = open[this_index];
                        open[this_index] = change_note;
                        open[left_index].nodeIndex = left_index;
                        open[this_index].nodeIndex = this_index;
                        this_index = left_index;
                    }
                    else
                    {
                        //比较右节点和父节点的F值，若父节点较大，则交换
                        if (open[this_index].F <= open[right_index].F)
                        {
                            break;
                        }
                        change_note = open[right_index];
                        open[right_index] = open[this_index];
                        open[this_index] = change_note;
                        open[right_index].nodeIndex = right_index;
                        open[this_index].nodeIndex = this_index;
                        this_index = right_index;
                    }
                }

            }
            return obj_note;
        }
        public List<VTile> Search(MCharacter mCharacter, VTile startTile, VTile endTile, List<MCharacter> characters = null)
        {
            path.Clear();
            if (startTile.coordinate.Equals(endTile.coordinate))
            {
                return path;
            }
            this.endNode = endTile;
            Init(characters);
            open.Add(null);
            bool isOver = false;
            VTile thisPoint = startTile;
            while (!isOver)
            {
                thisPoint.isChecked = true;
                List<Vector2Int> checkList = Global.mapSearch.GetNeighboringCoordinates(thisPoint.coordinate);
                //检测开始
                foreach (Vector2Int tileVec in checkList)
                {
                    VTile tile = Global.mapSearch.GetTile(tileVec);
                    if (IsWay(tile))
                    {
                        //如果坐标可以通过，则首先检查是不是目标点
                        if (tile.coordinate.Equals(endTile.coordinate))
                        {
                            tile.parentNode = thisPoint;
                            isOver = true;
                            break;
                        }
                        Calculation(tile, thisPoint);
                    }
                }
                if (!isOver)
                {
                    //如果未到达指定地点则取出f值最小的点作为循环点
                    if (open.Count > 1)
                    {
                        thisPoint = GetMinOpenNode();
                    }
                    else
                    {
                        return path;
                    }
                }
            }
            SetPath(startTile);
            return path;
        }
    }
}