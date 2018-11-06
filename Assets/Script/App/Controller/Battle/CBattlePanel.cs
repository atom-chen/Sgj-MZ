using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Model;
using App.Util.Manager;
using App.Util.Search;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Battle
{
    public class CBattlePanel : CPanel
    {
        private int boutCount = 0;
        private int maxBout = 0;
        public Belong currentBelong { get; set; }
        public BattleMode battleMode { get; set; }
        public TileMap mapSearch { get; set; }
        public AStar aStar { get; set; }
        public BreadthFirst breadthFirst { get; set; }
        public BattleManager manager { get; set; }
        public BattleTilesManager tilesManager { get; set; }
        public BattleCharactersManager charactersManager { get; set; }
        public BattleCalculateManager calculateManager { get; set; }
        public override IEnumerator OnLoad( Request request ) {
            LSharpInit();
            InitMap();
            yield return this.StartCoroutine(base.OnLoad(request));
        }
        private void LSharpInit() { 
        }
        public void InitMap(){

        }
        public void InitManager(){
            mapSearch = new TileMap();
            aStar = new AStar();
            breadthFirst = new BreadthFirst();

        }
        public void OnClickTile(Vector2Int coordinate)
        {
            if (currentBelong != Belong.self)
            {
                return;
            }
            switch (battleMode)
            {
                case BattleMode.none:
                    manager.ClickNoneNode(coordinate);
                    break;
                case BattleMode.show_move_tiles:
                    manager.ClickMovingNode(coordinate);
                    break;
                case BattleMode.move_end:
                    manager.ClickSkillNode(coordinate);
                    break;
                case BattleMode.move_after_attack:
                    manager.ClickMovingNode(coordinate);
                    break;
            }
        }
    }
}
