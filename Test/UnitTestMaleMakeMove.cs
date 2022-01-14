using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOPFirstLab;
using OOPFirstLab.Common;
using OOPFirstLab.GameObjectDescriptors;
using OOPFirstLab.GameObjects;
using System;
using System.Collections.Generic;
namespace Test
{
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

            Assert.AreEqual(new Position(6, 6), male.Position);
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
        public void MakeMaleMove_MaleHave10Heals_Male9Health()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                Health = 10
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male });
            gameEngine.AddGameObjectToProcessing(male);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

            gameEngine.NextStep();

            Assert.AreEqual(male.Health, 9);
        }

        [TestMethod]
        public void MakeMaleMove_MaleHave10BreedTimer_Male9BreedTimer()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                BreedTimer = 10
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male });
            gameEngine.AddGameObjectToProcessing(male);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

            gameEngine.NextStep();

            Assert.AreEqual(male.BreedTimer, 9);
        }
       
        #endregion

        #region Разделение на классы эквивалентности

        [TestMethod]
        public void MakeMaleMoved_PositiveHeal()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                Health = 10
            };

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

        [TestMethod]
        public void MakeMaleMoved_NegativeHeal()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                Health = -1
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male });
            gameEngine.AddGameObjectToProcessing(male);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

            gameEngine.NextStep();

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

            Assert.IsFalse(maleFound);
        }

        [TestMethod]
        public void MakeMaleMoved_NegativeBreeatTimer()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                BreedTimer = -1
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male });
            gameEngine.AddGameObjectToProcessing(male);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

            gameEngine.NextStep();

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

        [TestMethod]
        public void MakeMaleMoved_PositiveBreeatTimer()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                BreedTimer = 10
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male });
            gameEngine.AddGameObjectToProcessing(male);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));
            gameEngine.NextStep();

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

        #region Анализ граничных условий

        [TestMethod]
        public void MakeMaleMove_MaleHave0Heals_MaleRemove()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                Health = 0
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
        public void MakeMaleMove_MaleHaveNegativeHeals_MaleRemove()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                Health = -1
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
        public void MakeMaleMove_MaleHave101Heals_MaleNotRemove()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                Health = 101
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male });
            gameEngine.AddGameObjectToProcessing(male);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

            gameEngine.NextStep();

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

        [TestMethod]
        public void MaleHaveNegativeBreedTimer_MaleWantToBreed()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female)
            {
                BreedTimer = -1
            };

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                BreedTimer = -1
            };

            House house = new House(gameEngine);
            female.Home = house;

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female, house, male });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(4, 4));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(house, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

            female.MakeMove();
            male.MakeMove();

            Assert.AreEqual(female.Pair, male);
            Assert.AreEqual(male.Pair, female);
        }

        [TestMethod]
        public void MaleHave101BreedTimer_MaleNotWantToBreed()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female)
            {
                BreedTimer = 101
            };

            Human male = new Human(gameEngine, false, Gender.Male)
            {
                BreedTimer = 101
            };

            House house = new House(gameEngine);
            female.Home = house;

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female, house, male });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(4, 4));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(house, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

            female.MakeMove();
            male.MakeMove();

            Assert.AreNotEqual(female.Pair, male);
            Assert.AreNotEqual(male.Pair, female);
        }

        #endregion

        #region Классы хороших данных

        [TestMethod]
        public void MakeMaleMove_ValidPos_MaleFound()
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

            bool femaleFound = false;
            for (int x = 0; x < 10 && !femaleFound; ++x)
            {
                for (int y = 0; y < 10 && !femaleFound; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    if (objects != null && objects.Contains(male))
                    {
                        femaleFound = true;
                    }
                }
            }

            Assert.IsTrue(femaleFound);
        }

        [TestMethod]
        public void MakeMaleMove_NewValidPos_MaleFound()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human male = new Human(gameEngine, false, Gender.Male);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { male });
            gameEngine.AddGameObjectToProcessing(male);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(1, 1));

            bool femaleFound = false;
            for (int x = 0; x < 10 && !femaleFound; ++x)
            {
                for (int y = 0; y < 10 && !femaleFound; ++y)
                {
                    var objects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                    if (objects != null && objects.Contains(male))
                    {
                        femaleFound = true;
                    }
                }
            }

            Assert.IsTrue(femaleFound);
        }


        #endregion

        #region Классы плохих данных

        [TestMethod]
        public void MakeMaleMove_InvalidPos_MaleNotFound()
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
        public void MakeMaleMove_NewInvalidPos_MaleNotFound()
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
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(-88, 50));

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
    }
    #endregion
}
