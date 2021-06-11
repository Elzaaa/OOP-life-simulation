using OOPFirstLab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPFirstLab.GameObjectDescriptors
{
    public class HumanDescriptor : IGameObjectDescriptor
    {
        public bool CanBeMutant { get { return false; } }

        public GameObjectType Type { get { return GameObjectType.Human; } }

        public bool HasGender { get { return true; } }

        public bool CanMove { get { return true; } }

        public int MaxHealth { get { return 20; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 15; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType> {
            GameObjectType.Fruit1, GameObjectType.Fruit2, GameObjectType.Fruit3,
            GameObjectType.HerbivoreAnimal1, GameObjectType.HerbivoreAnimal2, GameObjectType.HerbivoreAnimal3,
            GameObjectType.OmnivoreAnimal1, GameObjectType.OmnivoreAnimal2, GameObjectType.OmnivoreAnimal3,
            GameObjectType.PredatoryAnimal1, GameObjectType.PredatoryAnimal2, GameObjectType.PredatoryAnimal3 
        };

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.SameCell; } }
    }
}
