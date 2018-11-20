using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;


namespace App.Service{
    /**
     * Master数据更新用API
    */
	public class SEditorMaster : SBase {
        public ResponseAll responseAll;
        public ResponseBase response;
        public SEditorMaster(){
        }
        public class ResponseAll : ResponseBase
        {
            public App.Model.Master.MCharacter[] characters;
            public App.Model.Master.MTile[] tiles;
            //public App.Model.Master.MBaseMap[] base_maps;
            //public App.Model.Master.MBuilding[] buildings;
            //public App.Model.Master.MWorld[] worlds;
            //public App.Model.Master.MArea[] areas;
            //public App.Model.Master.MBattlefield[] battlefields;
            public App.Model.Master.MEquipment[] weapons;
            public App.Model.Master.MEquipment[] horses;
            public App.Model.Master.MEquipment[] clothes;
            public App.Model.Master.MSkill[] skills;
            //public App.Model.Master.MStrategy[] strategys;
            //public App.Model.Master.MItem[] items;
            //public App.Model.Master.MShopItem[] shop_items;
            //public App.Model.Master.MMission[] missions;
            //public App.Model.Master.MGacha[] gachas;
            //public App.Model.Master.MConstant constant;
            //public App.Model.Master.MWord[] words;
            public App.Model.Master.MNpcEquipment[] npc_equipments;
            public App.Model.Master.MNpc[] npcs;
            //public App.Model.Master.MLoginBonus[] loginbonus;
            //public App.Model.Master.MExp[] exps;
            //public App.Model.Master.MCharacterStar[] character_stars;
            public App.MyEditor.MAvatar avatar;
            public List<string>[] tutorials;
            public List<string>[] scenarios;
            public int[] scenario_ids;
            public string[] story_progress_keys;
        }
        public IEnumerator RequestAll(string type = "")
        {
            var url = "master/alldata";
            WWWForm form = new WWWForm();
            if (string.IsNullOrEmpty(type))
            {
                form.AddField("character", 1);
                form.AddField("tile", 1);
            }
            else
            {
                form.AddField(type, 1);
            }
            HttpClient client = new HttpClient();
            yield return App.Util.AppManager.CurrentScene.StartCoroutine(client.Send(url, form));
            responseAll = client.Deserialize<ResponseAll>();
        }
        /*
        public IEnumerator RequestSetBasemap(int id,int width, int height,string tile_ids)
        {
            var url = "tool/set_basemap";
            WWWForm form = new WWWForm();
            form.AddField("id", id);
            form.AddField("width", width);
            form.AddField("height", height);
            form.AddField("tile_ids", tile_ids);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            response = client.Deserialize<ResponseBase>();
        }
        public IEnumerator RequestSetWorld(List<App.Model.Master.MWorld> worlds)
        {
            var url = "tool/set_world";
            WWWForm form = new WWWForm();
            form.AddField("worlds", JsonFx.JsonWriter.Serialize(worlds));
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            response = client.Deserialize<ResponseBase>();
        }
        public IEnumerator RequestSetStage(int world_id, List<App.Model.Master.MArea> stages)
        {
            var url = "tool/set_stage";
            WWWForm form = new WWWForm();
            form.AddField("world_id", world_id);
            form.AddField("stages", JsonFx.JsonWriter.Serialize(stages));
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            response = client.Deserialize<ResponseBase>();
        }

        public IEnumerator RequestUserReset(string user_id)
        {
            var url = "tool/user_reset";
            WWWForm form = new WWWForm();
            form.AddField("user_id", user_id);
            HttpClient client = new HttpClient();
            yield return App.Util.SceneManager.CurrentScene.StartCoroutine(client.Send( url, form));
            response = client.Deserialize<ResponseBase>();
        }*/
    }
}