using OOPFirstLab.Common;
using OOPFirstLab.GameObjectDescriptors;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OOPFirstLab.GameObjects
{

    public class GameObject<T> : IGameObject where T : IGameObjectDescriptor, new()
    {
        protected readonly T _objectDescriptor = new T();
        protected readonly GameEngine _gameEngine;

        public GameObject(GameEngine gameEngine, bool isMutant, Gender gender)
        {
            // Мутантом может быть только объект описатель которого это подразумевает. Не мутантом может быть любой
            Debug.Assert(_objectDescriptor.CanBeMutant && isMutant || !isMutant);

            // Объект у которого в описателе указано отсутствие пола должен иметь неопределенный пол. Если пол указан то должен быть какой-то определенный
            Debug.Assert(!_objectDescriptor.HasGender && gender == Gender.Unspecified 
                || _objectDescriptor.HasGender && gender != Gender.Unspecified);

            _gameEngine = gameEngine;
            IsMutant = _objectDescriptor.CanBeMutant && isMutant;

            if (_objectDescriptor.HasGender && gender != Gender.Unspecified)
            {
                Gender = gender;
            }
            else
            {
                Gender = Gender.Unspecified;
            }

            InitObject();
        }

        protected void InitObject()
        {
            Health = _objectDescriptor.MaxHealth;
            BreedTimer = _objectDescriptor.MaxBreedTimer;
            MaximizeHealth();
        }

        public GameObjectType Type { get { return _objectDescriptor.Type; } }

        public Position Position { get; set; }

        public Gender Gender { get; protected set; } = Gender.Unspecified;

        public int Health { get; protected set; }

        public bool IsMutant { get; protected set; }

        public virtual bool MakeMove()
        {
            // Если объект может ходить, то он должен двигаться или к паре, или к еде, или просто в случайном направлении
            if (_objectDescriptor.CanMove)
            {
                // Если объект голоден, то бежит к ближайшей пище
                if (WantToEat)
                {
                    // Рвём пару если она есть
                    if (_objectDescriptor.HasGender)
                    {
                        BreakPair();
                    }

                    // делаем шаг в направлении ближайшей еды
                    MoveToNearestFood();
                }
                else if (WantToBreed)
                {
                    // Если нужна пара и её нет, то образовываем её
                    if (_objectDescriptor.HasGender && Pair == null)
                    {
                        CreatePair();
                    }

                    // Если пара есть, то делаем шаг в её направлении. Если нет то просто шагаем в рандомном направлении
                    if (_objectDescriptor.HasGender && Pair != null)
                    {
                        MoveToPair();
                    }
                    else
                    {
                        MoveToRandomDirection();
                    }
                }
                else
                {
                    // Не едим и не размножаемся, значит просто праздно шатаемся туда-сюда
                    MoveToRandomDirection();
                }
            }

            BreedTimer = Math.Max(BreedTimer - 1, 0);
            Health = Math.Max(Health - 1, 0);

            // Едим всё что можем в текущей клетке
            EatFoodAtCurrentPosition();

            // Если подошло время размножения и есть для этого все условия то размножаемся
            if (WantToBreed)
            {
                Breed();
            }

            return _objectDescriptor.MaxHealth > 0
                ? Health > 0    // Объект со здоровьем умирает когда его здоровье 0
                : true;         // Объект без здоровья от голода умереть не может
        }

        public virtual void Die()
        {
            if (_objectDescriptor.HasGender)
            {
                BreakPair();
            }
        }

        /// <summary>
        /// Объект-пара для размножения
        /// </summary>
        protected GameObject<T> Pair { get; set; }

        protected int BreedTimer { get; set; }

        protected bool WantToEat { get { return Health < _objectDescriptor.MaxHealth * 0.7; } }

        protected bool WantToBreed { get { return BreedTimer <= 0; } }

        protected void CreatePair()
        {
            Debug.Assert(_objectDescriptor.HasGender && Gender != Gender.Unspecified);
            if (_objectDescriptor.HasGender && Pair == null && Gender != Gender.Unspecified)
            {
                // Пол требуемого объекта
                Gender pairGender = Gender == Gender.Male
                    ? Gender.Female
                    : Gender.Male;

                // Данная локальная функция по переданному объекту определяет подходит ли он нам как пара
                bool IsGoodPair(IGameObject gameObject)
                {
                    bool result = false;
                    if (gameObject.Type == Type)
                    {
                        result = gameObject is GameObject<T> go
                            && go.Gender == pairGender
                            && !go.WantToEat
                            && go.WantToBreed
                            && go.Pair == null;
                    }

                    return result;
                }

                IGameObject pair = _gameEngine.GetCurrentMap().GetNearestObject(Position, go => IsGoodPair(go));

                if (pair != null && pair is GameObject<T> goodPair)
                {
                    goodPair.Pair = this;
                    Pair = goodPair;
                }
            }
        }

        protected void Breed()
        {
            if (BreedTimer <= 0)
            {
                // можем размножиться если мы двуполые и есть пара в той же клетке или же мы однополые
                bool canBreed = _objectDescriptor.CanBreed && ((_objectDescriptor.HasGender && Pair != null && Position == Pair.Position)
                    || !_objectDescriptor.HasGender);

                if (canBreed)
                {
                    // создадим потомка
                    IGameObject newObject = new GameObjectFactory(_gameEngine).CreateGameObject(Type);
                    // Куда поставим новый объект?
                    bool isNewObjectPlaced = false;
                    if (_objectDescriptor.NewObjectPlacement == NewObjectPlacement.SameCell)
                    {
                        // Просим карту поставить новый объект в нашей клетке
                        isNewObjectPlaced = _gameEngine.GetCurrentMap().MoveGameObjectToPosition(newObject, Position);
                    }
                    else if (_objectDescriptor.NewObjectPlacement == NewObjectPlacement.NeighborFreeCell)
                    {
                        // Просим карту поставить новый объект где-нить рядом с нашей клеткой
                        isNewObjectPlaced = _gameEngine.GetCurrentMap().PlaceGameObjectNearPositionAtFreeCell(newObject, Position);
                    }

                    // Если мы успешно разместили новый объект, то включаем его в список объектов
                    if (isNewObjectPlaced)
                    {
                        _gameEngine.AddGameObjectToProcessing(newObject);
                    }

                    // Сбрасываем таймер размножения
                    BreedTimer = _objectDescriptor.MaxBreedTimer;
                    if (_objectDescriptor.HasGender && Pair != null)
                    {
                        Pair.BreedTimer = _objectDescriptor.MaxBreedTimer;
                    }
                }
            }
        }

        protected virtual void BreakPair()
        {
            Debug.Assert(_objectDescriptor.HasGender);

            if (Pair != null)
            {
                Pair.Pair = null;
                Pair = null;
            }
        }

        protected void MoveToNearestFood()
        {
            // Берем у карты ближайшую цель
            IGameObject food = _gameEngine.GetCurrentMap().GetNearestObject(Position, go => _objectDescriptor.Food.Contains(go.Type));
            if (food != null)
            {
                Position foodPosition = food.Position;
                Position newPosition = Position.GetPosOnWayTo(foodPosition);
                _gameEngine.GetCurrentMap().MoveGameObjectToPosition(this, newPosition);
            }
        }

        protected void EatFoodAtCurrentPosition()
        {
            List<IGameObject> gameObjects = _gameEngine.GetCurrentMap().GetObjectsAtPos(Position);
            
            if (gameObjects != null)
            {
                List<IGameObject> toRemove = new List<IGameObject>();

                foreach (IGameObject gameObject in gameObjects)
                {
                    // Этот объект подходит нам в качестве еды?
                    if (_objectDescriptor.Food.Contains(gameObject.Type))
                    {
                        // Съедаем этот объект
                        toRemove.Add(gameObject);
                        MaximizeHealth();
                        break;
                    }
                }

                foreach (IGameObject gameObject in toRemove)
                {
                    _gameEngine.RemoveGameObject(gameObject);
                }
            }
        }

        protected void MoveToRandomDirection()
        {
            if (Position != null)
            {
                const int numOfDirections = 8;
                MoveDirection md = (MoveDirection)_gameEngine.GetRandom().Next(numOfDirections);
                Position pos = Position.GetNeaghborPosition(md);
                _gameEngine.GetCurrentMap().MoveGameObjectToPosition(this, pos);
            }
        }

        protected void MoveToPair()
        {
            Debug.Assert(_objectDescriptor.HasGender && Pair != null);
            if (_objectDescriptor.HasGender && Pair != null)
            {
                Position newPos = Position.GetPosOnWayTo(Pair.Position);
                _gameEngine.GetCurrentMap().MoveGameObjectToPosition(this, newPos);
            }
        }

        protected void MaximizeHealth()
        {
            Health = (int)((IsMutant ? 1.5 : 1.0) * _objectDescriptor.MaxHealth);
        }
    }
}