using OOPFirstLab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPFirstLab.GameObjectDescriptors
{
    public static class GameObjectDescriptorFactory
    {
        public static IGameObjectDescriptor CreateGameObjectDescriptor(GameObjectType type)
        {
            switch (type)
            {
                case GameObjectType.Fruit1: return new FruitDescriptor1();
                case GameObjectType.Fruit2: return new FruitDescriptor2();
                case GameObjectType.Fruit3: return new FruitDescriptor3();
                case GameObjectType.HerbivoreAnimal1: return new HerbivoreDescriptor1();
                case GameObjectType.HerbivoreAnimal2: return new HerbivoreDescriptor2();
                case GameObjectType.HerbivoreAnimal3: return new HerbivoreDescriptor3();
                case GameObjectType.OmnivoreAnimal1: return new OmnivoreDescriptor1();
                case GameObjectType.OmnivoreAnimal2: return new OmnivoreDescriptor2();
                case GameObjectType.OmnivoreAnimal3: return new OmnivoreDescriptor3();
                case GameObjectType.PredatoryAnimal1: return new PredatoryDescriptor1();
                case GameObjectType.PredatoryAnimal2: return new PredatoryDescriptor2();
                case GameObjectType.PredatoryAnimal3: return new PredatoryDescriptor3();
                case GameObjectType.Human: return new HumanDescriptor();
                case GameObjectType.House: return new HouseDescriptor();
                default: throw new Exception($"Invalid GameObject type value {type:G} passed to GameObjectDescriptorFactory.CreateGameObjectDescriptor");
            }
        }
    }
}
