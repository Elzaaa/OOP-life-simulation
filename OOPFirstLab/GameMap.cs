using OOPFirstLab.Common;
using OOPFirstLab.GameObjectDescriptors;
using OOPFirstLab.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OOPFirstLab
{
    public class GameMap
    {
        readonly GameEngine _gameEngine;
        private readonly List<IGameObject>[,] _gameObjects;        

        public GameMap(GameEngine gameEngine, int w, int h)
        {
            _gameEngine = gameEngine;

            // задать ширину и высоту карты
            Width = w;
            Height = h;

            _gameObjects = new List<IGameObject>[Width, Height];
            for (int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height; ++j)
                {
                    _gameObjects[i, j] = new List<IGameObject>();
                }
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public void Init(List<IGameObject> gameObjects)
        {
            // запомнить персонажей и расставить их (рандомно)
            foreach (IGameObject gameObject in gameObjects)
            {
                Position pos = GetRandomFreePosition();
                gameObject.Position = pos;
                _gameObjects[pos.X, pos.Y].Add(gameObject);
            }
        }

        private void ShuffleArray(MoveDirection[] mds)
        {
            // берем два занчения
            List<Tuple<int, MoveDirection>> tmp = new List<Tuple<int, MoveDirection>>(mds.Length);
            //помещаем в список и рандомно берем первое значение, второе из нашего массива
            foreach (MoveDirection md in mds)
            {
                tmp.Add(new Tuple<int, MoveDirection>(_gameEngine.GetRandom().Next(), md));
            }

            // сортируем коллекцию по рандомному элементу
            tmp.Sort((e1, e2) => Comparer<int>.Default.Compare(e1.Item1, e2.Item1));

            // Записываем перемешанные элементы обратно во входной массив
            int i = 0;
            foreach (Tuple<int, MoveDirection> t in tmp)
            {
                mds[i++] = t.Item2;
            }
        }

        public IGameObject GetNearestObject(Position position, Func<IGameObject, bool> func, int maxDistance = -1)
        {
            for (int steps = 0; steps <= maxDistance || maxDistance == -1; ++steps)
            {
                IEnumerable<Position> possiblePositions = position.GetPositionsAtDistance(steps, 0, Width - 1, 0, Height - 1);

                int positions = 0;
                foreach (Position pos in possiblePositions)
                {
                    ++positions;

                    List<IGameObject> gameObjects = GetObjectsAtPos(pos);
                    if (gameObjects != null)
                    {
                        foreach (IGameObject gameObject in gameObjects)
                        {
                            if (func(gameObject))
                            {
                                return gameObject;
                            }
                        }
                    }
                }

                // Если мы отработали 0 позиций, то выходим
                if (positions == 0)
                    return null;
            }

            return null;
        }

        public (bool, Position) GetNearestCellWithoutHouses(Position position)
        {
            for (int steps = 0; ; ++steps)
            {
                IEnumerable<Position> possiblePositions = position.GetPositionsAtDistance(steps, 0, Width - 1, 0, Height - 1);
                int positions = 0;
                foreach (Position pos in possiblePositions)
                {
                    ++positions;
                    List<IGameObject> gameObjects = GetObjectsAtPos(pos);
                    if (!gameObjects.Exists(go => go.Type == GameObjectType.House))
                        return (true, pos);
                }

                // Если мы отработали 0 позиций, то выходим
                if (positions == 0)
                    return (false, new Position());
            }
        }

        public bool PlaceGameObjectNearPositionAtFreeCell(IGameObject @object, Position position)
        {
            // рандомно перебираем направления. Для этого возьмем все возможные направления и перемешаем их
            MoveDirection[] mds = (MoveDirection[])System.Enum.GetValues(typeof(MoveDirection));
            ShuffleArray(mds);

            foreach (MoveDirection md in mds)
            {
                Position newPos = position.GetNeaghborPosition(md);
                if (PosInMapBounds(newPos) && !IsPosOccupied(newPos))
                {
                    // Нашли свободную позицию в пределах карты
                    bool isPlaced = MoveGameObjectToPosition(@object, newPos);
                    if (isPlaced)
                        return true;
                }
            }

            return false;
        }
   
        public bool MoveGameObjectToPosition(IGameObject gameObject, Position position)
        {
            // меняем позицию объекта на заданную, если она в пределах карты
            if (PosInMapBounds(position))
            {
                // Убираем объект со старой позиции
                RemoveGameObject(gameObject);

                // Ставим объект на новую позицию
                gameObject.Position = position;
                List<IGameObject> gameObjectsAtNewPosition = GetObjectsAtPos(position);
                if (gameObjectsAtNewPosition != null)
                    gameObjectsAtNewPosition.Add(gameObject);

                return true;
            }

            return false;
        }

        public void RemoveGameObject(IGameObject gameObject)
        {
            if (gameObject.Position != null)
            {
                List<IGameObject> gameObjects = GetObjectsAtPos(gameObject.Position);
                if (gameObjects != null)
                    gameObjects.Remove(gameObject);
            }
        }

        private Position GetRandomFreePosition()
        {
            while (true)
            {
                int w = _gameEngine.GetRandom().Next(0, Width - 1);
                int h = _gameEngine.GetRandom().Next(0, Height - 1);

                if (!IsPosOccupied(w, h))
                    return new Position(w, h);
            }
        }

        private bool IsPosOccupied(Position pos)
        {
            return IsPosOccupied(pos.X, pos.Y);
        }

        private bool IsPosOccupied(int x, int y)
        {
            return GetObjectsAtPos(x, y).Count > 0;
        }

        public List<IGameObject> GetObjectsAtPos(Position pos)
        {
            return GetObjectsAtPos(pos.X, pos.Y);
        }

        public List<IGameObject> GetObjectsAtPos(int x, int y)
        {
            if (PosInMapBounds(x, y))
                return _gameObjects[x, y];

            return null;
        }

        private bool PosInMapBounds(Position pos)
        {
            return PosInMapBounds(pos.X, pos.Y);
        }

        private bool PosInMapBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}
