using OOPFirstLab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPFirstLab.GameObjectDescriptors
{
    public class FruitDescriptor1 : IGameObjectDescriptor
    {
        //морковь
        public bool CanBeMutant { get { return false; } }
        public GameObjectType Type { get { return GameObjectType.Fruit1; } }

        public bool HasGender { get { return false; } }

        public bool CanMove { get { return false; } }

        public int MaxHealth { get { return 0; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 10; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType>();

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.NeighborFreeCell; } }
    }

    public class FruitDescriptor2 : IGameObjectDescriptor
    {
        //трава
        public bool CanBeMutant { get { return false; } }
        public GameObjectType Type { get { return GameObjectType.Fruit2; } }

        public bool HasGender { get { return false; } }

        public bool CanMove { get { return false; } }

        public int MaxHealth { get { return 0; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 5; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType>();

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.NeighborFreeCell; } }
    }

    public class FruitDescriptor3 : IGameObjectDescriptor
    {
        //фрукт
        public bool CanBeMutant { get { return false; } }
        public GameObjectType Type { get { return GameObjectType.Fruit3; } }

        public bool HasGender { get { return false; } }

        public bool CanMove { get { return false; } }

        public int MaxHealth { get { return 0; } }

        public bool CanBreed => true;

        public int MaxBreedTimer { get { return 20; } }

        public List<GameObjectType> Food { get; private set; } = new List<GameObjectType>();

        public NewObjectPlacement NewObjectPlacement { get { return NewObjectPlacement.NeighborFreeCell; } }
    }
}
