using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOPFirstLab;
using OOPFirstLab.Common;
using OOPFirstLab.GameObjectDescriptors;
using OOPFirstLab.GameObjects;
using System;
using System.Collections.Generic;

namespace Test
{
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

            Assert.IsNull(elkMale.Pair);
            Assert.IsNull(elkMale2.Pair);
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
            gameEngine.AddGameObjectToProcessing(elk);
            gameEngine.AddGameObjectToProcessing(fruit);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(0, 0));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(fruit, new Position(0, 0));

            elk.SetWantToEat();
            gameEngine.NextStep();

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
                                FruitFound = false;
                                break;
                            }
                        }
                    }
                }
            }

            Assert.IsFalse(FruitFound);
        }

        public void HerbivoreDescriptor1_MakeMove_Elk_Dead()
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

        [TestMethod]
        public void HerbivoreDescriptor1Move_NotWantToBreed_NotWantToEat_MoveToRandomDirection()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk });

            Position origin = new Position(5, 5);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(5, 5));

            elk.MakeMove();

            Assert.AreNotEqual(origin, elk.Position);
        }


        #endregion

        #region Тестирование, основанное на потоках 
        [TestMethod]
        public void Elk_MakeMove_Have10Heals_ElkHave9Health()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Female)
            {
                Health = 10
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk });
            gameEngine.AddGameObjectToProcessing(elk);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(5, 5));

            gameEngine.NextStep();

            Assert.AreEqual(elk.Health, 9);
        }

        [TestMethod]
        public void Elk_MakeMove_Have10BreedTimer_Elk9BreedTimer()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Female)
            {
                BreedTimer = 10
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk });
            gameEngine.AddGameObjectToProcessing(elk);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(5, 5));

            gameEngine.NextStep();

            Assert.AreEqual(elk.BreedTimer, 9);
        }

        #endregion 

        #region Разделение на классы эквивалентности

        [TestMethod]
        public void MakeElkMoved_PositiveHeal()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male)
            {
                Health = 10
            };

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

        [TestMethod]
        public void MakeElkMoved_NegativeHeal()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male)
            {
                Health = -1
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk });
            gameEngine.AddGameObjectToProcessing(elk);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(5, 5));

            gameEngine.NextStep();

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

            Assert.IsFalse(elkFound);
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
        public void MakeMove_ElkHave0Heals_ElkRemove()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male)
            {
                Health = 0
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

        [TestMethod]
        public void MakeMove_ElkHaveNegativeHeals_ElkRemove()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male)
            {
                Health = -1
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

        [TestMethod]
        public void MakeMove_ElkHave101Heals_ElkNotRemove()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male)
            {
                Health = 101
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk });
            gameEngine.AddGameObjectToProcessing(elk);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(5, 5));

            gameEngine.NextStep();

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

        [TestMethod]
        public void MakeMove_ElkHaveNegativeBreedTimer_WantToBreed()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elkFemale = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Female)
            {
                BreedTimer = -1
            };

            GameObject<HerbivoreDescriptor1> elkMale = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male)
            {
                BreedTimer = -1
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elkFemale, elkMale });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elkFemale, new Position(4, 4));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elkMale, new Position(5, 5));

            elkFemale.MakeMove();
            elkMale.MakeMove();

            Assert.AreEqual(elkMale.Pair, elkFemale);
            Assert.AreEqual(elkFemale.Pair, elkMale);
        }

        [TestMethod]
        public void ElkHave101BreedTimer_ElkNotWantToBreed()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elkFemale = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Female)
            {
                BreedTimer = 101
            };

            GameObject<HerbivoreDescriptor1> elkMale = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male)
            {
                BreedTimer = 101
            };

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elkFemale, elkMale });
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elkFemale, new Position(4, 4));
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elkMale, new Position(5, 5));

            elkFemale.MakeMove();
            elkMale.MakeMove();

            Assert.AreEqual(elkMale.Pair, elkFemale);
            Assert.AreEqual(elkFemale.Pair, elkMale);
        }

        #endregion

        #region Классы хороших данных

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

        [TestMethod]
        public void MakeMove_HerbivoreDescriptor1_Elk_NewValidPos()
        {
            GameEngine gameEngine;
            Random s_r = new Random();

            IGameMap gameMap = new GameMap(10, 10);
            gameEngine = new GameEngine(s_r, gameMap);
            gameEngine.NewGame();

            GameObject<HerbivoreDescriptor1> elk = new GameObject<HerbivoreDescriptor1>(gameEngine, false, Gender.Male);

            gameEngine.GetCurrentMap().Init(new System.Collections.Generic.List<IGameObject> { elk });
            gameEngine.AddGameObjectToProcessing(elk);
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(6, 6));

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

        #region Классы плохих данных
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
        public void MakeMove_HerbivoreDescriptor1_Elk_NegativeInvalidPos()
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
            gameEngine.GetCurrentMap().MoveGameObjectToPosition(elk, new Position(-50, -50));

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
