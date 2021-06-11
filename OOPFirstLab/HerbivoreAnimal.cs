using System.Collections.Generic;

namespace OOPFirstLab
{
    public class HerbivoreDescriptor : IAnimalDescriptor
    {
        private List<GameObjectType> _foodTypes = new List<GameObjectType> { GameObjectType.Fruit };

        public List<GameObjectType> GetFoodTypes()
        {
            return _foodTypes;
        }
    }

    public class HerbivoreAnimal : AbstractAnimal<HerbivoreDescriptor>
    {
        HerbivoreDescriptor _animalDescriptor = new HerbivoreDescriptor();

        public HerbivoreAnimal(GameEngine gameEngine, Gender g, bool isMutant) : base(gameEngine, g, isMutant)
        {
            Type = GameObjectType.HerbivoreAnimal;
        }

        protected override int GetHealthToReduce()
        {
            if (_gameEngine.IsZasuha)
                return 4;

            return 1;
        }

        protected override HerbivoreDescriptor GetAnimalDescriptor()
        {
            return _animalDescriptor;
        }
    }
}
