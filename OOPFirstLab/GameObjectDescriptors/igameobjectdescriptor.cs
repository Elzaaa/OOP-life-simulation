using OOPFirstLab.Common;
using System.Collections.Generic;

namespace OOPFirstLab.GameObjectDescriptors
{
    /// <summary>
    /// Описание класса игрового объекта
    /// </summary>
    public interface IGameObjectDescriptor
    {
        /// <summary>
        /// Тип этого объекта
        /// </summary>
        GameObjectType Type { get; }

        /// <summary>
        /// Может ли объект мутировать при рождении (пока что это влияет только на количество максимального здоровья)
        /// </summary>
        bool CanBeMutant { get; }

        /// <summary>
        /// Есть ли пол и соответственно нужен ли партнёр для размножения
        /// </summary>
        bool HasGender { get; }

        /// <summary>
        /// Может ли объект данного типа двигаться
        /// </summary>
        bool CanMove { get; }

        /// <summary>
        /// Есть ли здоровье у объекта и его максимальное значение. Соответственно 
        /// если максимальное здоровье больше 0 то объект может умереть от голода
        /// </summary>
        int MaxHealth { get; }

        /// <summary>
        /// Может ли объект размножаться
        /// </summary>
        bool CanBreed { get; }

        /// <summary>
        /// Сколько времени должно пройти между размножениями
        /// </summary>
        int MaxBreedTimer { get; }

        /// <summary>
        /// Список объектов, которые данный объект может съесть
        /// </summary>
        List<GameObjectType> Food { get; }

        /// <summary>
        /// Позиция, в которую будет размещен новый объект при размножении
        /// </summary>
        NewObjectPlacement NewObjectPlacement { get; }
    }
}