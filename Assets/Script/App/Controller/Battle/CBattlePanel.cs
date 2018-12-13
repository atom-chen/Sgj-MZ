using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Model;
using App.Util;
using App.View.Battle;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Battle
{
    public class CBattlePanel : CPanel
    {
        [SerializeField] private Camera camera3d;
        [SerializeField] private VBottomMenu operatingMenu;
        [SerializeField] private VBottomMenu battleMenu;
        [SerializeField] private VBaseListChild characterPreview;
        private int boutCount = 0;
        private int maxBout = 0;
        public override IEnumerator OnLoad( Request request ) {
            LSharpInit();
            yield return this.StartCoroutine(base.OnLoad(request));
            Service.SUser sUser = Util.Global.SUser;
            string url = Service.HttpClient.assetBandleURL + "maps/maps_001.unity3d";
            yield return this.StartCoroutine(sUser.Download(url, 0, InitMap));
            InitManager();
            InitCharacters(); 
            Global.battleEvent.OperatingMenuHandler += ChangeOperatingMenu;
            Global.battleEvent.CharacterPreviewHandler += ChangeBattleCharacterPreviewDialog;
            Global.battleEvent.BelongChangeHandler += BoutWave;
            this.dispatcher.Notify();
        }
        private void LSharpInit()
        {
        }
        private void ChangeOperatingMenu(bool value)
        {
            if(value){
                OpenOperatingMenu();
            }else{
                CloseOperatingMenu();
            }
        }
        private void BoutWave(Belong belong)
        {
            Debug.LogError("BoutWave belong="+ belong);
            Global.battleManager.currentBelong = belong;
            if (belong == Belong.self)
            {
                boutCount++;
            }
            /*Global.battleManager.charactersManager.ActionRestore();
            System.Action closeEvent = () => {
                this.StartCoroutine(OnBoutStart());
            };
            foreach (MCharacter mCharacter in mBaseMap.Characters)
            {
                mCharacter.boutEventComplete = false;
            }
            Request req = Request.Create("belong", belong, "bout", boutCount, "maxBout", maxBout, "closeEvent", closeEvent);
            this.StartCoroutine(Global.AppManager.ShowDialog(Prefabs.BoutWaveDialog, req));
            */           
        }
        private void OpenOperatingMenu(){
            operatingMenu.Open();
            battleMenu.Close(null);
        }
        private void CloseOperatingMenu(){
            operatingMenu.Close(null);
            /*if (Global.battleManager.currentCharacter != null)
            {
                return;
            }*/
            if (Global.battleManager.currentBelong != Belong.self)
            {
                //ai.Execute(Global.battleManager.currentBelong);
            }
            else
            {
                battleMenu.Open();
            }
        }
        void ChangeBattleCharacterPreviewDialog(Model.Character.MCharacter mCharacter)
        {
            if(mCharacter == null)
            {
                characterPreview.gameObject.SetActive(false);
            }
            else
            {
                characterPreview.gameObject.SetActive(true);
                characterPreview.UpdateView(mCharacter);
            }
        }
        public void InitCharacters()
        {
            List<Model.Character.MCharacter> characters = Global.battleManager.charactersManager.mCharacters;
            Model.Character.MCharacter mCharacter = Global.SUser.self.characters[0];
            mCharacter.coordinate.x = 0;
            mCharacter.coordinate.y = 1;
            characters.Add(mCharacter);
            mCharacter = Global.SUser.self.characters[1];
            mCharacter.coordinate.x = 2;
            mCharacter.coordinate.y = 4;
            characters.Add(mCharacter);

            mCharacter = Global.SUser.self.characters[2];
            mCharacter.belong = Belong.enemy;
            mCharacter.coordinate.x = 2;
            mCharacter.coordinate.y = 3;
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
            Global.battleManager.Init();

        }
        public void OnClickTile(View.Map.VTile vTile)
        {
            Debug.LogError("OnClickTile:" + vTile.mTile.id);
            Vector2Int coordinate = vTile.coordinate;
            if (Global.battleManager.currentBelong != Belong.self)
            {
                return;
            }
            switch (Global.battleManager.battleMode)
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
