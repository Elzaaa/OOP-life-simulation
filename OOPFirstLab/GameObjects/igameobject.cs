using OOPFirstLab.Common;

namespace OOPFirstLab.GameObjects
{
    public interface IGameObject
    {
        GameObjectType Type { get; }        // Тип данного игрового объекта

        Position Position { get; set; }     // Позиция на карте

        bool IsMutant { get; }
        /// <summary>
        /// Говорим объекту сделать ход. Если объект жив после хода - 
        /// возвращаем true. Если false значит он умер и его надо убрать с поля.
        /// </summary>
        /// <returns></returns>
        bool MakeMove();

        /// <summary>
        /// Объект может умереть в результате внешнего воздействия (быть съеденным или убитым в результате катаклизма).
        /// Вызовом этой функции сообщаем объекту что он умер.
        /// </summary>
        void Die();
    }
}