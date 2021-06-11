namespace OOPFirstLab
{
    public class Mutant : Animal
    {

        public Mutant(GameEngine gameEngine, Gender g) : base(GameObjectType.Mutant, gameEngine, g)
        {
            // У мутанта на 50% больше здоровья
            Health = (int)(Health * 1.5);
        }
    }

}
