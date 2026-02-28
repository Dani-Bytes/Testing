public class InterpolationSearcher
{
    public int Search(int key, Order[] orders)
    {
        int bottom = 0;
        int top = orders.Length - 1;

        while (bottom <= top && key >= orders[bottom].OrderID && key <= orders[top].OrderID)
        {
            if (bottom == top)
            {
                if (orders[bottom].OrderID == key) return bottom;
                return -1;
            }

            // The "Guessing" Math Formula
            int pos = bottom + (((top - bottom) / (orders[top].OrderID - orders[bottom].OrderID)) * (key - orders[bottom].OrderID));

            if (orders[pos].OrderID == key) return pos; // Found it!
            if (orders[pos].OrderID < key) bottom = pos + 1;
            else top = pos - 1;
        }
        return -1;
    }
}