using OOPFirstLab.Common;
using OOPFirstLab.GameObjectDescriptors;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OOPFirstLab.GameObjects
{
    /// <summary>
    /// Поведение человека значительно отличается от прочих, поэтому выделим его в отдельный класс
    /// </summary>
    public class Human : GameObject<HumanDescriptor>
    {
        public Human(GameEngine gameEngine, bool isMutant, Gender g) : base(gameEngine, isMutant, g)
        {
        }

        /// <summary>
        /// Дом, милый дом
        /// </summary>
        private House Home { get; set; }

        // Женщины могут таскать фрукты домой, это свойство показывает что она несёт фрукт
        private bool CarryFruit { get; set; } = false;

        public override bool MakeMove()
        {
            bool result = false;
            // Мужчины и женщины действуют по-разному
            if (Gender == Gender.Male)
            {
                result = MakeMaleMove();
            }
            else if (Gender == Gender.Female)
            {
                result = MakeFemaleMove();
            }

            return result;
        }

        public override void Die()
        {
            // Оба пола рвут пару.
            BreakPair();

            // Будем также считать что со смертью мужчины дом тоже разрушается (богатых вдов пока не вводим в игру)
            if (Gender == Gender.Male && Home != null)
            {
                _gameEngine.RemoveGameObject(Home);
            }
        }

        protected override void BreakPair()
        {
            Human pair = Pair as Human;
            if (pair != null)
            {
                if (Gender == Gender.Male)
                {
                    // Оставляем вдову без дома
                    pair.Home = null;
                }

                pair.Pair = null;
            }

            Pair = null;
        }

        private bool MakeMaleMove()
        {
            // Голоден? Найдём еду
            if (WantToEat)
            {
                // Если мы дома, и тут есть запасы то просто съедаем один фрукт
                if (Home != null && Home.FruitCount > 0 && Position == Home.Position)
                {
                    EatFruitFromHomeStorage();
                }
                else
                {
                    // Дома еды нет, в руках тоже, придётся бежать до ближайшей
                    MoveToNearestFood();
                    // Едим всё что нашли в текущей клетке
                    EatFoodAtCurrentPosition();
                }
            }
            else
            {
                // Сыт. Надо найти пару
                if (WantToBreed)
                {
                    CreatePair();

                    if (Pair != null)
                    {
                        // Есть пара. Надо дом. Строим, если ещё нету
                        if (Home == null)
                        {
                            // Ищем позицию для дома
                            // Для этого мы должны стоять на клетке где нет дома
                            // Затем оттуда смотрим где ближайший дом. Если в соседней клетке то застраиваемся
                            (bool success, Position nearestFreePosition) = GetNearestPositionWithoutHouse();
                            if (success)
                            {
                                if (nearestFreePosition == Position)
                                {
                                    // Отлично, мы на свободной клетке, проверяем возможность построиться
                                    Position nearestHousePosition = GetNearestHousePosition();
                                    // Двигаемся к позиции для строительства и строим там дом
                                    if (Position.DistanceTo(nearestHousePosition) <= 1)
                                    {
                                        // на позиции для дома
                                        BuildHome();
                                    }
                                    else
                                    {
                                        // Двигаемся к ближайшему дому
                                        Position newPos = Position.GetPosOnWayTo(nearestHousePosition);
                                        _gameEngine.GetCurrentMap().MoveGameObjectToPosition(this, newPos);
                                    }
                                }
                                else
                                {
                                    // Прежде чем построиться мы должны попасть на свободную от зданий клетку, этим и займемся
                                    Position newPos = Position.GetPosOnWayTo(nearestFreePosition);
                                    _gameEngine.GetCurrentMap().MoveGameObjectToPosition(this, newPos);
                                }
                            }
                            else
                            {
                                // Крайне маловероятный вариант что не смогли найти свободную клетку на карте. Делать нечего, просто бегаем
                                MoveToRandomDirection();
                            }
                        }
                        else
                        {
                            // Если с партнером находимся дома, то размножаемся
                            if (Position == Home.Position && Position == Pair.Position)
                            {
                                Breed();
                            }
                            else if (Position != Home.Position)
                            {
                                // Идём домой
                                Position newPos = Position.GetPosOnWayTo(Home.Position);
                                _gameEngine.GetCurrentMap().MoveGameObjectToPosition(this, newPos);
                            }
                            else
                            {
                                // Мы уже сидим дома и ждём партнёра, ничего не делаем
                            }
                        }
                    }
                    else
                    {
                        // Не образовали пару, дома нет, есть не хотим, просто бегаем туда-сюда
                        MoveToRandomDirection();
                    }
                }
                else
                {
                    // Не хотим есть и размножаться. Просто идём домой и лежим на диване. Если дома нет - просто бегаем туда-сюда
                    if (Home != null)
                    {
                        Position newPos = Position.GetPosOnWayTo(Home.Position);
                        _gameEngine.GetCurrentMap().MoveGameObjectToPosition(this, newPos);
                    }
                    else
                    {
                        MoveToRandomDirection();
                    }
                }
            }

            BreedTimer = Math.Max(BreedTimer - 1, 0);
            Health = Math.Max(Health - 1, 0);

            return Health > 0;
        }

        private (bool, Position) GetNearestPositionWithoutHouse()
        {
            return _gameEngine.GetCurrentMap().GetNearestCellWithoutHouses(Position);
        }

        private void BuildHome()
        {
            // Создаем объект "дом" в нашей клетке
            GameObjectFactory factory = new GameObjectFactory(_gameEngine);
            IGameObject house = factory.CreateGameObject(GameObjectType.House);
            Home = house as House;
            if (Home != null)
            {
                if (_gameEngine.GetCurrentMap().MoveGameObjectToPosition(house, Position))
                {
                    _gameEngine.AddGameObjectToProcessing(house);
                    // Сообщаем даме сердца что у неё есть теперь дом
                    ((Human)Pair).Home = Home;
                }
                else
                {
                    Home = null;
                }
            }
        }

        private Position GetNearestHousePosition()
        {
            // Ищем ближайший дом в определенном радиусе. Если такого нет - строимся прям где находимся
            // Считаем что ближайший дом должен быть не более чем в пяти шагах
            const int kMaxDistance = 5;
            IGameObject house = _gameEngine.GetCurrentMap().GetNearestObject(Position, go => go.Type == GameObjectType.House, kMaxDistance);
            return house != null ? house.Position : Position;
        }

        private bool MakeFemaleMove()
        {
            // Голодная? Найдём еду
            if (WantToEat)
            {
                // Если мы дома, и тут есть запасы то просто съедаем один фрукт
                if (Home != null && Home.FruitCount > 0 && Position == Home.Position)
                {
                    EatFruitFromHomeStorage();
                }
                else if (CarryFruit)
                {
                    // Также можно просто съесть фрукт в руках
                    CarryFruit = false;
                    MaximizeHealth();
                }
                else
                {
                    // Дома еды нет, в руках тоже, придётся бежать до ближайшей
                    MoveToNearestFood();
                    // Едим всё что нашли в текущей клетке
                    EatFoodAtCurrentPosition();
                }
            }
            else if (WantToBreed)
            {
                // Если пары нет, то мы приличные девочки и ждём пока нас позовут замуж, сами ничего не делаем, просто скачем туда-сюда
                if (Pair != null && Home != null)
                {
                    // Если с партнером находимся дома, то размножаемся
                    if (Position == Home.Position && Position == Pair.Position)
                    {
                        Breed();
                    }
                    else if (Position != Home.Position)
                    {
                        // Идём домой
                        Position newPos = Position.GetPosOnWayTo(Home.Position);
                        _gameEngine.GetCurrentMap().MoveGameObjectToPosition(this, newPos);
                    }
                    else
                    {
                        // Мы уже сидим дома и ждём партнёра, ничего не делаем
                    }
                }
                else
                {
                    // Нет дома или пары, просто носимся
                    MoveToRandomDirection();
                }
            }
            else if (Home != null)
            {
                // Если дом не полон еды, то некогда прохлаждаться, надо носить фрукты домой
                if (Home.FruitCount < House.kMaxFruitCount)
                {
                    // Если фрукта нет в руках, то бежим к ближайшему фрукту и собираем его
                    if (!CarryFruit)
                    {
                        // Идём к ближайшему фрукту
                        List<GameObjectType> fruits = new List<GameObjectType> { GameObjectType.Fruit1, GameObjectType.Fruit2, GameObjectType.Fruit3 };
                        IGameObject fruit = _gameEngine.GetCurrentMap().GetNearestObject(Position, go => fruits.IndexOf(go.Type) != -1);
                        if (fruit != null)
                        {
                            // Обнаружили фрукт, идём к нему
                            Position newPos = Position.GetPosOnWayTo(fruit.Position);
                            _gameEngine.GetCurrentMap().MoveGameObjectToPosition(this, newPos);
                            // Если пришли к фрукту, то собираем его
                            if (Position == fruit.Position)
                            {
                                _gameEngine.RemoveGameObject(fruit);
                                CarryFruit = true;
                            }
                        }
                    }
                    else
                    {
                        // Если фрукт в руках то бежим домой. Дома скидываем фрукт
                        if (Position != Home.Position)
                        {
                            Position newPos = Position.GetPosOnWayTo(Home.Position);
                            _gameEngine.GetCurrentMap().MoveGameObjectToPosition(this, newPos);
                        }

                        if (Position == Home.Position)
                        {
                            // Всё, фрукт больше не несём
                            CarryFruit = false;
                            // В доме фруктов прибавилось
                            ++Home.FruitCount;
                        }
                    }
                }
                else
                {
                    // дом полон фруктов.
                    // просто идём домой и сидим там
                    if (Position != Home.Position)
                    {
                        Position newPos = Position.GetPosOnWayTo(Home.Position);
                        _gameEngine.GetCurrentMap().MoveGameObjectToPosition(this, newPos);
                    }
                }
            }
            else
            {
                // есть не хотим, размножаться тоже, пары и дома нет, просто бегаем
                MoveToRandomDirection();
            }

            BreedTimer = Math.Max(BreedTimer - 1, 0);
            Health = Math.Max(Health - 1, 0);

            return Health > 0;
        }

        private void EatFruitFromHomeStorage()
        {
            Debug.Assert(Home != null && Home.FruitCount > 0 && Position == Home.Position);
            if (Home != null && Home.FruitCount > 0 && Position == Home.Position)
            {
                --Home.FruitCount;
                MaximizeHealth();
            }
        }
    }
}
