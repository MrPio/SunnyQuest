namespace DefaultNamespace
{
    public class InventoryManager
    {
        private static InventoryManager _instance;

        public int coins = 0;

        public static InventoryManager getInstance
        {
            get { return _instance ??= new InventoryManager(); }
        }

        private InventoryManager()
        {
        }
    }
}