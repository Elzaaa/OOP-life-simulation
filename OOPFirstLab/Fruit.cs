using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPFirstLab
{
    public class Fruit : GameObject
    {
        private static int kMaxTimeToGrow = 10;

        private GameEngine _gameEngine;

        public Fruit(GameEngine gameEngine) : base(GameObjectType.Fruit)
        {
            TimeToGrow = kMaxTimeToGrow;
            _gameEngine = gameEngine;
        }

        public int TimeToGrow { get; private set; }

        public override bool IsDead
        {
            get { return false; }
        }

        public override void Die()
        {
        }

        public override void MakeMove()
        {
            if (!_gameEngine.IsZasuha)
            {
                --TimeToGrow;
            }

            if (TimeToGrow == 0)
            {
                // разрастаемся
                Fruit f = _gameEngine.CreateRandomFruit();
                Position myPos = _gameEngine.GetCurrentMap().GetFruitPosition(this);
                _gameEngine.GetCurrentMap().PlaceFruitNearPosition(f, myPos);

                TimeToGrow = kMaxTimeToGrow;
            }
        }
    }
}
