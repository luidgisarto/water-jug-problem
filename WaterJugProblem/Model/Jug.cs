namespace WaterJugProblem.Model
{
    public class Jug
    {
        public Jug(int capacity)
        {
            Capacity = capacity;
            Current = 0;
        }

        public int Capacity { get; private set; }

        public int Current { get; private set; }

        public int EmptySpace
        {
            get
            {
                return Capacity - Current;
            }
        }

        public void UpdateContent(int amount)
        {
            Current += amount;
        }

        public bool IsEmpty()
        {
            return Current == 0;
        }

        public bool IsFull()
        {
            return Current == Capacity;
        }

        public bool FillJug()
        {
            Current = Capacity;
            return true;
        }

        public bool EmptyJug()
        {
            Current = 0;
            return true;
        }
    }
}
