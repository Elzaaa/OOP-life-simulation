using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOPFirstLab;
using OOPFirstLab.Common;
using OOPFirstLab.GameObjectDescriptors;
using OOPFirstLab.GameObjects;
using System;
using System.Collections.Generic;

namespace Test
{

    public class MocGameMap : IGameMap
    {
        public int HeightCount { get; private set; } = 0;
        public int WidthCount { get; private set; } = 0;
        public int GetObjectsAtPosCount { get; private set; } = 0;
        public int ClearCount { get; private set; } = 0;
        public int InitCount { get; private set; } = 0;
        public int RemoveGameObjectCount { get; private set; } = 0;
        public int SetGameEngineCount { get; private set; } = 0;
        public int GetNearestCellWithoutHousesCount { get; private set; } = 0;
        public int GetNearestObjectCount { get; private set; } = 0;

        private IGameEngine _gameEngine;

        public MocGameMap(int w, int h) { }

        public int Height { get { ++HeightCount; return 1; } }

        public int Width { get { ++WidthCount; return 1; } }

        public void Clear()
        {
            ++ClearCount;
        }

        public (bool, Position) GetNearestCellWithoutHouses(Position position)
        {
            ++GetNearestCellWithoutHousesCount;
            return (false, new Position(0, 0));
        }

        public IGameObject GetNearestObject(Position position, Func<IGameObject, bool> func, int maxDistance = -1)
        {
            ++GetNearestObjectCount;
            return new House(_gameEngine);
        }

        public List<IGameObject> GetObjectsAtPos(Position pos)
        {
            ++GetObjectsAtPosCount;
            return new List<IGameObject>();
        }

        public List<IGameObject> GetObjectsAtPos(int x, int y)
        {
            return new List<IGameObject>();
        }

        public void Init(List<IGameObject> gameObjects)
        {
            ++InitCount;
        }

        public bool MoveGameObjectToPosition(IGameObject gameObject, Position position)
        {
            return false;
        }

        public bool PlaceGameObjectNearPositionAtFreeCell(IGameObject @object, Position position)
        {
            return false;
        }

        public void RemoveGameObject(IGameObject gameObject)
        {
            ++RemoveGameObjectCount;
        }

        public void SetGameEngine(IGameEngine gameEngine)
        {
            ++SetGameEngineCount;
            _gameEngine = gameEngine;
        }
    }

    public class MocGameEngine : IGameEngine
    {
        public int IsZasuhaCount { get; private set; } = 0;
        public int AddGameObjectToProcessingCount { get; private set; } = 0;
        public int GenerateRandomObjectsCount { get; private set; } = 0;
        public int GetCurrentMapCount { get; private set; } = 0;
        public int GetRandomCount { get; private set; } = 0;
        public int NewGameCount { get; private set; } = 0;
        public int NextStepCount { get; private set; } = 0;
        public int RemoveGameObjectCount { get; private set; } = 0;

        private Random _r;
        private IGameMap _map;

        public MocGameEngine(Random r, IGameMap map)
        {
            _r = r;
            _map = map;
            if (_map != null)
                _map.SetGameEngine(this);
        }


        public bool IsZasuha { get { ++IsZasuhaCount; return false; } }

        public void AddGameObjectToProcessing(IGameObject newObject)
        {
            ++AddGameObjectToProcessingCount;
        }

        public void GenerateRandomObjects()
        {
            ++GenerateRandomObjectsCount;
        }

        public IGameMap GetCurrentMap()
        {
            ++GetCurrentMapCount;
            return _map;
        }

        public Random GetRandom()
        {
            ++GetRandomCount;
            return _r;
        }

        public void NewGame()
        {
            ++NewGameCount;
        }

        public void NextStep()
        {
            ++NextStepCount;
        }

        public void RemoveGameObject(IGameObject newObject)
        {
            ++RemoveGameObjectCount;
        }
    }
    [TestClass]
    public class MocTests
    {
        [TestMethod]
        public void TestMocGameEngine()
        {
            GameMap map = new GameMap(10, 10);
            MocGameEngine gameEngine = new MocGameEngine(new Random(), map);

            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);
            Human female = new Human(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, female });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(0, 0));

            male.SetWantToBreed();
            female.SetWantToBreed();

            male.MakeMove();

            Console.WriteLine("gameEngine.IsZasuhaCount = {0}", gameEngine.IsZasuhaCount); 
            Console.WriteLine("gameEngine.AddGameObjectToProcessingCount = {0}", gameEngine.AddGameObjectToProcessingCount);
            Console.WriteLine("gameEngine.GenerateRandomObjectsCount = {0}", gameEngine.GenerateRandomObjectsCount); 
            Console.WriteLine("gameEngine.GetCurrentMapCount = {0}", gameEngine.GetCurrentMapCount); 
            Console.WriteLine("gameEngine.GetRandomCount = {0}", gameEngine.GetRandomCount); 
            Console.WriteLine("gameEngine.NewGameCount = {0}", gameEngine.NewGameCount); 
            Console.WriteLine("gameEngine.RemoveGameObjectCount = {0}", gameEngine.RemoveGameObjectCount); 
        }

        [TestMethod]
        public void TestMocGameMap()
        {
            Random r = new Random();
            MocGameMap map = new MocGameMap(1, 1);
            GameEngine gameEngine = new GameEngine(r, map);

            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);
            Human female = new Human(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, female });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(0, 0));

            male.SetWantToBreed();
            female.SetWantToBreed();

            male.MakeMove();

            Console.WriteLine("map.HeighCount = {0}", map.HeightCount);
            Console.WriteLine("map.WidthCount = {0}", map.WidthCount);
            Console.WriteLine("map.GetObjectsAtPosCount = {0}", map.GetObjectsAtPosCount);
            Console.WriteLine("map.ClearCount = {0}", map.ClearCount);
            Console.WriteLine("map.InitCount = {0}", map.InitCount);
            Console.WriteLine("map.RemoveGameObjectCount = {0}", map.RemoveGameObjectCount);
            Console.WriteLine("map.SetGameEngineCount = {0}", map.SetGameEngineCount);
            Console.WriteLine("map.GetNearestCellWithoutHousesCount = {0}", map.GetNearestCellWithoutHousesCount);
            Console.WriteLine("map.GetNearestObjectCount = {0}", map.GetNearestObjectCount);
        }
    }
}
