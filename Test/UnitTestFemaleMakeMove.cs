using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOPFirstLab;
using OOPFirstLab.Common;
using OOPFirstLab.GameObjectDescriptors;
using OOPFirstLab.GameObjects;
using System;
using System.Collections.Generic;

namespace Test
{
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
            Human male = new Human(gameEngine, false, Gender.Male);
            House house = new House(gameEngine);
            GameObject<FruitDescriptor1> fruit = new GameObject<FruitDescriptor1>(gameEngine, false, Gender.Unspecified);

            female.CarryFruit = false;
            female.Home = house;
            male.Home = house;
            female.Pair = male;
            male.Pair = female;

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female, house, fruit, male });
            gameEngine.AddGameObjectToProcessing(fruit);
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.AddGameObjectToProcessing(house);
            gameEngine.AddGameObjectToProcessing(male);

            Position fruitPos = new Position(3, 3);

            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, fruitPos);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(4, 4));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(house, new Position(5, 5));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(male, new Position(5, 5));

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

        [TestMethod]
        public void MakeFemaleMove_FemaleDead()
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
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));

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

        #region Тестирование, основанное на потоках данных

        [TestMethod]
        public void MakeFemaleMove_FemaleHave10Heals_Female9Health()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female)
            {
                Health = 10
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));

            gameEngine.NextStep();

            Assert.AreEqual(female.Health, 9);
        }

        [TestMethod]
        public void MakeFemaleMove_FemaleHave10BreedTimer_Female9BreedTimer()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female)
            {
                BreedTimer = 10
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));

            gameEngine.NextStep();

            Assert.AreEqual(female.BreedTimer, 9);
        }

        #endregion

        #region Разделение на классы эквивалентности

        [TestMethod]
        public void MakeFemaleMoved_PositiveHeal()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Male)
            {
                Health = 10
            };

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

        [TestMethod]
        public void MakeFemaleMoved_NegativeBreeatTimer()
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

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));

            gameEngine.NextStep();

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

        [TestMethod]
        public void MakeFemaleMoved_PositiveBreeatTimer()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female)
            {
                BreedTimer = 10
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));
            gameEngine.NextStep();

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

        #region Анализ граничных условий

        [TestMethod]
        public void MakeFemaleMove_FemaleHave0Heals_FemaleRemove()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female)
            {
                Health = 0
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));

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
        public void MakeFemaleMove_FemaleHaveNegativeHeals_FemaleRemove()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female)
            {
                Health = -1
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));

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
        public void MakeFemaleMove_FemaleHave101Heals_FemaleNotRemove()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female)
            {
                Health = 101
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(5, 5));

            gameEngine.NextStep();

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

        [TestMethod]
        public void FemaleHaveNegativeBreedTimer_FemaleWantToBreed()
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
        public void FemaleHave101BreedTimer_FemaleNotWantToBreed()
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
        public void MakeFemaleMove_ValidPos_FemaleFound()
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

        [TestMethod]
        public void MakeFemaleMove_NewValidPos_FemaleFound()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(0, 0));

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

        #region Классы плохих данных
        [TestMethod]
        public void MakeFemaleMove_InvalidPos_FemaleNotChangePos()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            Position oldPos = female.Position;
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(50, 50));
            Position newPos = female.Position;

            Assert.AreEqual(oldPos, newPos);
        }

        [TestMethod]
        public void MakeFemaleMove_NewInvalidPos_FemaleNotChangePos()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            Human female = new Human(gameEngine, false, Gender.Female);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { female });
            gameEngine.AddGameObjectToProcessing(female);
            Position oldPos = female.Position;
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(female, new Position(-99, -99));
            Position newPos = female.Position;

            Assert.AreEqual(oldPos, newPos);
        }

        #endregion
    }
    #endregion
}
