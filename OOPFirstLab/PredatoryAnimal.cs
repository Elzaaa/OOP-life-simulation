using System.Collections.Generic;

namespace OOPFirstLab
{
    public class PredatoryDesctiptor : IAnimalDescriptor
    {
        private List<GameObjectType> _foodTypes = new List<GameObjectType> { GameObjectType.HerbivoreAnimal };

        public List<GameObjectType> GetFoodTypes()
        {
            return _foodTypes;
        }
    }

    public class PredatoryAnimal : AbstractAnimal<PredatoryDesctiptor>
    {
        private PredatoryDesctiptor _animalDescriptor = new PredatoryDesctiptor();

        public PredatoryAnimal(GameEngine gameEngine, Gender g, bool isMutant) : base(gameEngine, g, isMutant)
        {
            Type = GameObjectType.PredatoryAnimal;
        }

        protected override PredatoryDesctiptor GetAnimalDescriptor()
        {
            return _animalDescriptor;
        }

        protected override int GetHealthToReduce()
        {
            if (_gameEngine.IsZasuha)
                return 2;

            return 1;
        }
    }
}
