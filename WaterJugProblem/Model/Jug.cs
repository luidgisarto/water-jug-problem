using System;

namespace WaterJugProblem.Model
{
    public class Jug : ICloneable
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
            if(EmptySpace > 0)
            {
                Current = Capacity;
                return true;
            }

            return false;
        }

        public bool EmptyJug()
        {
            if(Current > 0)
            {
                Current = 0;
                return true;
            }

            return false;
        }

        public object Clone()
        {
            var newJug = new Jug(Capacity);
            newJug.UpdateContent(Current);
            return newJug;
        }
    }
}
