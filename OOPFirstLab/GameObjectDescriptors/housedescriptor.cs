using OOPFirstLab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPFirstLab.GameObjectDescriptors
{
    public class HouseDescriptor : IGameObjectDescriptor
    {
        public GameObjectType Type => GameObjectType.House;

        public bool HasGender => false;

        public bool CanMove => false;

        public bool CanBeMutant => false;

        public int MaxHealth => 0;

        public bool CanBreed => false;

        public int MaxBreedTimer => 0;

        public List<GameObjectType> Food => new List<GameObjectType>();

        public NewObjectPlacement NewObjectPlacement => throw new NotImplementedException();
    }
}
