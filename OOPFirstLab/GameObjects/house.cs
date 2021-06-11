using OOPFirstLab.Common;
using OOPFirstLab.GameObjectDescriptors;

namespace OOPFirstLab.GameObjects
{
    public class House : GameObject<HouseDescriptor>
    {
        /// <summary>
        /// Примем что в доме может быть 5 фруктов
        /// </summary>
        public static int kMaxFruitCount = 5;

        public House(GameEngine gameEngine) : base(gameEngine, false, Gender.Unspecified)
        {
        }

        public int FruitCount { get; set; }
    }
}
