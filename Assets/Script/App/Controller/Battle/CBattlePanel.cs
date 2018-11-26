using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Model;
using App.Util;
using App.Util.Manager;
using App.Util.Search;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Battle
{
    public class CBattlePanel : CPanel
    {
        [SerializeField] private Camera camera3d;
        private int boutCount = 0;
        private int maxBout = 0;
        public List<Model.Character.MCharacter> characters { get; set; }
        public Belong currentBelong { get; set; }
        public BattleMode battleMode { get; set; }
        public TileMap mapSearch { get; set; }
        public AStar aStar { get; set; }
        public BreadthFirst breadthFirst { get; set; }
        public override IEnumerator OnLoad( Request request ) {
            LSharpInit();
            yield return this.StartCoroutine(base.OnLoad(request));
            Service.SUser sUser = Util.Global.SUser;
            string url = Service.HttpClient.assetBandleURL + "maps/maps_001.unity3d";
            yield return this.StartCoroutine(sUser.Download(url, 0, InitMap));
            InitManager();
            InitCharacters();
            this.dispatcher.Notify();
        }
        private void LSharpInit()
        {
        }
        public void InitCharacters()
        {

            characters = new List<Model.Character.MCharacter>();
            Model.Character.MCharacter mCharacter = new Model.Character.MCharacter();
            mCharacter.id = 0;
            mCharacter.characterId = 1;
            mCharacter.level = 1;
            mCharacter.weapon = 1;
            mCharacter.clothes = 1;
            mCharacter.horse = 1;
            //mCharacter.head = 1;
            mCharacter.coordinate.x = 0;
            mCharacter.coordinate.y = 1;
            mCharacter.StatusInit();
            characters.Add(mCharacter);
            mCharacter = new Model.Character.MCharacter();
            mCharacter.id = 1;
            mCharacter.characterId = 2;
            mCharacter.level = 1;
            mCharacter.weapon = 2;
            mCharacter.clothes = 2;
            mCharacter.horse = 2;
            //mCharacter.head = 2;
            mCharacter.coordinate.x = 2;
            mCharacter.coordinate.y = 3;
            mCharacter.StatusInit();
            characters.Add(mCharacter);
            this.dispatcher.Set("characters", characters);
        }
        public void InitMap(AssetBundle assetbundle)
        {
            Object[] ts = assetbundle.LoadAllAssets<Object>();
            GameObject obj = ts[0] as GameObject;
            this.dispatcher.Set("tileMap", obj);
            this.dispatcher.Set("camera3d", camera3d);
            Model.Scriptable.MapAsset mapAsset = ts[1] as Model.Scriptable.MapAsset;
            this.dispatcher.Set("map", mapAsset.map.tiles);
            assetbundle.Unload(false);
        }
        public void InitManager(){
            Global.battleManager.Init(this);
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
                    Global.battleManager.ClickNoneNode(coordinate);
                    break;
                case BattleMode.show_move_tiles:
                    Global.battleManager.ClickMovingNode(coordinate);
                    break;
                case BattleMode.move_end:
                    Global.battleManager.ClickSkillNode(coordinate);
                    break;
                case BattleMode.move_after_attack:
                    Global.battleManager.ClickMovingNode(coordinate);
                    break;
            }
        }
    }
}
