public class LinearSearcher
{
    public int Search(int key, Order[] orders)
    {
        // Look at each order one by one
        for (int i = 0; i < orders.Length; i++)
        {
            if (orders[i].OrderID == key)
            {
                return i; // Found it! Return the position.
            }
        }
        return -1; // -1 means "Not Found"
    }
}