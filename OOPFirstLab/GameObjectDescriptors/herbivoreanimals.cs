using OOPFirstLab.Common;
using System.Collections.Generic;

namespace OOPFirstLab.GameObjectDescriptors
{
    public class HerbivoreDescriptor1 : IGameObjectDescriptor
    {
        //лось
        public GameObjectType Type { get { return GameObjectType.HerbivoreAnimal1; } }

        public bool HasGender { get { return true; } }

        public bool CanMove { get { return true; } }

        public bool CanBeMutant { get { return true; } }

        public int MaxHealth { get { return 20; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 10; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType> { GameObjectType.Fruit1, GameObjectType.Fruit2 };

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.SameCell; } }
    }

    public class HerbivoreDescriptor2 : IGameObjectDescriptor
    {
        //олень
        public bool CanBeMutant { get { return true; } }
        public GameObjectType Type { get { return GameObjectType.HerbivoreAnimal2; } }

        public bool HasGender { get { return true; } }

        public bool CanMove { get { return true; } }

        public int MaxHealth { get { return 15; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 5; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType> { GameObjectType.Fruit1, GameObjectType.Fruit2 };

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.SameCell; } }
    }

    public class HerbivoreDescriptor3 : IGameObjectDescriptor
    {
        //кролик
        public bool CanBeMutant { get { return true; } }
        public GameObjectType Type { get { return GameObjectType.HerbivoreAnimal3; } }

        public bool HasGender { get { return true; } }

        public bool CanMove { get { return true; } }

        public int MaxHealth { get { return 25; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 20; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType> { GameObjectType.Fruit1, GameObjectType.Fruit2, GameObjectType.Fruit3 };

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.SameCell; } }
    }
}
