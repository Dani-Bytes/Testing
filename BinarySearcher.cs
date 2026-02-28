public class BinarySearcher
{
    // We changed int[] to Order[] 
    public int Search(int key, Order[] orders)
    {
        int bottom = 0;
        int top = orders.Length - 1;
        int mid; int index = -1; 
        bool found = false;

        while (bottom <= top && found == false)
        {
            mid = (top + bottom) / 2;
            
            // We changed elemArray[mid] to orders[mid].OrderID
            if (orders[mid].OrderID == key)
            {
                index = mid;
                found = true; return index;
            }
            else
            {
                if (orders[mid].OrderID < key)
                    bottom = mid + 1;
                else
                    top = mid - 1;
            }
        }
        return index;
    }
}