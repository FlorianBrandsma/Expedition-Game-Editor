using UnityEngine;
using System.Collections.Generic;
using System.Linq;

static public class Fixtures
{
    static public int timeFrames = 2;

    static public int projects = 1;

    static public int supplies = 4;
    static public int gear = 6;
    static public int spoils = 10;
    static public int interactables = 6;
    static public int models = 15;
    static public int icons = 9;
    static public int regions = 2;
    static public int terrainsInRegions = 3;
    static public int terrainTilesInTerrains = 5;
    static public int chapters = 3;
    static public int chapterWorldInteractableInChapter = 1;
    static public int chapterInteractablesInChapter = 3;
    static public int phasesInChapter = 4;
    static public int questsInPhase = 4;
    static public int objectivesInQuest = 3;
    static public int interactablesInObjective = 2;
    static public int sideWorldInteractables = 1;
    static public int interactionsPerWorldInteractable = 2;
    static public int scenesPerOutcome = 2;
    static public int baseTasks = 2;
    static public int tileSets = 2;
    static public int tilesInTileSet = 10;
    static public int objectsInWorld = 3;

    static public int saves = 1;

    static public List<IconBaseData>                    iconList                    = new List<IconBaseData>();
    static public List<ModelBaseData>                   modelList                   = new List<ModelBaseData>();

    static public List<UserBaseData>                    userList                    = new List<UserBaseData>();
    static public List<FavoriteUserBaseData>            favoriteUserList            = new List<FavoriteUserBaseData>();
    static public List<TeamBaseData>                    teamList                    = new List<TeamBaseData>();
    static public List<TeamUserBaseData>                teamUserList                = new List<TeamUserBaseData>();
    static public List<ProjectBaseData>                 projectList                 = new List<ProjectBaseData>();
    
    static public List<ItemBaseData>                    itemList                    = new List<ItemBaseData>();
    static public List<InteractableBaseData>            interactableList            = new List<InteractableBaseData>();
    static public List<TileSetBaseData>                 tileSetList                 = new List<TileSetBaseData>();
    static public List<TileBaseData>                    tileList                    = new List<TileBaseData>();
    static public List<RegionBaseData>                  regionList                  = new List<RegionBaseData>();
    static public List<AtmosphereBaseData>              atmosphereList              = new List<AtmosphereBaseData>();
    static public List<WorldObjectBaseData>             worldObjectList             = new List<WorldObjectBaseData>();
    static public List<TerrainBaseData>                 terrainList                 = new List<TerrainBaseData>();
    static public List<TerrainTileBaseData>             terrainTileList             = new List<TerrainTileBaseData>();
    static public List<ChapterBaseData>                 chapterList                 = new List<ChapterBaseData>();
    static public List<ChapterInteractableBaseData>     chapterInteractableList     = new List<ChapterInteractableBaseData>();
    static public List<ChapterRegionBaseData>           chapterRegionList           = new List<ChapterRegionBaseData>();
    static public List<PhaseBaseData>                   phaseList                   = new List<PhaseBaseData>();
    static public List<QuestBaseData>                   questList                   = new List<QuestBaseData>();
    static public List<ObjectiveBaseData>               objectiveList               = new List<ObjectiveBaseData>();
    static public List<WorldInteractableBaseData>       worldInteractableList       = new List<WorldInteractableBaseData>();
    static public List<TaskBaseData>                    taskList                    = new List<TaskBaseData>();
    static public List<InteractionBaseData>             interactionList             = new List<InteractionBaseData>();
    static public List<InteractionDestinationBaseData>  interactionDestinationList  = new List<InteractionDestinationBaseData>();
    static public List<OutcomeBaseData>                 outcomeList                 = new List<OutcomeBaseData>();
    static public List<SceneBaseData>                   sceneList                   = new List<SceneBaseData>();
    static public List<SceneShotBaseData>               sceneShotList               = new List<SceneShotBaseData>();
    static public List<CameraFilterBaseData>            cameraFilterList            = new List<CameraFilterBaseData>();
    static public List<SceneActorBaseData>              sceneActorList              = new List<SceneActorBaseData>();
    static public List<ScenePropBaseData>               scenePropList               = new List<ScenePropBaseData>();

    static public List<GameBaseData>                    gameList                    = new List<GameBaseData>();

    static public List<SaveBaseData>                    saveList                    = new List<SaveBaseData>();
    static public List<InteractableSaveBaseData>        interactableSaveList        = new List<InteractableSaveBaseData>();
    static public List<ChapterSaveBaseData>             chapterSaveList             = new List<ChapterSaveBaseData>();
    static public List<PhaseSaveBaseData>               phaseSaveList               = new List<PhaseSaveBaseData>();
    static public List<QuestSaveBaseData>               questSaveList               = new List<QuestSaveBaseData>();
    static public List<ObjectiveSaveBaseData>           objectiveSaveList           = new List<ObjectiveSaveBaseData>();
    static public List<TaskSaveBaseData>                taskSaveList                = new List<TaskSaveBaseData>();
    static public List<InteractionSaveBaseData>         interactionSaveList         = new List<InteractionSaveBaseData>();

    static public void LoadFixtures()
    {
        LoadIcons();
        LoadModels();
        LoadCameraFilters();
        LoadTileSets();
        LoadTiles();

        LoadUsers();
        LoadFavoriteUsers();
        LoadTeams();

        LoadProjects();

        Query();
    }

    #region Icons
    static public void LoadIcons()
    {
        /*01*/CreateIcon("Textures/Icons/Objects/Nothing",          Enums.IconCategory.Nothing);
        /*02*/CreateIcon("Textures/Icons/Objects/Polearm",          Enums.IconCategory.Polearm);
        /*03*/CreateIcon("Textures/Icons/Objects/MightyPolearm",    Enums.IconCategory.Polearm);
        /*04*/CreateIcon("Textures/Icons/Objects/Shortbow",         Enums.IconCategory.Bow);
        /*05*/CreateIcon("Textures/Icons/Objects/Longbow",          Enums.IconCategory.Bow);
        /*06*/CreateIcon("Textures/Icons/Objects/Crossbow",         Enums.IconCategory.Crossbow);
        /*07*/CreateIcon("Textures/Icons/Objects/StrongCrossbow",   Enums.IconCategory.Crossbow);
        /*08*/CreateIcon("Textures/Icons/Objects/Staff",            Enums.IconCategory.Staff);
        /*09*/CreateIcon("Textures/Icons/Objects/MenacingStaff",    Enums.IconCategory.Staff);
        /*10*/CreateIcon("Textures/Icons/Objects/RedWarrior",       Enums.IconCategory.Humanoid);
        /*11*/CreateIcon("Textures/Icons/Objects/BlueWarrior",      Enums.IconCategory.Humanoid);
        /*12*/CreateIcon("Textures/Icons/Objects/GreenWarrior",     Enums.IconCategory.Humanoid);
        /*13*/CreateIcon("Textures/Icons/Objects/Ranger",           Enums.IconCategory.Humanoid);
        /*14*/CreateIcon("Textures/Icons/Objects/Mage",             Enums.IconCategory.Humanoid);
        /*15*/CreateIcon("Textures/Icons/Objects/Drake",            Enums.IconCategory.Dragonkin);
        /*16*/CreateIcon("Textures/Icons/Objects/Goblin",           Enums.IconCategory.Goblin);
        /*17*/CreateIcon("Textures/Icons/Objects/Skull",            Enums.IconCategory.Environment);
        /*18*/CreateIcon("Textures/Icons/Objects/Rock",             Enums.IconCategory.Environment);
        /*19*/CreateIcon("Textures/Icons/Objects/Cactus",           Enums.IconCategory.Environment);
        /*20*/CreateIcon("Textures/Icons/Objects/Tree",             Enums.IconCategory.Environment);
        /*21*/CreateIcon("Textures/Icons/Objects/Pool",             Enums.IconCategory.Environment);
    }

    static public int CreateIcon(string path, Enums.IconCategory category)
    {
        var icon = new IconBaseData();

        int id = iconList.Count > 0 ? (iconList[iconList.Count - 1].Id + 1) : 1;

        icon.Id = id;
        icon.Category = (int)category;
        icon.Path = path;

        iconList.Add(icon);

        return id;
    }
    #endregion

    #region Models
    static public void LoadModels()
    {
        //Note: size values are not realistic until models have been made with these values in mind

        /*01*/CreateModel("Nothing",        1,  new Vector3(1,      1,      1));
        /*02*/CreateModel("Polearm",        2,  new Vector3(0.5f,   0.15f,  4));
        /*03*/CreateModel("Mighty Polearm", 3,  new Vector3(0.65f,  0.15f,  4));
        /*04*/CreateModel("Shortbow",       4,  new Vector3(0.5f,   0.1f,   2.75f));
        /*05*/CreateModel("Longbow",        5,  new Vector3(1,      0.25f,  3.5f));
        /*06*/CreateModel("Crossbow",       6,  new Vector3(1,      0.35f,  1.25f));
        /*07*/CreateModel("Strong Crossbow",7,  new Vector3(1.35f,  0.35f,  1.25f));
        /*08*/CreateModel("Staff",          8,  new Vector3(0.55f,  0.25f,  2.5f));
        /*09*/CreateModel("Menacing Staff", 9,  new Vector3(1.5f,   0.3f,   2.3f));
        /*10*/CreateModel("Red Warrior",    10, new Vector3(1,      1,      3.5f));
        /*11*/CreateModel("Blue Warrior",   11, new Vector3(1,      1,      3.5f));
        /*12*/CreateModel("Green Warrior",  12, new Vector3(1,      1,      3.5f));
        /*13*/CreateModel("Ranger",         13, new Vector3(1,      1,      3.3f));
        /*14*/CreateModel("Mage",           14, new Vector3(1,      1,      3.3f));
        /*15*/CreateModel("Drake",          15, new Vector3(1.5f,   3,      2));
        /*16*/CreateModel("Skull",          17, new Vector3(1.25f,  4.5f,   1.5f));
        /*17*/CreateModel("Rock",           18, new Vector3(4,      3,      2));
        /*18*/CreateModel("Cactus",         19, new Vector3(1,      1,      4));
        /*19*/CreateModel("Tree",           20, new Vector3(1,      1,      4));
        /*20*/CreateModel("Pool",           21, new Vector3(2.75f,  2.5f,   0.75f));
    }
    
