public class InventoryTower
{
    public TowerData data { get; private set; }
    public int stackSize { get; private set; }

    public InventoryTower(TowerData source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}