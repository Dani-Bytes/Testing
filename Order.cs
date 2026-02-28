public class Order
{
    public int OrderID { get; set; }
    public string CustomerName { get; set; }

    public Order(int id, string name)
    {
        OrderID = id;
        CustomerName = name;
    }
}