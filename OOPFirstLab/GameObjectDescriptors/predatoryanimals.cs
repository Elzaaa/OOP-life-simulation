using OOPFirstLab.Common;
using System.Collections.Generic;

namespace OOPFirstLab.GameObjectDescriptors
{
    public class PredatoryDescriptor1 : IGameObjectDescriptor
    {
        //тигр
        public bool CanBeMutant { get { return true; } }
        public GameObjectType Type { get { return GameObjectType.PredatoryAnimal1; } }

        public bool HasGender { get { return true; } }

        public bool CanMove { get { return true; } }

        public int MaxHealth { get { return 20; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 10; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType> { GameObjectType.HerbivoreAnimal1, GameObjectType.HerbivoreAnimal2, GameObjectType.HerbivoreAnimal3 };

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.SameCell; } }
    }

    public class PredatoryDescriptor2 : IGameObjectDescriptor
    {
        //гепард
        public bool CanBeMutant { get { return true; } }

        public GameObjectType Type { get { return GameObjectType.PredatoryAnimal2; } }

        public bool HasGender { get { return true; } }

        public bool CanMove { get { return true; } }

        public int MaxHealth { get { return 15; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 5; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType> { GameObjectType.HerbivoreAnimal2, GameObjectType.HerbivoreAnimal3 };

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.SameCell; } }
    }

    public class PredatoryDescriptor3 : IGameObjectDescriptor
    {
        //волк
        public bool CanBeMutant { get { return true; } }
        public GameObjectType Type { get { return GameObjectType.PredatoryAnimal3; } }

        public bool HasGender { get { return true; } }

        public bool CanMove { get { return true; } }

        public int MaxHealth { get { return 25; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 20; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType> { GameObjectType.HerbivoreAnimal1, GameObjectType.HerbivoreAnimal2, GameObjectType.HerbivoreAnimal3 };

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.SameCell; } }
    }
}