    static public void CreateModel(string name, int iconId, Vector3 size)
    {
        var model = new ModelBaseData();

        int id = modelList.Count > 0 ? (modelList[modelList.Count - 1].Id + 1) : 1;

        model.Id = id;
        model.IconId = iconId;
        model.Name = name;
        model.Path = "Objects/" + name;
        model.Height = size.z;
        model.Width = size.x;
        model.Depth = size.y;

        modelList.Add(model);
    }
    #endregion

    #region Camera filters
    static public void LoadCameraFilters()
    {
        /*01*/CreateCameraFilter("FadeBlack", "Fade black");
        /*02*/CreateCameraFilter("FadeWhite", "Fade white");
    }

    static private void CreateCameraFilter(string path, string name)
    {
        var cameraFilter = new CameraFilterBaseData();

        int id = cameraFilterList.Count > 0 ? (cameraFilterList[cameraFilterList.Count - 1].Id + 1) : 1;

        cameraFilter.Id = id;

        cameraFilter.Path = "Textures/CameraFilters/" + path;
        cameraFilter.IconPath = "Textures/Icons/CameraFilters/" + path;

        cameraFilter.Name = name;

        cameraFilterList.Add(cameraFilter);
    }
    #endregion

    static public void LoadTileSets()
    {
        CreateTileSet("Sand");
        CreateTileSet("Snow");
    }

    static public void CreateTileSet(string name)
    {
        var tileSet = new TileSetBaseData();

        int id = tileSetList.Count > 0 ? (tileSetList[tileSetList.Count - 1].Id + 1) : 1;

        tileSet.Id = id;
        tileSet.Name = name;
        tileSet.TileSize = 31.75f;

        tileSetList.Add(tileSet);
    }

    static public void LoadTiles()
    {
        int index = 0;

        foreach(TileSetBaseData tileSet in tileSetList)
        {
            for (int i = 0; i < tilesInTileSet; i++)
            {
                var tile = new TileBaseData();

                int id = tileList.Count > 0 ? (tileList[tileList.Count - 1].Id + 1) : 1;

                tile.Id = id;
                tile.TileSetId = tileSet.Id;
                tile.IconPath = "Textures/Tiles/" + tileSet.Name + "/" + i;

                tileList.Add(tile);
            }

            index++;
        }
    }

    static private void LoadUsers()
    {
        CreateUser("Florian", "florianbrandsma@hotmail.nl");
        CreateUser("Yuuki", "yuuki@expedition.com");
    }

    static private void CreateUser(string username, string email)
    {
        var user = new UserBaseData();

        int id = userList.Count > 0 ? (userList[userList.Count - 1].Id + 1) : 1;

        user.Id = id;

        user.IconId = 10;

        user.Username = username;
        user.Email = email;
        user.Password = "test";

        userList.Add(user);
    }

    static private void LoadFavoriteUsers()
    {
        CreateFavoriteUser(userList.First().Id, userList.Last().Id);
    }

    static private void CreateFavoriteUser(int userId, int favoriteUserId)
    {
        var favoriteUser = new FavoriteUserBaseData();

        int id = favoriteUserList.Count > 0 ? (favoriteUserList[favoriteUserList.Count - 1].Id + 1) : 1;

        favoriteUser.Id = id;

        favoriteUser.UserId = userId;
        favoriteUser.FavoriteUserId = favoriteUserId;

        favoriteUser.Note = "My favorite user";

        favoriteUserList.Add(favoriteUser);
    }

    static private void LoadTeams()
    {
        CreateTeam("Team Zero");
    }

    static private void CreateTeam(string name)
    {
        var team = new TeamBaseData();

        int id = teamList.Count > 0 ? (teamList[teamList.Count - 1].Id + 1) : 1;

        team.Id = id;

        team.IconId = 16;

        team.Name = name;
        team.Description = "Team description";

        teamList.Add(team);

        CreateTeamUser(id, userList.First().Id, Enums.UserRole.ProjectManager, Enums.UserStatus.Joined);
        CreateTeamUser(id, userList.Last().Id, Enums.UserRole.Tester, Enums.UserStatus.Applied);
    }
    
    static private void CreateTeamUser(int teamId, int userId, Enums.UserRole role, Enums.UserStatus status)
    {
        var teamUser = new TeamUserBaseData();

        int id = teamUserList.Count > 0 ? (teamUserList[teamUserList.Count - 1].Id + 1) : 1;

        teamUser.Id = id;

        teamUser.TeamId = teamId;
        teamUser.UserId = userId;

        teamUser.Role = (int)role;

        teamUser.Status = (int)status;

        teamUserList.Add(teamUser);
    }

    static public void LoadProjects()
    {
        for (int i = 0; i < projects; i++)
        {
            int id = projectList.Count > 0 ? (projectList[projectList.Count - 1].Id + 1) : 1;

            CreateProject(id);

            LoadItems(id);
            LoadInteractableCharacters(id);
            LoadInteractableObjects(id);
            LoadRegions(id);

            LoadTerrains();
            LoadTerrainTiles();
            LoadWorldObjects();

            LoadChapters(id);
            LoadChapterWorldInteractables();
            LoadChapterInteractables();
            LoadChapterRegions();
            LoadPhases();
            LoadQuests();
            LoadPhaseWorldInteractables();
            LoadObjectives();
            LoadObjectiveWorldInteractables();
            LoadTasks();

            CreateGame(id);
        }
    }

    static private void CreateProject(int id)
    {
        var project = new ProjectBaseData();

        project.Id = id;

        project.TeamId = teamList.First().Id;
        project.IconId = 10;

        project.Name = "The Expedition";
        project.Description = "My very first project!";

        projectList.Add(project);
    }

    static private void CreateGame(int projectId)
    {
        var game = new GameBaseData();

        int id = gameList.Count > 0 ? (gameList[gameList.Count - 1].Id + 1) : 1;

        game.Id = id;

        game.ProjectId = projectId;

        game.Rating = 5;

        gameList.Add(game);

        LoadSaves(id);
    }

    static public void LoadItems(int projectId)
    {
        LoadSupplies(projectId);
        LoadGear(projectId);
        LoadSpoils(projectId);
    }

    static public void LoadSupplies(int projectId)
    {
        int index = 0;

        for (int i = 0; i < supplies; i++)
        {
            var item = new ItemBaseData();

            int id = itemList.Count > 0 ? (itemList[itemList.Count - 1].Id + 1) : 1;

            item.Id = id;

            item.ProjectId = projectId;
            item.ModelId = 1;

            item.Type = (int)Enums.ItemType.Supplies;

            item.Index = index;
            
            item.Name = "Item " + id;

            itemList.Add(item);

            index++;
        }
    }

    static public void LoadGear(int projectId)
    {
        int index = 0;

        var gearList = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };

