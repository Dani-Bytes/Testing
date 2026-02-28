public class OrderSearchManager
{
    private LinearSearcher _linear = new LinearSearcher();
    private BinarySearcher _binary = new BinarySearcher();
    private InterpolationSearcher _interpolation = new InterpolationSearcher();

    public int FindOrder(int key, Order[] orders, string searchType)
    {
        if (searchType == "Linear") return _linear.Search(key, orders);
        if (searchType == "Binary") return _binary.Search(key, orders);
        if (searchType == "Interpolation") return _interpolation.Search(key, orders);
        
        return -1;
    }
}