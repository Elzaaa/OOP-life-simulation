using OOPFirstLab.Common;
using System.Collections.Generic;

namespace OOPFirstLab.GameObjectDescriptors
{
    public class OmnivoreDescriptor1 : IGameObjectDescriptor
    {
        //медведь
        public GameObjectType Type { get { return GameObjectType.OmnivoreAnimal1; } }
        public bool CanBeMutant { get { return true; } }
        public bool HasGender { get { return true; } }

        public bool CanMove { get { return true; } }

        public int MaxHealth { get { return 20; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 10; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType> { GameObjectType.Fruit1, GameObjectType.Fruit2, GameObjectType.Fruit3, GameObjectType.HerbivoreAnimal1, GameObjectType.HerbivoreAnimal2, GameObjectType.HerbivoreAnimal3 };

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.SameCell; } }
    }

    public class OmnivoreDescriptor2 : IGameObjectDescriptor
    {
        //енот
        public bool CanBeMutant { get { return true; } }
        public GameObjectType Type { get { return GameObjectType.OmnivoreAnimal2; } }

        public bool HasGender { get { return true; } }

        public bool CanMove { get { return true; } }

        public int MaxHealth { get { return 15; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 5; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType> { GameObjectType.Fruit1, GameObjectType.Fruit2, GameObjectType.Fruit3, GameObjectType.HerbivoreAnimal3 };

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.SameCell; } }
    }

    public class OmnivoreDescriptor3 : IGameObjectDescriptor
    {
        //пиг
        public bool CanBeMutant { get { return true; } }
        public GameObjectType Type { get { return GameObjectType.OmnivoreAnimal3; } }

        public bool HasGender { get { return true; } }

        public bool CanMove { get { return true; } }

        public int MaxHealth { get { return 25; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 20; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType> { GameObjectType.Fruit1, GameObjectType.Fruit2, GameObjectType.Fruit3, GameObjectType.HerbivoreAnimal3 };

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.SameCell; } }
    }

}