        for (int i = 0; i < gearList.Count; i++)
        {
            var item = new ItemBaseData();

            int id = itemList.Count > 0 ? (itemList[itemList.Count - 1].Id + 1) : 1;

            item.Id = id;

            item.ProjectId = projectId;
            item.ModelId = gearList[i];

            item.Type = (int)Enums.ItemType.Gear;

            item.Index = index;
            
            item.Name = "Item " + id;

            itemList.Add(item);

            index++;
        }
    }

    static public void LoadSpoils(int projectId)
    {
        int index = 0;

        for (int i = 0; i < spoils; i++)
        {
            var item = new ItemBaseData();

            int id = itemList.Count > 0 ? (itemList[itemList.Count - 1].Id + 1) : 1;

            item.Id = id;

            item.ProjectId = projectId;
            item.ModelId = 1;

            item.Type = (int)Enums.ItemType.Spoils;

            item.Index = index;
            
            item.Name = "Item " + id;

            itemList.Add(item);

            index++;
        }
    }

    static public void LoadInteractableCharacters(int projectId)
    {
        var objectList = new List<int> { 10, 11, 12, 13, 14, 15 };

        for (int i = 0; i < objectList.Count; i++)
        {
            var interactable = new InteractableBaseData();

            int id = interactableList.Count > 0 ? (interactableList[interactableList.Count - 1].Id + 1) : 1;

            interactable.Id = id;

            interactable.ProjectId = projectId;        
            interactable.ModelId = objectList[i];

            interactable.Type = (int)Enums.InteractableType.Agent;

            interactable.Index = i;
            
            interactable.Name = "Interactable " + id;

            interactable.Scale = 1;

            interactable.Health = 100;
            interactable.Hunger = 100;
            interactable.Thirst = 100;

            interactable.Weight = 80f;
            interactable.Speed = 5f;
            interactable.Stamina = 50f;

            interactableList.Add(interactable);
        }
    }

    static public void LoadInteractableObjects(int projectId)
    {
        var objectList = new List<int> { 20 };

        for (int i = 0; i < objectList.Count; i++)
        {
            var interactable = new InteractableBaseData();

            int id = interactableList.Count > 0 ? (interactableList[interactableList.Count - 1].Id + 1) : 1;

            interactable.Id = id;

            interactable.ProjectId = projectId;
            interactable.ModelId = objectList[i];

            interactable.Type = (int)Enums.InteractableType.Object;

            interactable.Index = i;
            
            interactable.Name = "Interactable " + id;

            interactable.Scale = 1;

            interactableList.Add(interactable);
        }
    }

    static public void LoadRegions(int projectId)
    {
        for (int i = 0; i < regions; i++)
        {
            var region = new RegionBaseData();

            int id = regionList.Count > 0 ? (regionList[regionList.Count - 1].Id + 1) : 1;

            region.Id = id;

            region.ProjectId = projectId;
            region.ChapterRegionId = 0;
            region.PhaseId = 0;
            region.TileSetId = (i % tileSetList.Count) + 1;

            region.Index = i;
            
            region.Name = "Region " + id;
            region.RegionSize = terrainsInRegions;
            region.TerrainSize = terrainTilesInTerrains;

            regionList.Add(region);
        }
    }

    static public void LoadTerrains()
    {
        foreach(RegionBaseData region in regionList)
        {
            for(int i = 0; i < (region.RegionSize * region.RegionSize); i++)
            {
                var terrain = new TerrainBaseData();

                int id = terrainList.Count > 0 ? (terrainList[terrainList.Count - 1].Id + 1) : 1;

                terrain.Id = id;
                
                terrain.RegionId = region.Id;
                terrain.IconId = 1;

                terrain.Index = i;

                terrain.Name = "Terrain " + (i + 1);

                CreateAtmosphere(terrain, true, 0, 0);
                CreateAtmosphere(terrain, false, 8 * TimeManager.secondsInHour, (13 * TimeManager.secondsInHour) - 1);
                CreateAtmosphere(terrain, false, 16 * TimeManager.secondsInHour, (21 * TimeManager.secondsInHour) - 1);

                terrainList.Add(terrain);
            }
        }
    }

    static public void CreateAtmosphere(TerrainBaseData terrain, bool isDefault, int startTime, int endTime)
    {
        var atmosphere = new AtmosphereBaseData();

        int id = atmosphereList.Count > 0 ? (atmosphereList[atmosphereList.Count - 1].Id + 1) : 1;

        atmosphere.Id = id;

        atmosphere.TerrainId = terrain.Id;

        atmosphere.Default = isDefault;
        
        atmosphere.StartTime = startTime;
        atmosphere.EndTime = endTime;

        atmosphereList.Add(atmosphere);
    }

    static public void LoadTerrainTiles()
    {
        foreach(TerrainBaseData terrain in terrainList)
        {
            var regionData = regionList.Where(x => x.Id == terrain.RegionId).FirstOrDefault();
            var tileData = tileList.Where(x => x.TileSetId == regionData.TileSetId).ToList();

            for (int i = 0; i < (regionData.TerrainSize * regionData.TerrainSize); i++)
            {
                var terrainTile = new TerrainTileBaseData();

                int id = terrainTileList.Count > 0 ? (terrainTileList[terrainTileList.Count - 1].Id + 1) : 1;

                terrainTile.Id = id;
                
                terrainTile.TerrainId = terrain.Id;
                terrainTile.TileId = tileData.FirstOrDefault().Id;

                terrainTile.Index = i;

                terrainTileList.Add(terrainTile);
            }
        }
    }

    static public void LoadWorldObjects()
    {
        foreach (RegionBaseData region in regionList)
        {
            var terrains = terrainList.Where(x => x.RegionId == region.Id).Distinct().ToList();
            var middleTerrain = terrains[terrains.Count / 2];
            var terrainTiles = terrainTileList.Where(x => x.TerrainId == (middleTerrain.Id)).Distinct().ToList();
            var middleTile = terrainTiles[terrainTiles.Count / 2];

            /*Skull*/
            CreateWorldObject(16, region.Id, new Vector3(245f, -0.2f, 236.5f), new Vector3(355, 155, 0));

            /*Rock*/
            CreateWorldObject(17, region.Id, new Vector3(230f, 0f, 241.75f), new Vector3(0, 180, 0));

            /*Cactus*/
            CreateWorldObject(18, region.Id, new Vector3(246.5f, 0f, 236.75f), new Vector3(0, 180, 0));

            /*Red warrior*/
            //CreateWorldInteractable(Enums.InteractableType.Agent, 1, region.Id, new Vector3(238.125f, 0.1f, 239.875f), new Vector3(0, 180, 0));

            /*Ranger*/
            var rangerDestinationList = new List<InteractionDestinationBaseData>()
            {
                new InteractionDestinationBaseData()
                {
                    PositionX = 235.625f,
                    PositionY = 0.2f,
                    PositionZ = 242.375f,

                    ChangeRotation = true,

                    RotationX = 0,
                    RotationY = 130,
                    RotationZ = 0,

                    PositionVariance = 0,
                    Patience = 5
                },
                new InteractionDestinationBaseData()
                {
                    PositionX = 230f,
                    PositionY = 0.2f,
                    PositionZ = 235f,

                    ChangeRotation = true,

                    RotationX = 0,
                    RotationY = 130,
                    RotationZ = 0,

                    PositionVariance = 0,
                    Patience = 0
                },
                //new InteractionDestinationBaseData()
                //{
                //    PositionX = 15.875f,
                //    PositionY = 0.2f,
                //    PositionZ = 15.875f,

                //    RotationX = 0,
                //    RotationY = 130,
                //    RotationZ = 0,

                //    PositionVariance = 0,
                //    Patience = 5
                //},
                //new InteractionDestinationBaseData()
                //{
                //    PositionX = 15.875f,
                //    PositionY = 0.2f,
                //    PositionZ = 460.375f,

                //    RotationX = 0,
                //    RotationY = 130,
                //    RotationZ = 0,

                //    PositionVariance = 0,
                //    Patience = 5
                //},
                new InteractionDestinationBaseData()
                {
                    PositionX = 223f,
                    PositionY = 0.2f,
                    PositionZ = 246f,

                    ChangeRotation = true,

                    RotationX = 0,
                    RotationY = 130,
                    RotationZ = 0,

                    PositionVariance = 0,
                    Patience = 5
                }
            };

            CreateWorldInteractable(Enums.InteractableType.Agent, 4, region.Id, rangerDestinationList);

            /*Mage*/
            var mageDestinationList = new List<InteractionDestinationBaseData>()
            {
                new InteractionDestinationBaseData()
                {
                    PositionX = 240.625f,
                    PositionY = 0f,
                    PositionZ = 242.375f,

                    ChangeRotation = false,

                    RotationX = 0,
                    RotationY = 255,
                    RotationZ = 0,

                    PositionVariance = 0,
                    Patience = 3
                }
            };

            CreateWorldInteractable(Enums.InteractableType.Agent, 5, region.Id, mageDestinationList);

            //for (int i = 0; i < 100; i++)
            //{
            //    CreateWorldInteractable(Enums.InteractableType.Agent, 5, region.Id, mageDestinationList);
            //}

            /*Pool*/
            var poolDestinationList = new List<InteractionDestinationBaseData>()
            {
                new InteractionDestinationBaseData()
                {
                    PositionX = 238.125f,
                    PositionY = 0f,
                    PositionZ = 242.375f,

                    RotationX = 0,
                    RotationY = 180,
                    RotationZ = 0,

                    PositionVariance = 0,
                    Patience = 0
                },

                //new InteractionDestinationBaseData()
                //{
                //    PositionX = 245.125f,
                //    PositionY = 0f,
                //    PositionZ = 242.375f,

                //    RotationX = 0,
                //    RotationY = 180,
                //    RotationZ = 0,

                //    PositionVariance = 0,
                //    Patience = 5
                //}
            };

            CreateWorldInteractable(Enums.InteractableType.Object, 7, region.Id, poolDestinationList);

            var regionSize = GetRegionSize(region.Id);

            for (int i = 3; i < objectsInWorld; i++)
            {
                CreateWorldObject(Random.Range(16, 21), region.Id, new Vector3(Random.Range(0, (regionSize - 1)), 0f, Random.Range(0, (regionSize - 1))), new Vector3(0, Random.Range(0, 359), 0));
            }
        }
    }

    static public void CreateWorldInteractable(Enums.InteractableType type, int interactableId, int regionId, List<InteractionDestinationBaseData> interactionDestinationList)
    {
        var worldInteractable = new WorldInteractableBaseData();

        int id = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].Id + 1) : 1;

        worldInteractable.Id = id;

        worldInteractable.Type = (int)type;

        worldInteractable.InteractableId = interactableId;

        worldInteractableList.Add(worldInteractable);

        for (int index = 0; index < baseTasks; index++)
        {
            CreateTask(worldInteractable, 0, index, regionId, interactionDestinationList);
        }
    }

    static public void CreateTask(WorldInteractableBaseData worldInteractable, int objectiveId, int taskIndex, int regionId, List<InteractionDestinationBaseData> interactionDestinationList)
    {
        var task = new TaskBaseData();

        int id = taskList.Count > 0 ? (taskList[taskList.Count - 1].Id + 1) : 1;

        task.Id = id;

        task.WorldInteractableId = worldInteractable.Id;
        task.ObjectiveId = objectiveId;

        task.Default = (taskIndex == 0);

        task.Index = taskIndex;
        
        task.Name = "Just a task" + (objectiveId == 0 ? "" : " with an objective " + task.ObjectiveId);

        task.EditorNotes = "I belong to Interactable " + worldInteractable.Id + ". This is definitely a test";

        taskList.Add(task);

        CreateInteraction(task, true, 0, 0, regionId, interactionDestinationList);
        CreateInteraction(task, false, 0, (5 * TimeManager.secondsInHour) - 1, regionId, interactionDestinationList);
        CreateInteraction(task, false, 9 * TimeManager.secondsInHour, (16 * TimeManager.secondsInHour) - 1, regionId, interactionDestinationList);
    }

    static public void CreateInteraction(TaskBaseData task, bool isDefault, int startTime, int endTime, int regionId, List<InteractionDestinationBaseData> interactionDestinationList)
    {
        var interaction = new InteractionBaseData();

        int id = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].Id + 1) : 1;
        
        interaction.Id = id;

        interaction.TaskId = task.Id;

        interaction.Default = isDefault;

        interaction.StartTime = startTime;
        interaction.EndTime = endTime;

        interaction.ArrivalType = interactionDestinationList.Count > 1 ? (int)Enums.ArrivalType.Backtrace : (int)Enums.ArrivalType.Stay;

        interaction.TriggerAutomatically = false;
        interaction.BeNearDestination = true;
        interaction.FaceInteractable = true;
        interaction.FaceControllable = false;
        interaction.HideInteractionIndicator = false;

        interaction.InteractionRange = 5;

        interaction.DelayMethod = (int)Enums.DelayMethod.Instant;
        interaction.DelayDuration = 3f;
        interaction.HideDelayIndicator = false;

        interaction.CancelDelayOnInput = true;
        interaction.CancelDelayOnMovement = false;
        interaction.CancelDelayOnHit = false;

        interaction.EditorNotes = "These are public interaction notes";

        interactionList.Add(interaction);

        for(int i = 0; i < interactionDestinationList.Count; i++)
        {
            var interactionDestination = interactionDestinationList[i];
            CreateInteractionDestination(interaction, regionId, (i == 0), interactionDestination);
        }

        CreateOutcome(interaction, regionId, Enums.OutcomeType.Positive);
    }

    static public void CreateInteractionDestination(InteractionBaseData interaction, int regionId, bool isDefault, InteractionDestinationBaseData interactionDestinationSource)
    {
        var interactionDestination = new InteractionDestinationBaseData();

        int id = interactionDestinationList.Count > 0 ? (interactionDestinationList[interactionDestinationList.Count - 1].Id + 1) : 1;

        interactionDestination.Id = id;

        interactionDestination.InteractionId = interaction.Id;

        interactionDestination.Default = true;

        interactionDestination.PositionX = interactionDestinationSource.PositionX;
        interactionDestination.PositionY = interactionDestinationSource.PositionY;
        interactionDestination.PositionZ = interactionDestinationSource.PositionZ;

        interactionDestination.PositionVariance = interactionDestinationSource.PositionVariance;

        interactionDestination.RegionId = regionId;

        interactionDestination.TerrainId = GetTerrain(interactionDestination.RegionId, interactionDestination.PositionX, interactionDestination.PositionZ);
        interactionDestination.TerrainTileId = GetTerrainTile(interactionDestination.TerrainId, interactionDestination.PositionX, interactionDestination.PositionZ);

        interactionDestination.ChangeRotation = interactionDestinationSource.ChangeRotation;

        interactionDestination.RotationX = interactionDestinationSource.RotationX;
        interactionDestination.RotationY = interactionDestinationSource.RotationY;
        interactionDestination.RotationZ = interactionDestinationSource.RotationZ;

        interactionDestination.Animation = interactionDestinationSource.Animation;
        interactionDestination.Patience = interactionDestinationSource.Patience;

        interactionDestinationList.Add(interactionDestination);
    }

    static public void CreateOutcome(InteractionBaseData interaction, int regionId, Enums.OutcomeType type)
    {
        var outcome = new OutcomeBaseData();

        int id = outcomeList.Count > 0 ? (outcomeList[outcomeList.Count - 1].Id + 1) : 1;

        outcome.Id = id;
        
        outcome.InteractionId = interaction.Id;

        outcome.Type = (int)type;

        outcome.CompleteTask = true;
        outcome.ResetObjective = false;

        outcome.CancelScenarioType = (int)Enums.CancelScenarioType.Cancel;
        outcome.CancelScenarioOnInteraction = false;
        outcome.CancelScenarioOnInput = true;
        outcome.CancelScenarioOnRange = false;
        outcome.CancelScenarioOnHit = false;

        outcome.EditorNotes = "Requirements" + (type == Enums.OutcomeType.Positive ? " passed" : " failed");

        outcomeList.Add(outcome);

        for (int i = 0; i < scenesPerOutcome; i++)
        {
            CreateScene(outcome, regionId, i);
        }
    }

    static private void CreateScene(OutcomeBaseData outcome, int regionId, int index)
    {
        var scene = new SceneBaseData();

        int id = sceneList.Count > 0 ? (sceneList[sceneList.Count - 1].Id + 1) : 1;

        scene.Id = id;

        scene.OutcomeId = outcome.Id;
        scene.RegionId = regionId;

        scene.Index = index;

        scene.Name = "Scene " + (index + 1);

        scene.FreezeTime = true;
        scene.AutoContinue = false;
        scene.SetActorsInstantly = false;

        scene.SceneDuration = 2f;
        scene.ShotDuration = 10;
        
        scene.EditorNotes = "This is a scene which belongs to outcome " + outcome.Id;
        scene.GameNotes = "";

        var shotStartCameraPosition = new Vector3(230f, 7.5f, 247.5f);
        var shotStartCameraRotation = new Vector3(30, 60, 0);

        CreateSceneShot(scene, Enums.SceneShotType.Start, shotStartCameraPosition, shotStartCameraRotation, 1);

        var shotEndCameraPosition = new Vector3(242.5f, 7.5f, 250f);
        var shotEndCameraRotation = new Vector3(30, 330, 0);

        //Create an actor based on the interactee
        var interaction = interactionList.Where(x => x.Id == outcome.InteractionId).FirstOrDefault();
        var task = taskList.Where(x => x.Id == interaction.TaskId).FirstOrDefault();
        var worldInteractable = worldInteractableList.Where(x => x.Id == task.WorldInteractableId).FirstOrDefault();

        CreateSceneActor(scene, worldInteractable);
        
        //Create an actor based on the interactor
        var region = regionList.Where(x => x.Id == scene.RegionId).FirstOrDefault();
        var phase = phaseList.Where(x => x.Id == region.PhaseId).FirstOrDefault();

        if(phase != null)
        {
            var chapter = chapterList.Where(x => x.Id == phase.ChapterId).FirstOrDefault();
            var chapterWorldInteractable = worldInteractableList.Where(x => x.ChapterId == chapter.Id).FirstOrDefault();

            CreateSceneActor(scene, chapterWorldInteractable);
        }
        
        CreateSceneProp(scene);

        CreateSceneShot(scene, Enums.SceneShotType.End, shotEndCameraPosition, shotEndCameraRotation, 0);

        sceneList.Add(scene);
    }

    static private void CreateSceneActor(SceneBaseData scene, WorldInteractableBaseData worldInteractable)
    {
        var outcome = outcomeList.Where(x => x.Id == scene.OutcomeId).FirstOrDefault();
        var interactable = interactableList.Where(x => x.Id == worldInteractable.InteractableId).FirstOrDefault();

        var sceneActor = new SceneActorBaseData();

        int id = sceneActorList.Count > 0 ? (sceneActorList[sceneActorList.Count - 1].Id + 1) : 1;

        sceneActor.Id = id;

        sceneActor.SceneId = scene.Id;
        sceneActor.WorldInteractableId = worldInteractable.Id;
        
        sceneActor.ChangePosition = true;
        sceneActor.FreezePosition = false;

        sceneActor.SpeechMethod = (int)Enums.SpeechMethod.Speak;
        sceneActor.SpeechText = "Hello, my name is " + interactable.Name  + ". I belong to scene " + scene.Id;
        sceneActor.ShowTextBox = true;

        sceneActor.PositionX = 238.5f;
        sceneActor.PositionY = 0f;
        sceneActor.PositionZ = 238.5f;

        sceneActor.TerrainId = GetTerrain(scene.RegionId, sceneActor.PositionX, sceneActor.PositionZ);
        sceneActor.TerrainTileId = GetTerrainTile(sceneActor.TerrainId, sceneActor.PositionX, sceneActor.PositionZ);

        sceneActor.ChangeRotation = true;
        sceneActor.FaceTarget = true;

        sceneActor.RotationX = 0;
        sceneActor.RotationY = 0;
        sceneActor.RotationZ = 0;

        sceneActorList.Add(sceneActor);
    }

    static private void CreateSceneProp(SceneBaseData scene)
    {
        var sceneProp = new ScenePropBaseData();

        int id = scenePropList.Count > 0 ? (scenePropList[scenePropList.Count - 1].Id + 1) : 1;

        sceneProp.Id = id;

        sceneProp.SceneId = scene.Id;
        sceneProp.ModelId = 2;

        sceneProp.PositionX = 238.5f;
        sceneProp.PositionY = 0;
        sceneProp.PositionZ = 240.5f;

        sceneProp.TerrainId = GetTerrain(scene.RegionId, sceneProp.PositionX, sceneProp.PositionZ);
        sceneProp.TerrainTileId = GetTerrainTile(sceneProp.TerrainId, sceneProp.PositionX, sceneProp.PositionZ);

        sceneProp.RotationX = 0;
        sceneProp.RotationY = 0;
        sceneProp.RotationZ = 0;

        sceneProp.Scale = 1;

        scenePropList.Add(sceneProp);
    }

    static private void CreateSceneShot(SceneBaseData scene, Enums.SceneShotType shotType, Vector3 cameraPosition, Vector3 cameraRotation, int cameraFilterId)
    {
        var sceneShot = new SceneShotBaseData();

        int id = sceneShotList.Count > 0 ? (sceneShotList[sceneShotList.Count - 1].Id + 1) : 1;

        sceneShot.Id = id;

        sceneShot.SceneId = scene.Id;

        sceneShot.Type = (int)shotType;

        sceneShot.ChangePosition = true;

        sceneShot.PositionX = cameraPosition.x;
        sceneShot.PositionY = cameraPosition.y;
        sceneShot.PositionZ = cameraPosition.z;

        sceneShot.ChangeRotation = true;

        sceneShot.RotationX = (int)cameraRotation.x;
        sceneShot.RotationY = (int)cameraRotation.y;
        sceneShot.RotationZ = (int)cameraRotation.z;

        sceneShot.CameraFilterId = cameraFilterId;
        
        sceneShotList.Add(sceneShot);
    }

    static public void CreateWorldObject(int modelId, int regionId, Vector3 position, Vector3 rotation)
    {
        var worldObject = new WorldObjectBaseData();

        int id = worldObjectList.Count > 0 ? (worldObjectList[worldObjectList.Count - 1].Id + 1) : 1;

        worldObject.Id = id;

        worldObject.ModelId = modelId;
        worldObject.RegionId = regionId;
        
        worldObject.PositionX = position.x;
        worldObject.PositionY = position.y;
        worldObject.PositionZ = position.z;

        worldObject.TerrainId = GetTerrain(worldObject.RegionId, worldObject.PositionX, worldObject.PositionZ);
        worldObject.TerrainTileId = GetTerrainTile(worldObject.TerrainId, worldObject.PositionX, worldObject.PositionZ);
        
        worldObject.RotationX = (int)rotation.x;
        worldObject.RotationY = (int)rotation.y;
        worldObject.RotationZ = (int)rotation.z;

        worldObject.Scale = 1;

        worldObjectList.Add(worldObject);
    }

    static public void LoadChapters(int projectId)
    {
        for (int i = 0; i < chapters; i++)
        {
            var chapter = new ChapterBaseData();

            int chapterId = (i + 1);

            chapter.Id = chapterId;

            chapter.ProjectId = projectId;

            chapter.Index = i;

            chapter.Name = "Chapter " + chapterId + " Name";

            chapter.TimeSpeed = 240;

            chapter.EditorNotes = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";
            
            chapterList.Add(chapter);
        }
    }

    static public void LoadChapterWorldInteractables()
    {
        foreach (ChapterBaseData chapter in chapterList)
        {
            for (int i = 0; i < chapterWorldInteractableInChapter; i++)
            {
                var worldInteractable = new WorldInteractableBaseData();

                int id = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].Id + 1) : 1;

                worldInteractable.Id = id;

                worldInteractable.ChapterId = chapter.Id;

                var interactableId = 1;
                worldInteractable.InteractableId = interactableId;

                worldInteractable.Type = (int)Enums.InteractableType.Controllable;

                worldInteractableList.Add(worldInteractable);
            }
        }
    }

    static public void LoadChapterInteractables()
    {
        foreach(ChapterBaseData chapter in chapterList)
        {
            List<int> randomInteractables = new List<int>();

            var interactableIdList = worldInteractableList.Where(x => x.ChapterId == chapter.Id).Select(x => x.InteractableId).Distinct().ToList();
            interactableList.Where(x => !interactableIdList.Contains(x.Id)).Distinct().ToList().ForEach(x => randomInteractables.Add(x.Id));

            for (int i = 0; i < chapterInteractablesInChapter; i++)
            {
                var chapterInteractable = new ChapterInteractableBaseData();

                int id = chapterInteractableList.Count > 0 ? (chapterInteractableList[chapterInteractableList.Count - 1].Id + 1) : 1;

                chapterInteractable.Id = id;

                chapterInteractable.ChapterId = chapter.Id;

                int randomInteractable = Random.Range(0, randomInteractables.Count);

                chapterInteractable.InteractableId = randomInteractables[randomInteractable];

                randomInteractables.RemoveAt(randomInteractable);

                chapterInteractableList.Add(chapterInteractable);
            }
        }
    }

    static public void LoadChapterRegions()
    {
        foreach (ChapterBaseData chapter in chapterList)
        {
            List<int> randomRegions = new List<int>();

            regionList.ForEach(x => randomRegions.Add(x.Id));

            //int randomRegionAmount = Random.Range(1, regionList.Count + 1);

            for (int i = 0; i < /*randomRegionAmount*/1; i++)
            {
                var chapterRegion = new ChapterRegionBaseData();

                int id = chapterRegionList.Count > 0 ? (chapterRegionList[chapterRegionList.Count - 1].Id + 1) : 1;

                chapterRegion.Id = id;

                chapterRegion.ChapterId = chapter.Id;

                int randomRegion = Random.Range(0, randomRegions.Count);

                chapterRegion.RegionId = randomRegions[randomRegion];

                randomRegions.RemoveAt(randomRegion);

                chapterRegionList.Add(chapterRegion);
            }
        }
    }

    static public void LoadPhases()
    {
        foreach (ChapterBaseData chapter in chapterList)
        {
            for (int i = 0; i < phasesInChapter; i++)
            {
                var phase = new PhaseBaseData();

                int id = phaseList.Count > 0 ? (phaseList[phaseList.Count - 1].Id + 1) : 1;

                phase.Id = id;
                phase.Index = i;

                phase.ChapterId = chapter.Id;

                phase.Name = "Phase " + (i + 1) + " Name";

                foreach (ChapterRegionBaseData chapterRegion in chapterRegionList.Where(x => x.ChapterId == phase.ChapterId).Distinct().ToList())
                {
                    var regionSource = regionList.Where(x => x.Id == chapterRegion.RegionId).FirstOrDefault();

                    var region = new RegionBaseData();

                    int regionId = regionList.Count > 0 ? (regionList[regionList.Count - 1].Id + 1) : 1;

                    region.Id = regionId;
                    region.PhaseId = phase.Id;
                    region.ChapterRegionId = chapterRegion.Id;

                    region.TileSetId = regionSource.TileSetId;

                    region.Name = regionSource.Name;
                    region.RegionSize = regionSource.RegionSize;
                    region.TerrainSize = regionSource.TerrainSize;

                    regionList.Add(region);
                    
                    var terrainSourceList = terrainList.Where(x => x.RegionId == regionSource.Id).OrderBy(x => x.Index).Distinct().ToList();

                    foreach (TerrainBaseData terrainSource in terrainSourceList)
                    {
                        var terrain = new TerrainBaseData();

                        int terrainId = terrainList.Count > 0 ? (terrainList[terrainList.Count - 1].Id + 1) : 1;

                        terrain.Id = terrainId;
                        terrain.RegionId = region.Id;

                        terrain.Index = terrainSource.Index;

                        terrain.IconId = terrainSource.IconId;
                        terrain.Name = terrainSource.Name;

                        var atmosphereSourceList = atmosphereList.Where(x => x.TerrainId == terrainSource.Id).OrderByDescending(x => x.Default).ThenBy(x => x.StartTime).ToList();

                        foreach (AtmosphereBaseData atmosphereSource in atmosphereSourceList)
                        {
                            CreateAtmosphere(terrain, atmosphereSource.Default, atmosphereSource.StartTime, atmosphereSource.EndTime);
                        }

                        var terrainTileSourceList = terrainTileList.Where(x => x.TerrainId == terrainSource.Id).OrderBy(x => x.Index).Distinct().ToList();

                        foreach (TerrainTileBaseData terrainTileSource in terrainTileSourceList)
                        {
                            var terrainTile = new TerrainTileBaseData();

                            int terrainTileId = terrainTileList.Count > 0 ? (terrainTileList[terrainTileList.Count - 1].Id + 1) : 1;

                            terrainTile.Id = terrainTileId;
                            terrainTile.TerrainId = terrain.Id;

                            terrainTile.Index = terrainTileSource.Index;

                            terrainTile.TileId = terrainTileSource.TileId;

                            terrainTileList.Add(terrainTile);
                        }

                        terrainList.Add(terrain);
                    }

                    //Get all world interactables belonging to this region                
                    var interactionDestinationSourceList = interactionDestinationList.Where(x => x.RegionId == regionSource.Id).Distinct().ToList();
                    var interactionSourceList = interactionList.Where(x => interactionDestinationSourceList.Select(y => y.InteractionId).Contains(x.Id)).Distinct().ToList();
                    var taskSourceList = taskList.Where(x => interactionSourceList.Select(y => y.TaskId).Contains(x.Id)).Distinct().ToList();

                    var worldInteractableSourceList = worldInteractableList.Where(x => taskSourceList.Select(y => y.WorldInteractableId).Contains(x.Id)).Distinct().ToList();

                    foreach (WorldInteractableBaseData worldInteractableSource in worldInteractableSourceList)
                    {
                        var worldInteractable = new WorldInteractableBaseData();

                        int worldInteractableId = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].Id + 1) : 1;

                        worldInteractable.Id = worldInteractableId;
                        
                        worldInteractable.InteractableId = worldInteractableSource.InteractableId;

                        worldInteractable.Type = worldInteractableSource.Type;

                        worldInteractableList.Add(worldInteractable);

                        foreach (TaskBaseData taskSource in taskSourceList.Where(x => x.WorldInteractableId == worldInteractableSource.Id).ToList())
                        {
                            var task = new TaskBaseData();

                            int taskId = taskList.Count > 0 ? (taskList[taskList.Count - 1].Id + 1) : 1;

                            task.Id = taskId;
                            task.Index = taskSource.Index;

                            task.WorldInteractableId = worldInteractable.Id;
                            task.ObjectiveId = taskSource.ObjectiveId;

                            task.Name = "Just a task" + (taskSource.ObjectiveId == 0 ? "" : " with an objective " + task.ObjectiveId);

                            taskList.Add(task);

                            foreach (InteractionBaseData interactionSource in interactionSourceList.Where(x => x.TaskId == taskSource.Id).ToList())
                            {
                                var interaction = new InteractionBaseData();

                                int interactionId = interactionList.Count > 0 ? (interactionList[interactionList.Count - 1].Id + 1) : 1;

                                interaction.Id = interactionId;
                                interaction.TaskId = task.Id;

                                interaction.Default = interactionSource.Default;

                                interaction.StartTime = interactionSource.StartTime;
                                interaction.EndTime = interactionSource.EndTime;

                                interaction.ArrivalType = interactionSource.ArrivalType;

                                interaction.TriggerAutomatically = interactionSource.TriggerAutomatically;
                                interaction.BeNearDestination = interactionSource.BeNearDestination;
                                interaction.FaceInteractable = interactionSource.FaceInteractable;
                                interaction.FaceControllable = interactionSource.FaceControllable;
                                interaction.HideInteractionIndicator = interactionSource.HideInteractionIndicator;

                                interaction.InteractionRange = interactionSource.InteractionRange;

                                interaction.DelayMethod = interactionSource.DelayMethod;
                                interaction.DelayDuration = interactionSource.DelayDuration;
                                interaction.HideDelayIndicator = interactionSource.HideDelayIndicator;

                                interaction.CancelDelayOnInput = interactionSource.CancelDelayOnInput;
                                interaction.CancelDelayOnMovement = interactionSource.CancelDelayOnMovement;
                                interaction.CancelDelayOnHit = interactionSource.CancelDelayOnHit;

                                interaction.EditorNotes = interactionSource.EditorNotes;
                                interaction.GameNotes = interactionSource.GameNotes;

                                interactionList.Add(interaction);

                                foreach (InteractionDestinationBaseData interactionDestinationSource in interactionDestinationSourceList.Where(x => x.InteractionId == interactionSource.Id).ToList())
                                {
                                    var interactionDestination = new InteractionDestinationBaseData();

                                    int interactionDestinationId = interactionDestinationList.Count > 0 ? (interactionDestinationList[interactionDestinationList.Count - 1].Id + 1) : 1;

                                    interactionDestination.Id = interactionDestinationId;

                                    interactionDestination.InteractionId = interaction.Id;

                                    interactionDestination.PositionX = interactionDestinationSource.PositionX;
                                    interactionDestination.PositionY = interactionDestinationSource.PositionY;
                                    interactionDestination.PositionZ = interactionDestinationSource.PositionZ;

                                    interactionDestination.PositionVariance = interactionDestinationSource.PositionVariance;

                                    interactionDestination.RegionId = region.Id;

                                    interactionDestination.TerrainId = GetTerrain(interactionDestination.RegionId, interactionDestination.PositionX, interactionDestination.PositionZ);
                                    interactionDestination.TerrainTileId = GetTerrainTile(interactionDestination.TerrainId, interactionDestination.PositionX, interactionDestination.PositionZ);

                                    interactionDestination.ChangeRotation = interactionDestinationSource.ChangeRotation;

                                    interactionDestination.RotationX = interactionDestinationSource.RotationX;
                                    interactionDestination.RotationY = interactionDestinationSource.RotationY;
                                    interactionDestination.RotationZ = interactionDestinationSource.RotationZ;

                                    interactionDestination.Animation = interactionDestinationSource.Animation;
                                    interactionDestination.Patience = interactionDestinationSource.Patience;

                                    interactionDestinationList.Add(interactionDestination);
                                }

                                var outcomeSourceList = outcomeList.Where(x => x.InteractionId == interactionSource.Id).OrderBy(x => x.Type).Distinct().ToList();

                                foreach (OutcomeBaseData outcomeSource in outcomeSourceList)
                                {
                                    var outcome = new OutcomeBaseData();

                                    int outcomeId = outcomeList.Count > 0 ? (outcomeList[outcomeList.Count - 1].Id + 1) : 1;

                                    outcome.Id = outcomeId;
                                    
                                    outcome.InteractionId = interaction.Id;

                                    outcome.Type = outcomeSource.Type;

                                    outcome.CompleteTask = outcomeSource.CompleteTask;
                                    outcome.ResetObjective = outcomeSource.ResetObjective;

                                    outcome.CancelScenarioType = outcomeSource.CancelScenarioType;
                                    outcome.CancelScenarioOnInteraction = outcomeSource.CancelScenarioOnInteraction;
                                    outcome.CancelScenarioOnInput = outcomeSource.CancelScenarioOnInput;
                                    outcome.CancelScenarioOnRange = outcomeSource.CancelScenarioOnRange;
                                    outcome.CancelScenarioOnHit = outcomeSource.CancelScenarioOnHit;

                                    outcome.EditorNotes = outcomeSource.EditorNotes;
                                    outcome.GameNotes = outcomeSource.GameNotes;

                                    outcomeList.Add(outcome);

                                    var sceneSourceList = sceneList.Where(x => x.OutcomeId == outcomeSource.Id).OrderBy(x => x.Index).Distinct().ToList();

                                    foreach(SceneBaseData sceneSource in sceneSourceList)
                                    {
                                        var scene = new SceneBaseData();

                                        int sceneId = sceneList.Count > 0 ? (sceneList[sceneList.Count - 1].Id + 1) : 1;

                                        scene.Id = sceneId;

                                        scene.OutcomeId = outcome.Id;
                                        scene.RegionId = region.Id;

                                        scene.Index = sceneSource.Index;

                                        scene.Name = sceneSource.Name;

                                        scene.FreezeTime = sceneSource.FreezeTime;
                                        scene.AutoContinue = sceneSource.AutoContinue;
                                        scene.SetActorsInstantly = sceneSource.SetActorsInstantly;

                                        scene.SceneDuration = sceneSource.SceneDuration;
                                        scene.ShotDuration = sceneSource.ShotDuration;
                                        
                                        scene.EditorNotes = sceneSource.EditorNotes;
                                        scene.GameNotes = sceneSource.GameNotes;

                                        sceneList.Add(scene);

                                        var sceneShotSourceList = sceneShotList.Where(x => x.SceneId == sceneSource.Id).Distinct().ToList();

                                        foreach(SceneShotBaseData sceneShotSource in sceneShotSourceList)
                                        {
                                            var sceneShot = new SceneShotBaseData();

                                            int sceneShotId = sceneShotList.Count > 0 ? (sceneShotList[sceneShotList.Count - 1].Id + 1) : 1;

                                            sceneShot.Id = sceneShotId;

                                            sceneShot.SceneId = scene.Id;

                                            sceneShot.Type = sceneShotSource.Type;

                                            sceneShot.ChangePosition = sceneShotSource.ChangePosition;

                                            sceneShot.PositionX = sceneShotSource.PositionX;
                                            sceneShot.PositionY = sceneShotSource.PositionY;
                                            sceneShot.PositionZ = sceneShotSource.PositionZ;

                                            sceneShot.PositionTargetSceneActorId = sceneShotSource.PositionTargetSceneActorId;

                                            sceneShot.ChangeRotation = sceneShotSource.ChangeRotation;

                                            sceneShot.RotationX = sceneShotSource.RotationX;
                                            sceneShot.RotationY = sceneShotSource.RotationY;
                                            sceneShot.RotationZ = sceneShotSource.RotationZ;

                                            sceneShot.RotationTargetSceneActorId = sceneShotSource.RotationTargetSceneActorId;

                                            sceneShot.CameraFilterId = sceneShotSource.CameraFilterId;

                                            sceneShotList.Add(sceneShot);
                                        }

                                        var sceneActorSourceList = sceneActorList.Where(x => x.SceneId == sceneSource.Id).Distinct().ToList();

                                        foreach(SceneActorBaseData sceneActorSource in sceneActorSourceList)
                                        {
                                            var sceneActor = new SceneActorBaseData();

                                            int sceneActorId = sceneActorList.Count > 0 ? (sceneActorList[sceneActorList.Count - 1].Id + 1) : 1;

                                            sceneActor.Id = sceneActorId;

                                            sceneActor.SceneId = scene.Id;

                                            //This assumes that the scene actor is always and only the interaction's world interactable
                                            sceneActor.WorldInteractableId = worldInteractable.Id; 

                                            sceneActor.ChangePosition = sceneActorSource.ChangePosition;
                                            sceneActor.FreezePosition = sceneActorSource.FreezePosition;

                                            sceneActor.SpeechMethod = sceneActorSource.SpeechMethod;
                                            sceneActor.SpeechText = sceneActorSource.SpeechText;
                                            sceneActor.ShowTextBox = sceneActorSource.ShowTextBox;

                                            sceneActor.TargetSceneActorId = sceneActorSource.TargetSceneActorId;

                                            sceneActor.PositionX = sceneActorSource.PositionX;
                                            sceneActor.PositionY = sceneActorSource.PositionY;
                                            sceneActor.PositionZ = sceneActorSource.PositionZ;

                                            sceneActor.TerrainId = GetTerrain(scene.RegionId, sceneActor.PositionX, sceneActor.PositionZ);
                                            sceneActor.TerrainTileId = GetTerrainTile(sceneActor.TerrainId, sceneActor.PositionX, sceneActor.PositionZ);

                                            sceneActor.ChangeRotation = sceneActorSource.ChangeRotation;
                                            sceneActor.FaceTarget = sceneActorSource.FaceTarget;

                                            sceneActor.RotationX = sceneActorSource.RotationX;
                                            sceneActor.RotationY = sceneActorSource.RotationY;
                                            sceneActor.RotationZ = sceneActorSource.RotationZ;

                                            sceneActorList.Add(sceneActor);
                                        }

                                        var scenePropSourceList = scenePropList.Where(x => x.SceneId == sceneSource.Id).Distinct().ToList();

                                        foreach(ScenePropBaseData scenePropSource in scenePropSourceList)
                                        {
                                            var sceneProp = new ScenePropBaseData();

                                            int scenePropId = scenePropList.Count > 0 ? (scenePropList[scenePropList.Count - 1].Id + 1) : 1;

                                            sceneProp.Id = scenePropId;

                                            sceneProp.SceneId = scene.Id;
                                            sceneProp.ModelId = scenePropSource.ModelId;

                                            sceneProp.PositionX = scenePropSource.PositionX;
                                            sceneProp.PositionY = scenePropSource.PositionY;
                                            sceneProp.PositionZ = scenePropSource.PositionZ;

                                            sceneProp.TerrainId = GetTerrain(scene.RegionId, sceneProp.PositionX, sceneProp.PositionZ);
                                            sceneProp.TerrainTileId = GetTerrainTile(sceneProp.TerrainId, sceneProp.PositionX, sceneProp.PositionZ);

                                            sceneProp.RotationX = scenePropSource.RotationX;
                                            sceneProp.RotationY = scenePropSource.RotationY;
                                            sceneProp.RotationZ = scenePropSource.RotationZ;

                                            sceneProp.Scale = scenePropSource.Scale;

                                            scenePropList.Add(sceneProp);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var worldObjectSourceList = worldObjectList.Where(x => x.RegionId == regionSource.Id).Distinct().ToList();

                    foreach (WorldObjectBaseData worldObjectSource in worldObjectSourceList)
                    {
                        var worldObject = new WorldObjectBaseData();

                        int worldObjectId = worldObjectList.Count > 0 ? (worldObjectList[worldObjectList.Count - 1].Id + 1) : 1;

                        worldObject.Id = worldObjectId;
                        worldObject.RegionId = region.Id;

                        worldObject.PositionX = worldObjectSource.PositionX;
                        worldObject.PositionY = worldObjectSource.PositionY;
                        worldObject.PositionZ = worldObjectSource.PositionZ;

                        worldObject.TerrainId = GetTerrain(worldObject.RegionId, worldObject.PositionX, worldObject.PositionZ);
                        worldObject.TerrainTileId = GetTerrainTile(worldObject.TerrainId, worldObject.PositionX, worldObject.PositionZ);

                        worldObject.RotationX = worldObjectSource.RotationX;
                        worldObject.RotationY = worldObjectSource.RotationY;
                        worldObject.RotationZ = worldObjectSource.RotationZ;

                        worldObject.Scale = worldObjectSource.Scale;

                        worldObject.ModelId = worldObjectSource.ModelId;

                        worldObjectList.Add(worldObject);
                    }
                }
                
                phase.DefaultRegionId = regionList.Where(x => x.PhaseId == phase.Id).First().Id;

                phase.DefaultPositionX = 238.125f;
                phase.DefaultPositionY = 0.1f;
                phase.DefaultPositionZ = 245.5f;

                phase.DefaultRotationX = 0;
                phase.DefaultRotationY = 0;
                phase.DefaultRotationZ = 0;

                phase.DefaultTime = 7 * TimeManager.secondsInHour;

                phase.EditorNotes = "I belong to Chapter " + chapter.Id + ". This is definitely a test";
                phase.GameNotes = "This is a description of the phase which belongs to Chapter " + chapter.Id + ".";
                
                phaseList.Add(phase);
            }
        }
    }

    static public void LoadQuests()
    {
        foreach (PhaseBaseData phase in phaseList)
        {
            for (int i = 0; i < questsInPhase; i++)
            {
                var quest = new QuestBaseData();

                int id = questList.Count > 0 ? (questList[questList.Count - 1].Id + 1) : 1;

                quest.Id = id;
                quest.Index = i;

                quest.PhaseId = phase.Id;
                quest.Name = "Quest " + (i + 1) + " Name";
                quest.EditorNotes = "I belong to Phase " + phase.Id + ". This is definitely a test";
                
                questList.Add(quest);
            }
        }
    }

    static public void LoadPhaseWorldInteractables()
    {
        foreach (ChapterBaseData chapter in chapterList)
        {
            foreach (PhaseBaseData phase in phaseList.Where(x => x.ChapterId == chapter.Id).Distinct().ToList())
            {
                var chapterInteractables = chapterInteractableList.Where(x => x.ChapterId == chapter.Id).Distinct().ToList();
                var questIds = questList.Where(x => x.PhaseId == phase.Id).Select(x => x.Id).Distinct().ToList();

                for (int i = 0; i < chapterInteractables.Count; i++)
                {
                    var worldInteractable = new WorldInteractableBaseData();

                    int id = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].Id + 1) : 1;

                    worldInteractable.Id = id;
                    
                    var chapterInteractable = chapterInteractables[i];

                    worldInteractable.ChapterInteractableId = chapterInteractable.Id;
                    worldInteractable.PhaseId = phase.Id;
                    
                    int randomQuestId = Random.Range(0, questIds.Count);

                    worldInteractable.QuestId = questIds[randomQuestId];

                    questIds.RemoveAt(randomQuestId);

                    worldInteractable.InteractableId = chapterInteractable.InteractableId;

                    worldInteractable.Type = (int)Enums.InteractableType.Agent;

                    worldInteractableList.Add(worldInteractable);
                }
            }
        }
    }

    static public void LoadObjectives()
    {
        foreach (QuestBaseData quest in questList)
        {
            for (int i = 0; i < objectivesInQuest; i++)
            {
                var objective = new ObjectiveBaseData();

                int id = objectiveList.Count > 0 ? (objectiveList[objectiveList.Count - 1].Id + 1) : 1;

                objective.Id = id;
                objective.Index = i;

                objective.QuestId = quest.Id;
                objective.Name = "Objective " + (i + 1) + " Name";
                objective.EditorNotes = "I belong to Quest " + quest.Id + ". This is definitely a test";
                
                objectiveList.Add(objective);
            }
        }
    }
    
    static public void LoadObjectiveWorldInteractables()
    {
        foreach (ObjectiveBaseData objective in objectiveList)
        {
            List<int> randomInteractables = new List<int>();

            interactableList.ForEach(x => randomInteractables.Add(x.Id));

            for (int i = 0; i < interactablesInObjective; i++)
            {
                var worldInteractable = new WorldInteractableBaseData();

                int id = worldInteractableList.Count > 0 ? (worldInteractableList[worldInteractableList.Count - 1].Id + 1) : 1;

                worldInteractable.Id = id;

                worldInteractable.ObjectiveId = objective.Id;

                int randomInteractable = Random.Range(0, randomInteractables.Count);
                worldInteractable.InteractableId = randomInteractables[randomInteractable];

                worldInteractable.Type = (int)Enums.InteractableType.Object;

                worldInteractableList.Add(worldInteractable);
            }
        }
    }

    static public void LoadTasks()
    {
        foreach (ObjectiveBaseData objective in objectiveList)
        {
            var questWorldInteractables = worldInteractableList.Where(x => x.QuestId == objective.QuestId).Distinct().ToList();
            var objectiveWorldInteractables = worldInteractableList.Where(x => x.ObjectiveId == objective.Id).Distinct().ToList();
            
            var worldInteractables = questWorldInteractables.Concat(objectiveWorldInteractables);

            var phaseId = phaseList.Where(x => questList.Where(y => y.Id == objective.QuestId).Select(y => y.PhaseId).Contains(x.Id)).Select(x => x.Id).FirstOrDefault();
            var regions = regionList.Where(x => x.PhaseId == phaseId).Distinct().ToList();
            
            foreach (WorldInteractableBaseData worldInteractable in worldInteractables)
            {
                for (int index = 0; index < baseTasks; index++)
                {
                    var randomRegion = regions[Random.Range(0, regions.Count)];

                    var regionSize = GetRegionSize(randomRegion.Id);

                    var randomPosition = new Vector3(Random.Range(0, (regionSize - 1)), 0, Random.Range(0, (regionSize - 1)));
                    var rotation = new Vector3(0, 180, 0);

                    var randomDestinationList = new List<InteractionDestinationBaseData>()
                    {
                        new InteractionDestinationBaseData()
                        {
                            PositionX = randomPosition.x,
                            PositionY = randomPosition.y,
                            PositionZ = randomPosition.z,

                            RotationX = 0,
                            RotationY = 180,
                            RotationZ = 0,

                            PositionVariance = 0
                        }
                    };

                    CreateTask(worldInteractable, objective.Id, index, randomRegion.Id, randomDestinationList);
                }
            }
        }
    }
    
    static private float GetRegionSize(int regionId)
    {
        var region = regionList.Where(x => x.Id == regionId).FirstOrDefault();
        var tileSet = tileSetList.Where(x => x.Id == region.TileSetId).FirstOrDefault();
        var terrains = terrainList.Where(x => x.RegionId == region.Id).Distinct().ToList();

        var regionSize = region.RegionSize * region.TerrainSize * tileSet.TileSize;

        return regionSize;
    }

    static private int GetTerrain(int regionId, float posX, float posZ)
    {
        var region = regionList.Where(x => x.Id == regionId).FirstOrDefault();
        var tileSet = tileSetList.Where(x => x.Id == region.TileSetId).FirstOrDefault();
        var terrains = terrainList.Where(x => x.RegionId == region.Id).Distinct().ToList();

        var terrainSize = region.TerrainSize * tileSet.TileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posZ / terrainSize));

        var terrainIndex = (region.RegionSize * terrainCoordinates.y) + terrainCoordinates.x;

        var terrainId = terrains.Where(x => x.Index == terrainIndex).Select(x => x.Id).FirstOrDefault();

        return terrainId;
    }

    static private int GetTerrainTile(int terrainId, float posX, float posZ)
    {
        var terrain = terrainList.Where(x => x.Id == terrainId).FirstOrDefault();
        var region = regionList.Where(x => x.Id == terrain.RegionId).FirstOrDefault();
        var tileSet = tileSetList.Where(x => x.Id == region.TileSetId).FirstOrDefault();
        
        var terrainSize = region.TerrainSize * tileSet.TileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posZ / terrainSize));

        var terrainTiles = terrainTileList.Where(x => x.TerrainId == terrainId).Distinct().ToList();

        var terrainPosition = new Vector2(terrainCoordinates.x * terrainSize,
                                          terrainCoordinates.y * terrainSize);

        var localPosition = new Vector2(posX - terrainPosition.x,
                                        posZ - terrainPosition.y);

        var tileCoordinates = new Vector2(Mathf.Floor(localPosition.x / tileSet.TileSize),
                                          Mathf.Floor(localPosition.y / tileSet.TileSize));

        var tileIndex = (region.TerrainSize * tileCoordinates.y) + tileCoordinates.x;

        var terrainTileId = terrainTiles.Where(x => x.Index == tileIndex).Select(x => x.Id).FirstOrDefault();

        return terrainTileId;
    }

    static public void LoadSaves(int gameId)
    {
        var save = new SaveBaseData();

        int id = saveList.Count > 0 ? (saveList[saveList.Count - 1].Id + 1) : 1;

        save.Id = id;

        var game = gameList.Where(x => x.Id == gameId).First();
        var project = projectList.Where(x => x.Id == game.ProjectId).First();    
        var firstChapter = chapterList.Where(x => x.ProjectId == project.Id).OrderBy(x => x.Index).First();
        var firstPhase = phaseList.Where(x => x.ChapterId == firstChapter.Id).OrderBy(x => x.Index).First();
        var firstWorldInteractable = worldInteractableList.Where(x => x.ChapterId == firstChapter.Id).First();

        save.GameId = gameId;
        save.RegionId = firstPhase.DefaultRegionId;
        save.WorldInteractableId = firstWorldInteractable.Id;

        save.PositionX = firstPhase.DefaultPositionX;
        save.PositionY = firstPhase.DefaultPositionY;
        save.PositionZ = firstPhase.DefaultPositionZ;

        save.GameTime = firstPhase.DefaultTime;

        save.SaveTime = System.DateTime.Now;
        //Test
        //playerSave.playedSeconds = 123456;
        //34 hours
        //17 minutes
        //36 seconds
        //----

        CreateInteractableSaves(save);
        CreateStageSaves(save);

        saveList.Add(save);
    }

    static private void CreateInteractableSaves(SaveBaseData save)
    {
        foreach(InteractableBaseData interactable in interactableList)
        {
            var interactableSave = new InteractableSaveBaseData();

            int interactableSaveId = interactableSaveList.Count > 0 ? (interactableSaveList[interactableSaveList.Count - 1].Id + 1) : 1;

            interactableSave.Id = interactableSaveId;

            interactableSave.SaveId = save.Id;
            interactableSave.InteractableId = interactable.Id;

            interactableSaveList.Add(interactableSave);
        }
    }

    static private void CreateStageSaves(SaveBaseData save)
    {
        LoadChapterSaves(save);
        LoadPhaseRegionWorldInteractableTasks(save);
    }

    static public void LoadChapterSaves(SaveBaseData save)
    {
        foreach (ChapterBaseData chapter in chapterList)
        {
            var chapterSave = new ChapterSaveBaseData();

            int chapterSaveId = chapterSaveList.Count > 0 ? (chapterSaveList[chapterSaveList.Count - 1].Id + 1) : 1;

            chapterSave.Id = chapterSaveId;

            chapterSave.SaveId = save.Id;
            chapterSave.ChapterId = chapter.Id;

            LoadPhaseSaves(chapter, save);

            chapterSaveList.Add(chapterSave);
        }
    }

    static public void LoadPhaseSaves(ChapterBaseData chapter, SaveBaseData save)
    {
        foreach(PhaseBaseData phase in phaseList.Where(x => x.ChapterId == chapter.Id))
        {
            var phaseSave = new PhaseSaveBaseData();

            int id = phaseSaveList.Count > 0 ? (phaseSaveList[phaseSaveList.Count - 1].Id + 1) : 1;

            phaseSave.Id = id;

            phaseSave.SaveId = save.Id;
            phaseSave.PhaseId = phase.Id;

            LoadQuestSaves(phase, save);

            phaseSaveList.Add(phaseSave);
        }
    }

    static private void LoadPhaseRegionWorldInteractableTasks(SaveBaseData save)
    {
        foreach (PhaseSaveBaseData phaseSave in phaseSaveList)
        {
            var phaseRegionSourceList = regionList.Where(x => x.PhaseId == phaseSave.PhaseId);

            foreach (RegionBaseData phaseRegionSource in phaseRegionSourceList)
            {
                var interactionIdList           = interactionDestinationList.Where(x => x.RegionId == phaseRegionSource.Id).Select(x => x.InteractionId).ToList();
                var taskIdList                  = interactionList.Where(x => interactionIdList.Contains(x.Id)).Select(x => x.TaskId).ToList();
                var worldInteractableIdList     = taskList.Where(x => taskIdList.Contains(x.Id)).Select(x => x.WorldInteractableId).ToList();
                var worldInteractableSourceList = worldInteractableList.Where(x => worldInteractableIdList.Contains(x.Id)).Distinct().ToList();
                
                foreach (WorldInteractableBaseData worldInteractableSource in worldInteractableSourceList)
                {
                    LoadWorldInteractableTaskSaves(worldInteractableSource, save);
                }
            }
        }
    }

    static public void LoadWorldInteractableTaskSaves(WorldInteractableBaseData worldInteractable, SaveBaseData save)
    {
        foreach (TaskBaseData task in taskList.Where(x => x.WorldInteractableId == worldInteractable.Id && x.ObjectiveId == 0))
        {
            var taskSave = new TaskSaveBaseData();

            int id = taskSaveList.Count > 0 ? (taskSaveList[taskSaveList.Count - 1].Id + 1) : 1;

            taskSave.Id = id;

            taskSave.SaveId = save.Id;
            taskSave.WorldInteractableId = worldInteractable.Id;
            taskSave.TaskId = task.Id;

            LoadInteractionSaves(task, save);

            taskSaveList.Add(taskSave);
        }
    }

    static public void LoadQuestSaves(PhaseBaseData phase, SaveBaseData save)
    {
        foreach (QuestBaseData quest in questList.Where(x => x.PhaseId == phase.Id))
        {
            var questSave = new QuestSaveBaseData();

            int id = questSaveList.Count > 0 ? (questSaveList[questSaveList.Count - 1].Id + 1) : 1;

            questSave.Id = id;

            questSave.SaveId = save.Id;
            questSave.QuestId = quest.Id;

            LoadObjectiveSaves(quest, save);

            questSaveList.Add(questSave);
        }
    }

    static public void LoadObjectiveSaves(QuestBaseData quest, SaveBaseData save)
    {
        foreach (ObjectiveBaseData objective in objectiveList.Where(x => x.QuestId == quest.Id))
        {
            var objectiveSave = new ObjectiveSaveBaseData();

            int id = objectiveSaveList.Count > 0 ? (objectiveSaveList[objectiveSaveList.Count - 1].Id + 1) : 1;

            objectiveSave.Id = id;

            objectiveSave.SaveId = save.Id;
            objectiveSave.ObjectiveId = objective.Id;
            
            LoadObjectiveTaskSaves(objective, save);

            objectiveSaveList.Add(objectiveSave);
        }
    }

    static public void LoadObjectiveTaskSaves(ObjectiveBaseData objective, SaveBaseData save)
    {
        foreach (TaskBaseData task in taskList.Where(x => x.ObjectiveId == objective.Id))
        {
            var taskSave = new TaskSaveBaseData();

            int id = taskSaveList.Count > 0 ? (taskSaveList[taskSaveList.Count - 1].Id + 1) : 1;

            taskSave.Id = id;

            taskSave.SaveId = save.Id;
            taskSave.WorldInteractableId = task.WorldInteractableId;
            taskSave.TaskId = task.Id;

            LoadInteractionSaves(task, save);

            taskSaveList.Add(taskSave);
        }
    }
    
    static public void LoadInteractionSaves(TaskBaseData task, SaveBaseData save)
    {
        foreach (InteractionBaseData interaction in interactionList.Where(x => x.TaskId == task.Id))
        {
            var interactionSave = new InteractionSaveBaseData();

            int id = interactionSaveList.Count > 0 ? (interactionSaveList[interactionSaveList.Count - 1].Id + 1) : 1;

            interactionSave.Id = id;

            interactionSave.SaveId = save.Id;
            interactionSave.InteractionId = interaction.Id;

            interactionSaveList.Add(interactionSave);
        }
    }
    
    static private void Query()
    {

    }
}
