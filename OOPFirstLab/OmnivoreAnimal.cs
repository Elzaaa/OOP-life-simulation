using System.Collections.Generic;

namespace OOPFirstLab
{
    public class OmnivoreDescriptor : IAnimalDescriptor
    {
        private List<GameObjectType> _foodTypes = new List<GameObjectType> { GameObjectType.Fruit, GameObjectType.HerbivoreAnimal };

        public List<GameObjectType> GetFoodTypes()
        {
            return _foodTypes;
        }
    }
    public class OmnivoreAnimal : AbstractAnimal<OmnivoreDescriptor>
    {
        OmnivoreDescriptor _animalDescriptor = new OmnivoreDescriptor();

        protected override int GetHealthToReduce()
        {
            if (_gameEngine.IsZasuha)
                return 3;

            return 1;
        }
        public OmnivoreAnimal(GameEngine gameEngine, Gender g, bool isMutant) : base(gameEngine, g, isMutant)
        {
            Type = GameObjectType.OmnivoreAnimal;
        }

        protected override OmnivoreDescriptor GetAnimalDescriptor()
        {
            return _animalDescriptor;
        }
    }

}
