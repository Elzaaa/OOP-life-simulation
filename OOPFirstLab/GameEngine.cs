﻿using OOPFirstLab.Common;
using OOPFirstLab.GameObjectDescriptors;
using OOPFirstLab.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OOPFirstLab
{
    public interface IGameEngine
    {
        void NewGame();
        void GenerateRandomObjects();
        void NextStep();
        bool IsZasuha { get; }
        IGameMap GetCurrentMap();
        Random GetRandom();
        void AddGameObjectToProcessing(IGameObject newObject);
        void RemoveGameObject(IGameObject newObject);
    }

    public class GameEngine : IGameEngine
    {
        private readonly int rows;
        private readonly int cols;

        private readonly Random _random;

        private IGameMap _gameMap;
        private List<IGameObject> _gameObjects;

        private int m_nCurrentGameObject = 0;

        public GameEngine(Random random, IGameMap gameMap)
        {
            _random = random;
            _gameMap = gameMap;
            _gameMap.SetGameEngine(this);
        }

        public void GenerateRandomObjects()
        {
            _gameObjects = CreateGameObjects(50);
            _gameMap.Init(_gameObjects);
        }

        public void NewGame() 
        {
            IsZasuha = false;
            _gameObjects = new List<IGameObject>();
            _gameMap.Clear();
        }

        private List<IGameObject> CreateGameObjects(int countOfEachType)
        {
            List<IGameObject> result = new List<IGameObject>(13 * countOfEachType);

            GameObjectFactory factory = new GameObjectFactory(this);

            for (int i = 0; i < countOfEachType; ++i)
            {
                // Фрукты
                result.Add(factory.CreateGameObject(GameObjectType.Fruit1));
                result.Add(factory.CreateGameObject(GameObjectType.Fruit2));
                result.Add(factory.CreateGameObject(GameObjectType.Fruit3));

                // Травоядные
                result.Add(factory.CreateGameObject(GameObjectType.HerbivoreAnimal1));
                result.Add(factory.CreateGameObject(GameObjectType.HerbivoreAnimal2));
                result.Add(factory.CreateGameObject(GameObjectType.HerbivoreAnimal3));

                // Всеядные
                result.Add(factory.CreateGameObject(GameObjectType.OmnivoreAnimal1));
                result.Add(factory.CreateGameObject(GameObjectType.OmnivoreAnimal2));
                result.Add(factory.CreateGameObject(GameObjectType.OmnivoreAnimal3));

                // Хищники
                result.Add(factory.CreateGameObject(GameObjectType.PredatoryAnimal1));
                result.Add(factory.CreateGameObject(GameObjectType.PredatoryAnimal2));
                result.Add(factory.CreateGameObject(GameObjectType.PredatoryAnimal3));

                // Люди
                result.Add(factory.CreateGameObject(GameObjectType.Human));
            }

            return result;
        }

        public void NextStep()
        {
            // делаем шаг каждым игровым объектом
            m_nCurrentGameObject = 0;
            while (m_nCurrentGameObject < _gameObjects.Count)
            {
                IGameObject gameObject = _gameObjects[m_nCurrentGameObject];
                if (!gameObject.MakeMove())
                {
                    RemoveGameObjectAtPosition(m_nCurrentGameObject);
                }

                // Переходим к следующему объекту
                ++m_nCurrentGameObject;
            }
        }

        public void RemoveGameObject(IGameObject gameObject)
        {
            int index = _gameObjects.IndexOf(gameObject);
            if (index != -1)
            {
                RemoveGameObjectAtPosition(index);
            }
        }

        private void RemoveGameObjectAtPosition(int position)
        {
            if (position >= 0 && position < _gameObjects.Count)
            {
                IGameObject gameObject = _gameObjects[position];
                gameObject.Die();
                _gameMap.RemoveGameObject(gameObject);
                _gameObjects.RemoveAt(position);

                if (position <= m_nCurrentGameObject)
                {
                    --m_nCurrentGameObject;
                }
            }
        }

        public IGameMap GetCurrentMap()
        {
            return _gameMap;
        }


        public void AddGameObjectToProcessing(IGameObject newObject)
        {
            _gameObjects.Add(newObject);
        }

        public Random GetRandom()
        {
            return _random;
        }

        public bool IsZasuha { get; private set; }
    }
}
