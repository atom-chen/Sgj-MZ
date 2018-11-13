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
            yield return this.StartCoroutine(base.OnLoad(request));
            Service.SUser sUser = Util.Global.SUser;
            string url = Service.HttpClient.assetBandleURL + "maps/maps_001.unity3d";
            yield return this.StartCoroutine(sUser.Download(url, 0, InitMap));
            this.dispatcher.Notify();
        }
        private void LSharpInit() { 
        }
        public void InitMap(AssetBundle assetbundle)
        {
            Object[] ts = assetbundle.LoadAllAssets<Object>();
            Debug.LogError("ts.Length=" + ts.Length);
            GameObject obj = ts[0] as GameObject;
            this.dispatcher.Set("tileMap", obj);
            Model.Scriptable.MapAsset mapAsset = ts[1] as Model.Scriptable.MapAsset;
            Debug.LogError("mapAsset.map.width" + mapAsset.map.width);
            Debug.LogError("mapAsset.map.height" + mapAsset.map.height);
            Debug.LogError("mapAsset.map.tiles.Count=" + mapAsset.map.tiles.Count);
            this.dispatcher.Set("map", mapAsset.map.tiles);
            assetbundle.Unload(false);
        }
        public void InitManager(){
            mapSearch = new TileMap();
            aStar = new AStar();
            breadthFirst = new BreadthFirst();

        }
        public void OnClickTile(View.Map.VTile vTile)
        {
            Vector2Int coordinate = vTile.coordinate;
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
