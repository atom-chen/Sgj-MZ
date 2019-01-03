using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Model;
using App.Model.Character;
using App.Util;
using App.Util.Cacher;
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
        private List<MCharacter> selectedCharacters;
        public override IEnumerator OnLoad( Request request )
        {
            Model.Master.MBattlefield battlefieldMaster = request.Get<Model.Master.MBattlefield>("mBattlefield");
            selectedCharacters = request.Get<List<MCharacter>>("selectedCharacters");
            Dictionary<int, bool> characterIds = new Dictionary<int, bool>();
            selectedCharacters.ForEach(chara => {
                characterIds.Add(chara.characterId, true);
            }); 
            yield return this.StartCoroutine(base.OnLoad(request));
            Service.SUser sUser = Util.Global.SUser;
            string url = Service.HttpClient.assetBandleURL + "maps/maps_001.unity3d";
            yield return this.StartCoroutine(sUser.Download(url, 0, InitMap));
            InitManager();
            InitCharacters(characterIds, battlefieldMaster);
            AddEvents();
            this.dispatcher.Notify();
            LSharpInit();
        }
        private void LSharpInit()
        {
        }
        /// <summary>
        /// 结束胜利
        /// </summary>
        private void BattleOver(bool isWin)
        {
            List<MCharacter> characters = Global.battleManager.charactersManager.mCharacters;
            Request req = Request.Create("selectedCharacters", selectedCharacters, "isWin", isWin);
            this.StartCoroutine(Global.AppManager.ShowDialog(Util.Dialog.BattleResultDialog, req));
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
           Global.battleManager.charactersManager.ActionRestore();
            System.Action closeEvent = () => {
                this.StartCoroutine(Global.battleEvent.OnBoutStart());
            };
            foreach (MCharacter mCharacter in Global.battleManager.charactersManager.mCharacters)
           {
               mCharacter.boutEventComplete = false;
           }
           Request req = Request.Create("belong", belong, "bout", boutCount, "maxBout", maxBout, "closeEvent", closeEvent);
           this.StartCoroutine(Global.AppManager.ShowDialog(Util.Dialog.BoutWaveDialog, req));
        }
        private void OpenOperatingMenu(){
            operatingMenu.Open();
            battleMenu.Close(null);
        }
        private void CloseOperatingMenu(){
            operatingMenu.Close(null);
            if (Global.battleManager.characterIsRunning)
            {
                return;
            }
            if (Global.battleManager.currentBelong != Belong.self)
            {
                Global.battleManager.aiManager.Execute(Global.battleManager.currentBelong);
            }
            else
            {
                battleMenu.Open();
            }
        }
        void ChangeBattleCharacterPreviewDialog(MCharacter mCharacter)
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
        public void InitCharacters(Dictionary<int, bool> characterIds, Model.Master.MBattlefield battlefieldMaster)
        {
            int index = 0;
            List<MCharacter> characters = Global.battleManager.charactersManager.mCharacters;
            characters.Clear();
            System.Array.ForEach(Global.SUser.self.characters, (model) =>
            {
                if (characterIds.ContainsKey(model.characterId))
                {
                    Model.Master.MBattleOwn own = battlefieldMaster.owns[index++];
                    model.belong = Belong.self;
                    model.coordinate.x = own.x;
                    model.coordinate.y = own.y;
                    CharacterInit(model);
                    characters.Add(model); 
                }
            });
            foreach (Model.Master.MBattleNpc battleNpc in battlefieldMaster.enemys)
            {
                MCharacter mCharacter = NpcCacher.Instance.GetFromBattleNpc(battleNpc);
                mCharacter.belong = Belong.enemy;
                CharacterInit(mCharacter);
                characters.Add(mCharacter);
            }
            foreach (App.Model.Master.MBattleNpc battleNpc in battlefieldMaster.friends)
            {
                MCharacter mCharacter = NpcCacher.Instance.GetFromBattleNpc(battleNpc);
                mCharacter.belong = Belong.friend;
                CharacterInit(mCharacter);
                characters.Add(mCharacter);
            }
            this.dispatcher.Set("characters", characters);
        }
        /// <summary>
        /// 武将初始化
        /// </summary>
        /// <param name="mCharacter">M character.</param>
        private void CharacterInit(MCharacter mCharacter)
        {
            mCharacter.StatusInit();
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
        void AddEvents()
        {
            Global.battleEvent.OperatingMenuHandler += ChangeOperatingMenu;
            Global.battleEvent.CharacterPreviewHandler += ChangeBattleCharacterPreviewDialog;
            Global.battleEvent.BelongChangeHandler += BoutWave;
            Global.battleEvent.BattleOverHandler += BattleOver;
        }
        void RemoveEvents()
        {
            Global.battleEvent.OperatingMenuHandler -= ChangeOperatingMenu;
            Global.battleEvent.CharacterPreviewHandler -= ChangeBattleCharacterPreviewDialog;
            Global.battleEvent.BelongChangeHandler -= BoutWave;
            Global.battleEvent.BattleOverHandler -= BattleOver;
        }
        void OnDestroy()
        {
            RemoveEvents();
        }
    }
}
