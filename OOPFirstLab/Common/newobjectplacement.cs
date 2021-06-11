namespace OOPFirstLab.Common
{
    /// <summary>
    /// Определяет положение нового объекта (созданного при размножении)
    /// </summary>
    public enum NewObjectPlacement
    {
        /// <summary>
        /// Новый объект должен быть помещен в соседней свободной клетке
        /// </summary>
        NeighborFreeCell,   
        
        /// <summary>
        /// Новый объект должен быть помещён в той же клетке что и родительский объект
        /// </summary>
        SameCell
    }
}