using OOPFirstLab.Common;
using OOPFirstLab.GameObjectDescriptors;

namespace OOPFirstLab.GameObjects
{
    public class GameObjectFactory
    {
        GameEngine _gameEngine;

        public GameObjectFactory(GameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public IGameObject CreateGameObject(GameObjectType type)
        {
            IGameObjectDescriptor objectDescriptor = GameObjectDescriptorFactory.CreateGameObjectDescriptor(type);

            // dynamic даёт возможность выбора функции по типу объекта на этапе исполнения. Так что для всех объектов
            // будет вызвана generic функция, а для людей и домов - специализированная, как более подходящая
            return CreateGameObjectImpl((dynamic)objectDescriptor);
        }

        /// <summary>
        /// Общая функция создания всех объектов, за исключением людей и домов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        private GameObject<T> CreateGameObjectImpl<T>(T descriptor) where T : IGameObjectDescriptor, new()
        {
            bool isMutant = descriptor.CanBeMutant ? (_gameEngine.GetRandom().Next(10) < 2) : false;

            Gender g = Gender.Unspecified;
            if (descriptor.HasGender)
            {
                g = (_gameEngine.GetRandom().Next() % 2 == 0) ? Gender.Male : Gender.Female;
            }

            return new GameObject<T>(_gameEngine, isMutant, g);
        }

        /// <summary>
        /// Специализация для людей, так как они представлены отдельным классом
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        private Human CreateGameObjectImpl(HumanDescriptor descriptor)
        {
            bool isMutant = descriptor.CanBeMutant ? _gameEngine.GetRandom().Next(10) < 2 : false;
            Gender g = (_gameEngine.GetRandom().Next() % 2 == 0) ? Gender.Male : Gender.Female;
            return new Human(_gameEngine, isMutant, g);
        }

        /// <summary>
        /// Специализация для домов, так как они представлены отдельным классом
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        private House CreateGameObjectImpl(HouseDescriptor descriptor)
        {
            return new House(_gameEngine);
        }
    }
}
