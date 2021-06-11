using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOPFirstLab
{
    public interface IAnimalDescriptor
    {
        List<GameObjectType> GetFoodTypes();
    }

    public abstract class AbstractAnimal<T> : GameObject where T : IAnimalDescriptor
    {
        private static int kMaxHealth = 30;

        private static int kTimeBetweenBreeds = 10;
        protected readonly GameEngine _gameEngine;
        private static Random s_r = new Random();

        protected AbstractAnimal(GameEngine gameEngine, Gender g, bool isMutant)
        {
            _gameEngine = gameEngine;

            IsMutant = isMutant;
            Health = GetMaxHealth();
            Gender = g;

            BreedTimer = kTimeBetweenBreeds;
        }

        public int Health { get; protected set; }

        public int BreedTimer { get; protected set; }

        public Gender Gender { get; private set; }

        public AbstractAnimal<T> Pair { get; set; }

        public override void MakeMove()
        {
            GameMap gameMap = _gameEngine.GetCurrentMap();

            // флаг для обозначения того что в конце хода мы должны уменьшить здоровье
            bool bReduceHealth = true;
            // флаг для обозначения того что в конце хода мы должны уменьшить таймер запрета на размножение
            bool bReduceBreedTimer = true;

            if (WantToEat)
            {
                // Голодный зверь не может размножаться. Рвём пару, если она есть
                BreakPair();

                // Бежим к ближайшему цели которую будем есть
                MoveToNearestTarget();

                // пытаемся съесть всё что можно на текущей клетке
                if (gameMap.EatTargets(GetAnimalDescriptor().GetFoodTypes(), Position))
                {
                    Health = kMaxHealth;
                    // мы поели, значит здоровье отнимать не будем
                    bReduceHealth = false;
                }
            }
            else
            {
                // Если пара уже есть то сразу бежим к ней
                if (Pair != null)
                {
                    MoveToAnimal(Pair);
                }
                // если пары нет то пытаемся образовать пару. В случае успеха бежим к ней.
                else if (CreatePair() && Pair != null)
                {
                    MoveToAnimal(Pair);
                }
                // просто делаем шаг в случайном направлении
                else
                {
                    MoveToRandomDirection();
                }

                // если добежали до пары, то размножаемся
                if (Pair != null && Pair.Position != null && Pair.Position == Position)
                {
                    Breed();
                    bReduceBreedTimer = false;
                }
            }


            if (bReduceHealth)
            {
                // в конце хода здоровье уже не то...
                Health = Math.Max(0, Health - GetHealthToReduce());
            }

            if (bReduceBreedTimer)
            {
                // Уменьшаем штраф на размножение
                BreedTimer = Math.Max(0, BreedTimer - 1);
            }
        }

        private void Breed()
        {
            // создаем новое животное в своей клетке и разрываем пару
            if (Pair != null && Position != null && Pair.Position != null)
            {
                // 
                GameObject newAnimal = _gameEngine.CreateRandomAnimal(Type);
                _gameEngine.AddAnimalToProcessing(newAnimal);
                _gameEngine.GetCurrentMap().MoveAnimalToPosition(newAnimal, Position);
            }

            // запрещаем размножаться в ближайшее время себе и паре
            BreedTimer = kTimeBetweenBreeds;
            if (Pair != null)
                Pair.BreedTimer = kTimeBetweenBreeds;

            BreakPair();
        }

        private void MoveToRandomDirection()
        {
            if (Position != null)
            {
                const int numOfDirections = 8;
                MoveDirection md = (MoveDirection)s_r.Next(numOfDirections);
                Position pos = Position.GetNeaghborPosition(md);
                _gameEngine.GetCurrentMap().MoveAnimalToPosition(this, pos);
                if (_gameEngine.GetCurrentMap().EatTargets(GetAnimalDescriptor().GetFoodTypes(), Position))
                {
                    Health = kMaxHealth;
                }
            }
        }

        private bool CreatePair()
        {
            if (Pair != null)
                return true;

            if (Position == null)
                return false;

            if (BreedTimer > 0)
                return false;

            Gender targetGender = Gender == Gender.Male ? Gender.Female : Gender.Male;
            IEnumerable<AbstractAnimal<T>> animals = _gameEngine.GetAnimals<T>();
            AbstractAnimal<T> target = null;
            int stepsToNearest = 0;
            foreach (AbstractAnimal<T> animal in animals)
            {
                if (animal != null && animal.Position != null
                    // Выбираем однотипное сытое животное противоположного пола, которое не состоит в паре и может размножаться
                    && animal.Type == Type && !animal.WantToEat && animal.Pair == null && animal.BreedTimer == 0)
                {
                    int steps = GetStepsBetweenPositions(Position, animal.Position);

                    if (target == null || steps < stepsToNearest)
                    {
                        target = animal;
                        stepsToNearest = steps;
                    }
                }
            }

            // Нашли подходящюю кандидатуру, образуем пару с ним
            if (target != null)
            {
                target.Pair = this;
                Pair = target;
                return true;
            }

            // Никого не нашли
            return false;
        }

        private void MoveToAnimal(AbstractAnimal<T> animal)
        {
            if (Position != null && animal != null && animal.Position != null)
            {
                Position pos = GetIntermediatePosition(Position, animal.Position);
                if (pos != null)
                {
                    _gameEngine.GetCurrentMap().MoveAnimalToPosition(this, pos);
                    if (_gameEngine.GetCurrentMap().EatTargets(GetAnimalDescriptor().GetFoodTypes(), Position))
                    {
                        Health = kMaxHealth;
                    }
                }
            }
        }

        private void BreakPair()
        {
            if (Pair != null)
            {
                Pair.Pair = null;
                Pair = null;
            }
        }

        private void MoveToNearestTarget()
        {
            // Запрашиваем список допустимых целей для атаки. Каждый класс предоставляет отдельный список
            var targets = _gameEngine.GetGameObjects(GetAnimalDescriptor().GetFoodTypes());
            GameObject target = GetNearestTarget(targets);
            if (target != null && target.Position != null)
            {
                Position pos = GetIntermediatePosition(Position, target.Position);
                if (pos != null && _gameEngine.GetCurrentMap().MoveAnimalToPosition(this, pos))
                {
                }
            }
        }

        private GameObject GetNearestTarget(IEnumerable<GameObject> targets)
        {
            GameObject result = null;
            int stepsToNearestFruit = 0;
            if (Position != null)
            {
                foreach (GameObject go in targets)
                {
                    if (go != null && go.Position != null)
                    {
                        int steps = GetStepsBetweenPositions(Position, go.Position);
                        if (result == null || steps < stepsToNearestFruit)
                        {
                            result = go;
                            stepsToNearestFruit = steps;
                        }
                    }
                }
            }

            return result;
        }

        private Position GetIntermediatePosition(Position myPos, Position targetPos)
        {
            int xOffset = myPos.X < targetPos.X ? 1 : (myPos.X > targetPos.X ? -1 : 0);
            int yOffset = myPos.Y < targetPos.Y ? 1 : (myPos.Y > targetPos.Y ? -1 : 0);
            return new Position(myPos.X + xOffset, myPos.Y + yOffset);
        }

        private int GetStepsBetweenPositions(Position myPos, Position position)
        {
            return Math.Max(
                Math.Abs(myPos.X - position.X),
                Math.Abs(myPos.Y - position.Y));
        }

        private bool WantToEat
        {
            get
            {
                return Health < kMaxHealth * 0.6;
            }
        }

        private bool WantToBreed
        {
            get
            {
                return Health < kMaxHealth * 0.8;
            }
        }

        private int GetMaxHealth() { return IsMutant ? (int)(kMaxHealth * 1.5) : kMaxHealth; }

        public override bool IsDead
        {
            get { return Health <= 0; }
        }

        public override void Die()
        {
            BreakPair();
        }

        protected abstract T GetAnimalDescriptor();

        protected abstract int GetHealthToReduce();


    }
}
