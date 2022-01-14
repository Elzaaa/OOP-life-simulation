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
            var newMap = new GameMap(1, 1);
            newMap.SetGameEngine(this);
            return newMap;
        }

        public Random GetRandom()
        {
            ++GetRandomCount;
            return new Random();
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

    //MakeMaleMove
    #region MakeMaleMove
    [TestClass]
    public class MaleMove
    {
     
        #region Структурное базисное тестирование
        [TestMethod]
        public void MakeMaleMove_Goes_To_Fruit_WantToEat()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);

            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);
            male.SetWantToEat();
            Human female = new Human(gameEngine, false, Gender.Female);

            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, female, fruit });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(0, 0));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, new Position(9, 9));

            male.MakeMove();

            Assert.AreEqual(new Position(6,6), male.Position);
        }

        [TestMethod]
        public void MakeMaleMove_NoPairHomeless_CreatePair()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);

            gameEngine = new GameEngine
                    (
                        s_r,
                        gameMap
                    );
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);
            Human female = new Human(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, female });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(0, 0));

            male.SetWantToBreed();
            female.SetWantToBreed();

            male.MakeMove();
            // female.MakeMove();

            Assert.AreEqual(female, male.Pair);
            Assert.AreEqual(female.Pair, male);
        }

        [TestMethod]
        public void MakeMaleMove_NoPairHomeless_CreatePairCreateHome()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);
            Human female = new Human(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, female });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(0, 0));

            male.SetWantToBreed();
            female.SetWantToBreed();

            male.MakeMove();

            Assert.AreEqual(female, male.Pair);
            Assert.AreEqual(female.Pair, male);

            List<IGameObject> objects = gameEngine.GetCurrentMap().GetObjectsAtPos(male.Position);
            bool houseExist = false;
            foreach (IGameObject obj in objects)
            {
                if (obj.Type == GameObjectType.House)
                {
                    Assert.AreEqual(male.Home, obj);
                    if (male.Home == obj)
                    {
                        houseExist = true;
                        break;
                    }
                }
            }
            Assert.IsTrue(houseExist);
        }

        [TestMethod]
        public void MakeMaleMove_WantToEat_EatFruitFromHomeStorage()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);
            House house = new House(gameEngine);
            male.Home = house;

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, house });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(house, new Position(5, 5));

            male.SetWantToEat();
            house.FruitCount = House.kMaxFruitCount;
            male.MakeMove();

            Assert.AreEqual(house.Position, male.Position);
            Assert.AreEqual(House.kMaxFruitCount - 1, house.FruitCount);
        }

        [TestMethod]
        public void MakeMaleMove_WantToEat_MoveToNearestFood()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);
            male.SetWantToEat();

            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);
            GameObject<FruitDescriptor1> nearFruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, nearFruit, fruit });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(nearFruit, new Position(4, 4));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, new Position(9, 9));

            male.MakeMove();

            Assert.AreEqual(new Position(4, 4), male.Position);
        }

        [TestMethod]
        public void MakeMaleMove_EatHuman()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);
            Human male2 = new Human(gameEngine, false, Gender.Male);
            male.SetWantToEat();

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, male2 });
            gameEngine.AddGameObjectToProcessing(male);
            gameEngine.AddGameObjectToProcessing(male2);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male2, new Position(5, 5));

            gameEngine.NextStep();

            bool male2Found = false;
            for (int x = 0; x < 10 && !male2Found; ++x)
            {
                for (int y = 0; y < 10 && !male2Found; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    if (objects != null && objects.Contains(male2))
                    {
                        male2Found = true;
                    }
                }
            }

            Assert.IsTrue(male2Found);
        }

        [TestMethod]
        public void MakeMaleMove_MaleDead()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                Health = 1
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male });
            gameEngine.AddGameObjectToProcessing(male);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

            gameEngine.NextStep();

            for (int x = 0; x < 10; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    Assert.IsTrue(objects == null || objects.Count == 0);
                }
            }
        }

        [TestMethod]
        public void MakeMaleMove_MoveToNearestFood()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);
            male.SetWantToEat();

            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);
            GameObject<FruitDescriptor1> nearFruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, nearFruit, fruit });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(nearFruit, new Position(4, 4));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, new Position(9, 9));

            male.MakeMove();

            Assert.AreEqual(new Position(4, 4), male.Position);
        }

        [TestMethod]
        public void MaleMove_NotWantToBreed_NotWantToEat_MoveToRandomDirection()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male });

            Position origin = new Position(5, 5);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

            male.MakeMove();

            Assert.AreNotEqual(origin, male.Position);
        }
        #endregion


        #region Тестирование, основанное на потоках 
        [TestMethod]
        public void MakeMaleMove_InvalidPos()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                Health = 1
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male });
            gameEngine.AddGameObjectToProcessing(male);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(50, 50));

            gameEngine.NextStep();

            for (int x = 0; x < 10; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    Assert.IsTrue(objects == null || objects.Count == 0);
                }
            }
        }

        [TestMethod]
        public void MakeMaleMove_ValidPos()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male });
            gameEngine.AddGameObjectToProcessing(male);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

            bool maleFound = false;
            for (int x = 0; x < 10 && !maleFound; ++x)
            {
                for (int y = 0; y < 10 && !maleFound; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    if (objects != null && objects.Contains(male))
                    {
                        maleFound = true;
                    }
                }
            }

            Assert.IsTrue(maleFound);
        }


        #endregion

        #region Разделение на классы эквивалентности

        #endregion

        #region Анализ граничных условий

        #endregion

        #region Классы хороших данных

        #endregion

        #region Классы плохих данных

        #endregion
    }
    #endregion

    //MakeFemaleMove
    #region MakeFemaleMove
    [TestClass]
    public class FemaleMove
    {

        #region Структурное базисное тестирование
        [TestMethod]
        public void Female_WantToEat()
        {
            GameEngine gameEngine;
            //GameMap gameMap;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);
            Human female = new Human(gameEngine, false, Gender.Female);

            female.SetWantToEat();

            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, female, fruit });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(0, 0));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, new Position(9, 9));

            female.MakeMove();

            Assert.AreEqual(new Position(6, 6), female.Position);
        }

        [TestMethod]
        public void Female_WantToEat_CarryFruit()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female);
            female.CarryFruit = true;
            female.SetWantToEat();

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
   
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));

            female.MakeMove();

            Assert.IsFalse(female.CarryFruit);
        }

        [TestMethod]
        public void Female_WantToEat_EatFruitFromHomeStorage()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female);
            House house = new House(gameEngine);
            female.Home = house;

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female, house });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(house, new Position(5, 5));

            female.SetWantToEat();
            house.FruitCount = House.kMaxFruitCount;
            female.MakeMove();

            Assert.AreEqual(house.Position, female.Position);
            Assert.AreEqual(House.kMaxFruitCount - 1, house.FruitCount);
        }

        [TestMethod]
        public void MakeMaleMove_WantToEat_MoveToNearestFood()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female);
            female.SetWantToEat();

            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);
            GameObject<FruitDescriptor1> nearFruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female, nearFruit, fruit });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(nearFruit, new Position(4, 4));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, new Position(9, 9));

            female.MakeMove();

            Assert.AreEqual(new Position(4, 4), female.Position);
        }

        [TestMethod]
        public void Female_WantToBreed_MoveToHome()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female);
            Human male = new Human(gameEngine, false, Gender.Male);
            female.Pair = male;
            male.Pair = female;

            House house = new House(gameEngine);
            female.Home = house;
            male.Home = house;

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, female, house });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(0, 0));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(house, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

            female.SetWantToBreed();

            female.MakeMove();

            Assert.AreEqual(new Position(1, 1), female.Position);
        }

        [TestMethod]
        public void Female_WantToBreed_StayAtHome()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female);
            Human male = new Human(gameEngine, false, Gender.Male);
            female.Pair = male;
            male.Pair = female;
            House house = new House(gameEngine);
            female.Home = house;

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, female, house });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(0, 0));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(house, new Position(5, 5));

            female.SetWantToBreed();
            male.SetWantToBreed();

            female.MakeMove();

            Assert.AreEqual(house.Position, female.Position);
        }

        [TestMethod]
        public void Female_CarryFruit_MoveToHome()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female);
            female.CarryFruit = true;
            House house = new House(gameEngine);
            female.Home = house;

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female, house });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(4, 4));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(house, new Position(5, 5));

            house.FruitCount = 0;
            female.MakeMove();

            Assert.AreEqual(house.Position, female.Position);
            Assert.AreEqual(1, house.FruitCount);
            Assert.IsFalse(female.CarryFruit);
        }

        [TestMethod]
        public void Female_CollectFruit()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();


            Human female = new Human(gameEngine, false, Gender.Female);
            female.CarryFruit = true;
            House house = new House(gameEngine);
            female.Home = house;
            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female, house, fruit });
            gameEngine.AddGameObjectToProcessing(fruit);
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.AddGameObjectToProcessing(house);

            Position fruitPos = new Position(3, 3);

            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, fruitPos);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(4, 4));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(house, new Position(5, 5));

            house.FruitCount = 0;

            gameEngine.NextStep();

            Assert.AreEqual(fruitPos, female.Position);
            Assert.IsTrue(female.CarryFruit);

            var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(fruitPos);

            Assert.IsFalse(objects.Contains(fruit));
        }

        [TestMethod]
        public void FemaleMove_NotWantToBreed_NotWantToEat_MoveToRandomDirection()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });

            Position origin = new Position(5, 5);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));

            female.MakeMove();

            Assert.AreNotEqual(origin, female.Position);
        }



        #endregion

        #region Тестирование, основанное на потоках данных
        [TestMethod]
        public void MakeFemaleMove_InvalidPos()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female)
            {
                Health = 1
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(50, 50));

            gameEngine.NextStep();

            for (int x = 0; x < 10; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    Assert.IsTrue(objects == null || objects.Count == 0);
                }
            }
        }

        [TestMethod]
        public void MakeFemaleMove_ValidPos()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));

            bool femaleFound = false;
            for (int x = 0; x < 10 && !femaleFound; ++x)
            {
                for (int y = 0; y < 10 && !femaleFound; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    if (objects != null && objects.Contains(female))
                    {
                        femaleFound = true;
                    }
                }
            }

            Assert.IsTrue(femaleFound);
        }

        #endregion

        #region Разделение на классы эквивалентности

        #endregion

        #region Анализ граничных условий

        #endregion

        #region Классы хороших данных

        #endregion

        #region Классы плохих данных

        #endregion
    }
    #endregion

    //MakeMove()
    #region MakeMove
    [TestClass]
    public class MakeMove
    {

        #region Структурное базисное тестирование
        [TestMethod]
        public void GameObject_Fruit1_Not_MakeMove()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);

            gameEngine.NewGame();

            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { fruit });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, new Position(5, 5));

            fruit.MakeMove();

            Assert.AreEqual(new Position(5, 5), fruit.Position);
        }

        [TestMethod]
        public void GameObject_Fruit1_WantToBreed()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);

            gameEngine.NewGame();

            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { fruit });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, new Position(5, 5));

            fruit.SetWantToBreed();

            fruit.MakeMove();
            int fruitCount = 0;
            for (int x = 0; x < 10; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    if (objects != null)
                    {
                        fruitCount++;
                    }
                }
            }

            bool fruitChild = false;
            if (fruitCount > 1)
            {
                fruitChild = true;
            }

            Assert.IsTrue(fruitChild);
            Assert.AreEqual(new Position(5, 5), fruit.Position);
        }

        [TestMethod]
        public void GameObject_HerbivoreDescriptor1_WantToEat()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);

            gameEngine.NewGame();

            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);
            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male);
            elk.SetWantToEat();

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk, fruit});
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, new Position(9, 9));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(5, 5));

            elk.MakeMove();

            Assert.AreEqual(new Position(6, 6), elk.Position);
        }

        [TestMethod]
        public void HerbivoreDescriptor1_WantToEat_MoveToNearestFood()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);
            GameObject<FruitDescriptor1> nearFruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);
            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male);
            elk.SetWantToEat();

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk, nearFruit, fruit });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(nearFruit, new Position(4, 4));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, new Position(9, 9));

            elk.MakeMove();

            Assert.AreEqual(new Position(4, 4), elk.Position);
        }

        [TestMethod]
        public void HerbivoreDescriptor1_WantToBreed_CreatePair()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elkMale = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male);
            GameObject<HerbivoreDescriptor1> elkFemale = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elkMale, elkFemale });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elkMale, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elkFemale, new Position(4, 4));

            elkMale.SetWantToBreed();
            elkFemale.SetWantToBreed();
            elkMale.MakeMove();

            Assert.AreEqual(elkFemale, elkMale.Pair);
            Assert.AreEqual(elkMale.Pair, elkFemale);

        }

        [TestMethod]
        public void HerbivoreDescriptor1_WantToBreed_NotCreatePair_MaleElk()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elkMale = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male);
            GameObject<HerbivoreDescriptor1> elkMale2 = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elkMale, elkMale2 });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elkMale, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elkMale2, new Position(4, 4));

            elkMale.SetWantToBreed();
            elkMale2.SetWantToBreed();
            elkMale.MakeMove();

            Assert.AreEqual(elkMale2, elkMale.Pair);
            Assert.AreEqual(elkMale.Pair, elkMale2);

        }

        [TestMethod]
        public void HerbivoreDescriptor1_WantToBreed_MoveToPair()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elkMale = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male);
            GameObject<HerbivoreDescriptor1> elkFemale = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elkMale, elkFemale });

            Position origin = new Position(0, 0);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elkMale,origin);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elkFemale, new Position(9, 9));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elkFemale, new Position(99, 99));

            elkMale.SetWantToBreed();
            elkFemale.SetWantToBreed();
            elkMale.MakeMove();
            elkFemale.MakeMove();

            Assert.AreEqual(elkFemale, elkMale.Pair);
            Assert.AreEqual(elkMale.Pair, elkFemale);

            Assert.AreEqual(new Position(1, 1), elkMale.Position);
            Assert.AreEqual(new Position(8, 8), elkFemale.Position);
            Assert.AreNotEqual(origin, elkFemale.Position);
        }

        [TestMethod]
        public void HerbivoreDescriptor1_EatFoodAtCurrentPosition()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male);
            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk, fruit });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, new Position(5, 5));

            elk.SetWantToEat();
            elk.MakeMove();

            bool FruitFound = false;
            for (int x = 0; x < 10 && !FruitFound; ++x)
            {
                for (int y = 0; y < 10 && !FruitFound; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    if (objects != null)
                    {
                        foreach (var obj in objects)
                        {
                            if (obj.Type == GameObjectType.Fruit1)
                            {
                                FruitFound = true;
                                break;
                            }
                        }
                    }
                }
            }

            Assert.IsFalse(FruitFound);
        }

        public void HerbivoreDescriptor1_Elk_Dead()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male)
            {
                Health = 1
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk });
            gameEngine.AddGameObjectToProcessing(elk);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(5, 5));

            gameEngine.NextStep();

            for (int x = 0; x < 10; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    Assert.IsTrue(objects == null || objects.Count == 0);
                }
            }
        }

        #endregion

        #region Тестирование, основанное на потоках 

        [TestMethod]
        public void MakeMove_HerbivoreDescriptor1_Elk_InvalidPos()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male)
            {
                Health = 1
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk });
            gameEngine.AddGameObjectToProcessing(elk);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(50, 50));

            gameEngine.NextStep();

            for (int x = 0; x < 10; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    Assert.IsTrue(objects == null || objects.Count == 0);
                }
            }
        }

        [TestMethod]
        public void MakeMove_HerbivoreDescriptor1_Elk_ValidPos()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk });
            gameEngine.AddGameObjectToProcessing(elk);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(5, 5));

            bool elkFound = false;
            for (int x = 0; x < 10 && !elkFound; ++x)
            {
                for (int y = 0; y < 10 && !elkFound; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    if (objects != null && objects.Contains(elk))
                    {
                        elkFound = true;
                    }
                }
            }

            Assert.IsTrue(elkFound);
        }

        #endregion 

        #region Разделение на классы эквивалентности

        #endregion

        #region Анализ граничных условий

        #endregion

        #region Классы хороших данных

        #endregion

        #region Классы плохих данных

        #endregion

    }
    #endregion

    //DrawCurrentState()
    #region DrawCurrentState
    [TestClass]
    public class DrawCurrentState
    {
        IGameEngine gameEngine;
        IGameMap gameMap;

        #region Структурное базисное тестирование
        [TestMethod]
        public void TestMethod1()
        {

        }
        #endregion

        #region Тестирование, основанное на потоках данных
        #endregion

        #region Разделение на классы эквивалентности

        #endregion

        #region Анализ граничных условий

        #endregion

        #region Классы хороших данных

        #endregion

        #region Классы плохих данных

        #endregion
    }
    #endregion

    [TestClass]
    public class MocGameEngineTest
    {
        [TestMethod]
        public void TestMocGameEngine()
        {
            MocGameEngine gameEngine = new MocGameEngine();

            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);
            Human female = new Human(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male, female });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(0, 0));

            male.SetWantToBreed();
            female.SetWantToBreed();

            male.MakeMove();

            Assert.AreEqual(-1, gameEngine.IsZasuhaCount);
            Assert.AreEqual(-1, gameEngine.AddGameObjectToProcessingCount);
            Assert.AreEqual(-1, gameEngine.GenerateRandomObjectsCount);
            Assert.AreEqual(-1, gameEngine.GetCurrentMapCount);
            Assert.AreEqual(-1, gameEngine.GetRandomCount);
            Assert.AreEqual(-1, gameEngine.NewGameCount);
            Assert.AreEqual(-1, gameEngine.NextStepCount);
            Assert.AreEqual(-1, gameEngine.RemoveGameObjectCount);
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

            Assert.AreEqual(-1, map.HeightCount);
            Assert.AreEqual(-1, map.WidthCount);
            Assert.AreEqual(-1, map.GetObjectsAtPosCount);
            Assert.AreEqual(-1, map.ClearCount);
            Assert.AreEqual(-1, map.InitCount);
            Assert.AreEqual(-1, map.RemoveGameObjectCount);
            Assert.AreEqual(-1, map.SetGameEngineCount);
            Assert.AreEqual(-1, map.GetNearestCellWithoutHousesCount);
            Assert.AreEqual(-1, map.GetNearestObjectCount);
        }
    }
}
