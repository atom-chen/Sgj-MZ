using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using System.IO;
using App.Controller.Common;
using App.Model.Scriptable;
using App.Util.Cacher;
using App.Util;

#if UNITY_EDITOR

namespace App.Controller{
    /*
    public class CTopScene : CBaseMap {
        [SerializeField] private int width = 30;
        [SerializeField] private int height = 30;
        [SerializeField] private VTile tileUnit;
        private bool loadComplete = false;
        private App.Model.Master.MBaseMap baseMapMaster;

        private bool createMapOk = false;
        private App.Model.Master.MTile currentTile = null;
        private App.View.VTile currentVTile = null;
        private bool isBuild = false;
        private bool isCheck = false;
        private bool deleteBuild = false;
        private int setId = 1;
        private int mapId = 1;
        //private int worldId = 0;
        private string buildName = "";
        public override IEnumerator OnLoad( Request req ) 
		{  
			yield return 0;
        }
        void OnGUI()
        {
            if (!loadComplete)
            {
                GUI.Label(new Rect(100, 50, 100, 30), "Loading");
                return;
            }
            if (createMapOk)
            {
                GUI.Label(new Rect(10, 10, 100, 30), currentTile.name);
                if (GUI.Button(new Rect(50, 50, 100, 30), "isBuild:"+isBuild))
                {
                    isBuild = !isBuild;
                }
                if (GUI.Button(new Rect(160, 50, 100, 30), "isCheck:"+isCheck))
                {
                    isCheck = !isCheck;
                }
                if (isBuild)
                {
                    if (GUI.Button(new Rect(160, 50, 100, 30), "delete:"+deleteBuild))
                    {
                        deleteBuild = !deleteBuild;
                    }
                }
                ChangeCurrentTile();
                buildName = GUI.TextField(new Rect(120, 10, 100, 30),buildName);
                GUI.Label(new Rect(230, 10, 100, 30), "id:");
                setId = int.Parse(GUI.TextField(new Rect(250, 10, 50, 30),setId.ToString()));
                GUI.Label(new Rect(310, 10, 100, 30), "mapId:");
                mapId = int.Parse(GUI.TextField(new Rect(350, 10, 50, 30),mapId.ToString()));
                if (GUI.Button(new Rect(350, 50, 150, 30), "save basemap"))
                {
                    StartCoroutine(SaveBaseMap());
                }
                if (GUI.Button(new Rect(350, 80, 150, 30), "save world"))
                {
                    CConfirmDialog.Show("确认","保存World吗？",()=>{
                        StartCoroutine(SaveWorld());
                    });
                }
                if (GUI.Button(new Rect(350, 110, 150, 30), "change world id"))
                {
                    if (currentVTile != null)
                    {
                        currentVTile.Id = setId;
                    }
                }
                if (GUI.Button(new Rect(350, 140, 150, 30), "change world mapId"))
                {
                    if (currentVTile != null)
                    {
                        currentVTile.MapId = mapId;
                    }
                }
                if (GUI.Button(new Rect(350, 170, 150, 30), "save stage"))
                {
                    StartCoroutine(SaveStage());
                }
            }
            else
            {
                width = int.Parse(GUI.TextField(new Rect(50, 10, 50, 30),width.ToString()));
                height = int.Parse(GUI.TextField(new Rect(150, 10, 50, 30),height.ToString()));
                setId = int.Parse(GUI.TextField(new Rect(350, 10, 50, 30),setId.ToString()));
                if (GUI.Button(new Rect(50, 50, 100, 30), "createMap"))
                {
                    CreateMap(vBaseMap);
                    createMapOk = true;
                    currentTile = TileCacher.Instance.Get(1);
                }
                if (GUI.Button(new Rect(350, 50, 100, 30), "loadMap"))
                {
                    StartCoroutine(LoadMap(vBaseMap));
                    createMapOk = true;
                    currentTile = TileCacher.Instance.Get(1);
                }
                if (GUI.Button(new Rect(350, 100, 100, 30), "loadWorld"))
                {
                    StartCoroutine(LoadMap(vBaseMap, "world"));
                    createMapOk = true;
                    currentTile = TileCacher.Instance.Get(2001);
                }
                if (GUI.Button(new Rect(350, 150, 100, 30), "loadStage"))
                {
                    StartCoroutine(LoadMap(vBaseMap, "stage"));
                    createMapOk = true;
                    currentTile = TileCacher.Instance.Get(1001);
                }
            }

        }
        private void ChangeCurrentTile(){
            App.Model.Master.MTile[] tiles = TileCacher.Instance.GetAll();
            int i = 0;
            int j = 0;
            foreach(App.Model.Master.MTile tile in tiles){
                if (GUI.Button(new Rect(50 + i*110, 100 + j * 40, 100, 30), tile.name))
                {
                    currentTile = tile;
                }
                j++;
                if (j >= 15)
                {
                    j = 0;
                    i++;
                }
            }
        }
        private IEnumerator SaveStage(){
            SEditorMaster service = new SEditorMaster();
            List<App.Model.Master.MArea> mTiles = new List<App.Model.Master.MArea>();
            foreach(VTile vTile in vBaseMap.tileUnits){
                if (!vTile.gameObject.activeSelf || vTile.BuildingId == 0)
                {
                    continue;
                }
                App.Model.Master.MArea stage = new App.Model.Master.MArea();
                stage.map_id = setId;
                stage.tile_id = vTile.BuildingId;
                stage.x = vTile.CoordinateX;
                stage.y = vTile.CoordinateY;
                mTiles.Add(stage);
            }
            yield return StartCoroutine(service.RequestSetStage(setId, mTiles));
            Debug.LogError("result = " + service.response.result);
        }
        private IEnumerator SaveWorld(){
            SEditorMaster service = new SEditorMaster();
            List<App.Model.Master.MWorld> worlds = new List<App.Model.Master.MWorld>();
            foreach(VTile vTile in vBaseMap.tileUnits){
                if (!vTile.gameObject.activeSelf || vTile.BuildingId == 0)
                {
                    continue;
                }
                App.Model.Master.MWorld mWorld = new App.Model.Master.MWorld();
                mWorld.id = vTile.Id;
                mWorld.map_id = vTile.MapId;
                mWorld.tile_id = vTile.BuildingId;
                mWorld.x = vTile.CoordinateX;
                mWorld.y = vTile.CoordinateY;
                mWorld.build_name = vTile.tileName.text;
                worlds.Add(mWorld);
            }
            yield return StartCoroutine(service.RequestSetWorld(worlds));
            Debug.LogError("result = " + service.response.result);
        }
        private IEnumerator SaveBaseMap(){
            SEditorMaster service = new SEditorMaster();
            string tile_ids = "";
            string plus = "";
            foreach(VTile vTile in vBaseMap.tileUnits){
                if (!vTile.gameObject.activeSelf)
                {
                    continue;
                }
                tile_ids += plus + vTile.TileId;
                plus = ",";
            }
            yield return StartCoroutine(service.RequestSetBasemap(setId, baseMapMaster.width, baseMapMaster.height, tile_ids));
            Debug.LogError("result = " + service.response.result);
        }
        public override void OnClickTile(int index){
            App.Model.Master.MBaseMap topMapMaster = baseMapMaster;
            Vector2 coordinate = topMapMaster.GetCoordinateFromIndex(index);
            App.View.VTile vTile = this.mapSearch.GetTile(coordinate);
            Debug.Log("vTile:"+vTile.Index+","+vTile.TileId+",("+vTile.CoordinateX+","+vTile.CoordinateY+")");
            if (isCheck)
            {
                return;
            }
            if (isBuild)
            {
                if (vTile.BuildingId > 0)
                {
                    if (deleteBuild)
                    {
                        vTile.SetData(vTile.Index, vTile.CoordinateX, vTile.CoordinateY, vTile.TileId, 0);
                        currentVTile = null;
                    }
                    else
                    {
                        currentVTile = vTile;
                        setId = vTile.Id;
                        mapId = vTile.MapId;
                        buildName = vTile.tileName.text;
                    }
                }
                else if(currentTile.id > 1000)
                {
                    if (vTile.BuildingId == 0 || currentVTile.TileId != vTile.TileId)
                    {
                        vTile.SetData(vTile.Index, vTile.CoordinateX, vTile.CoordinateY, vTile.TileId, currentTile.id, buildName);
                        vTile.Id = setId;
                        vTile.MapId = mapId;
                        currentVTile = vTile;
                    }
                }
            }
            else
            {
                vTile.SetData(vTile.Index, vTile.CoordinateX, vTile.CoordinateY, currentTile.id, vTile.BuildingId, vTile.tileName.text);
                currentVTile = null;
            }
            //OnClickTile(tile);
        }
        IEnumerator LoadMap(VBaseMap vBaseMap, string tableName = "map")
        {
            SEditorMaster sMaster = new SEditorMaster();
            int mapId = setId;
            App.Model.Master.MBaseMap base_map = null;
            App.Model.Master.MWorld[] worlds = null;
            App.Model.MTile[] stages = null;
            if (!string.IsNullOrEmpty(tableName))
            {
                yield return StartCoroutine(sMaster.RequestAll("world"));
                worlds = sMaster.responseAll.worlds;
                if (tableName == "world")
                {
                    mapId = Global.Constant.world_map_id;
                }
                else if (tableName == "stage")
                {
                    mapId = setId;
                    App.Model.Master.MWorld world = System.Array.Find(worlds, w=>w.map_id == mapId);
                    stages = world.stages;
                }
                else if (tableName == "map")
                {
                    mapId = setId;
                }
            }

            yield return StartCoroutine (sMaster.RequestAll("base_map"));
            base_map = System.Array.Find(sMaster.responseAll.base_maps, b=>b.id == mapId);

            if (base_map == null)
            {
                Debug.LogError("not have map");
                yield break;
            }
            List<VTile> tiles = new List<VTile>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject obj = GameObject.Instantiate (tileUnit.gameObject);
                    obj.SetActive(true);
                    obj.name = "Tile_"+(i + 1)+"_"+(j + 1);
                    obj.transform.parent = vBaseMap.transform;
                    obj.transform.localPosition = new Vector3(j * 0.69f + (i % 2) * 0.345f, -i * 0.617f, 0f);
                    VTile vTile = obj.GetComponent<VTile>();
                    vTile.tileSprite.sprite = null;
                    vTile.buildingSprite.sprite = null;
                    vTile.lineSprite.sprite = null;
                    tiles.Add(vTile);
                }
            }
            vBaseMap.mapWidth = width;
            vBaseMap.mapHeight = height;
            vBaseMap.tileUnits = tiles.ToArray();


            mBaseMap = new MBaseMap();
            mBaseMap.Tiles = new MTile[]{};
            vBaseMap.BindingContext = mBaseMap.ViewModel;

            baseMapMaster = base_map;
            vBaseMap.ResetAll(baseMapMaster);
            vBaseMap.MoveToPosition(0,0, baseMapMaster);
            //savePrefab(vBaseMap.gameObject, vBaseMap.gameObject.name);
            mapSearch = new App.Util.Search.TileMap(mBaseMap, vBaseMap);
            if (tableName == "world")
            {
                foreach (App.Model.Master.MWorld world in worlds)
                {
                    VTile vTile = mapSearch.GetTile(world.x, world.y);
                    vTile.MapId = world.map_id;
                    vTile.Id = world.id;
                    vTile.SetData(vTile.Index, vTile.CoordinateX, vTile.CoordinateY, vTile.TileId, world.tile_id, world.build_name);
                }
            }
            else if (tableName == "stage")
            {
                foreach (App.Model.MTile mTile in stages)
                {
                    VTile vTile = mapSearch.GetTile(mTile.x, mTile.y);
                    vTile.MapId = mapId;
                    vTile.SetData(vTile.Index, vTile.CoordinateX, vTile.CoordinateY, vTile.TileId, mTile.tile_id);
                }
            }
        }
        void CreateMap(VBaseMap vBaseMap)
        {
            List<VTile> tiles = new List<VTile>();
            List<int> tileIds = new List<int>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject obj = GameObject.Instantiate (tileUnit.gameObject);
                    obj.SetActive(true);
                    obj.name = "Tile_"+(i + 1)+"_"+(j + 1);
                    obj.transform.parent = vBaseMap.transform;
                    obj.transform.localPosition = new Vector3(j * 0.69f + (i % 2) * 0.345f, -i * 0.617f, 0f);
                    VTile vTile = obj.GetComponent<VTile>();
                    vTile.tileSprite.sprite = null;
                    vTile.buildingSprite.sprite = null;
                    vTile.lineSprite.sprite = null;
                    //vTile.SetData(1,0,0,1,0);
                    tiles.Add(vTile);
                    tileIds.Add(1);
                }
            }
            vBaseMap.mapWidth = width;
            vBaseMap.mapHeight = height;
            vBaseMap.tileUnits = tiles.ToArray();


            mBaseMap = new MBaseMap();
            mBaseMap.Tiles = new MTile[]{};
            vBaseMap.BindingContext = mBaseMap.ViewModel;

            baseMapMaster = new App.Model.Master.MBaseMap();
            baseMapMaster.width = width;
            baseMapMaster.height = height;
            baseMapMaster.tile_ids = tileIds.ToArray();
            vBaseMap.ResetAll(baseMapMaster);
            vBaseMap.MoveToPosition(0,0, baseMapMaster);
            //savePrefab(vBaseMap.gameObject, vBaseMap.gameObject.name);
            mapSearch = new App.Util.Search.TileMap(mBaseMap, vBaseMap);
        }

        public override IEnumerator Start()
        {
            Caching.ClearCache();
            Global.Initialize();
            yield return StartCoroutine (base.Start());
            //SEditorMaster sMaster = new SEditorMaster();
            MVersion versions = new MVersion();
            SUser sUser = Global.SUser;
            List<IEnumerator> list = new List<IEnumerator>();
            list.Add(sUser.Download(ImageAssetBundleManager.mapUrl, versions.map, (AssetBundle assetbundle)=>{
                ImageAssetBundleManager.map = assetbundle;
            }, false));
            list.Add(sUser.Download(TileAsset.Url, versions.tile, (AssetBundle assetbundle)=>{
                TileAsset.assetbundle = assetbundle;
                TileCacher.Instance.Reset(TileAsset.Data.tiles);
                TileAsset.Clear();
            }));
            list.Add(sUser.Download(ConstantAsset.Url, versions.constant, (AssetBundle assetbundle)=>{
                ConstantAsset.assetbundle = assetbundle;
                Global.Constant = ConstantAsset.Data.constant;
            }));
            Debug.Log("Start");
            for (int i = 0; i < list.Count; i++)
            {
                Debug.Log(i+"/"+list.Count);
                yield return this.StartCoroutine(list[i]);
            }
            Debug.Log("Start Over");
            loadComplete = true;
        }
	}*/
}
#endif