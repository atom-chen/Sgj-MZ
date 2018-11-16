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
            InitCharacters();
            this.dispatcher.Notify();
        }
        private void LSharpInit()
        {
        }
        public void InitCharacters()
        {

            List<Model.Character.MCharacter> characters = new List<Model.Character.MCharacter>();
            Model.Character.MCharacter mCharacter = new Model.Character.MCharacter();
            mCharacter.id = 0;
            mCharacter.characterId = 1;
            mCharacter.weapon = 1;
            mCharacter.clothes = 1;
            mCharacter.horse = 1;
            mCharacter.head = 1;
            characters.Add(mCharacter);
            mCharacter = new Model.Character.MCharacter();
            mCharacter.id = 0;
            mCharacter.characterId = 1;
            mCharacter.weapon = 2;
            mCharacter.clothes = 2;
            mCharacter.horse = 2;
            mCharacter.head = 2;
            mCharacter.coordinate.x = 0;
            mCharacter.coordinate.y = 1;
            characters.Add(mCharacter);
            this.dispatcher.Set("characters", characters);
        }
        public void InitMap(AssetBundle assetbundle)
        {
            Object[] ts = assetbundle.LoadAllAssets<Object>();
            GameObject obj = ts[0] as GameObject;
            this.dispatcher.Set("tileMap", obj);
            Model.Scriptable.MapAsset mapAsset = ts[1] as Model.Scriptable.MapAsset;
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
            Debug.LogError("OnClickTile:" + vTile.mTile.id);
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
