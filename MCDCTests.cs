namespace Testing;

// =============================================================================
// MC/DC (Modified Condition/Decision Coverage) Test Suite
// =============================================================================
// Each decision point in the production code has every condition independently
// shown to affect the outcome — satisfying MC/DC coverage criteria.
//
// Classes Under Test:
//   - LinearSearcher
//   - BinarySearcher
//   - InterpolationSearcher
//   - OrderSearchManager
// =============================================================================

[TestFixture]
[Category("MainBranch")]
public class LinearSearcherMCDCTests
{
    private LinearSearcher _searcher = null!;

    [SetUp]
    public void Setup() => _searcher = new LinearSearcher();

    // -------------------------------------------------------------------------
    // Decision 1: Loop condition  →  i < orders.Length
    //   Condition A: i < orders.Length
    //     A=true  → loop body executes   (non-empty array)
    //     A=false → loop exits (array is empty OR exhausted)
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – Loop: array is empty → condition A=false immediately, returns -1")]
    public void Search_EmptyArray_ReturnsMinusOne()
    {
        var orders = Array.Empty<Order>();
        Assert.That(_searcher.Search(10, orders), Is.EqualTo(-1));
    }

    [Test]
    [Description("MC/DC – Loop: single-element array, key NOT found → loop body runs once (A=true), then exits (A=false)")]
    public void Search_SingleElement_KeyNotFound_ReturnsMinusOne()
    {
        var orders = new[] { new Order(5, "Alice") };
        Assert.That(_searcher.Search(99, orders), Is.EqualTo(-1));
    }

    // -------------------------------------------------------------------------
    // Decision 2: Inner condition  →  orders[i].OrderID == key
    //   Condition B: orders[i].OrderID == key
    //     B=true  → return index immediately
    //     B=false → continue loop
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – Inner: B=true at index 0 → returns 0")]
    public void Search_KeyAtFirstElement_ReturnsZero()
    {
        var orders = new[] { new Order(10, "Alice"), new Order(20, "Bob"), new Order(30, "Carol") };
        Assert.That(_searcher.Search(10, orders), Is.EqualTo(0));
    }

    [Test]
    [Description("MC/DC – Inner: B=false several times then B=true → returns last index")]
    public void Search_KeyAtLastElement_ReturnsLastIndex()
    {
        var orders = new[] { new Order(10, "Alice"), new Order(20, "Bob"), new Order(30, "Carol") };
        Assert.That(_searcher.Search(30, orders), Is.EqualTo(2));
    }

    [Test]
    [Description("MC/DC – Inner: B=false throughout → returns -1")]
    public void Search_KeyNotInArray_ReturnsMinusOne()
    {
        var orders = new[] { new Order(10, "Alice"), new Order(20, "Bob"), new Order(30, "Carol") };
        Assert.That(_searcher.Search(99, orders), Is.EqualTo(-1));
    }

    [Test]
    [Description("MC/DC – Inner: B=true at middle index → returns correct middle index")]
    public void Search_KeyAtMiddleElement_ReturnsMiddleIndex()
    {
        var orders = new[] { new Order(10, "Alice"), new Order(20, "Bob"), new Order(30, "Carol") };
        Assert.That(_searcher.Search(20, orders), Is.EqualTo(1));
    }
}

// =============================================================================

[TestFixture]
[Category("MainBranch")]
public class BinarySearcherMCDCTests
{
    private BinarySearcher _searcher = null!;

    [SetUp]
    public void Setup() => _searcher = new BinarySearcher();

    // -------------------------------------------------------------------------
    // Decision 1 (compound loop guard): bottom <= top  &&  found == false
    //   Condition A: bottom <= top
    //   Condition B: found == false
    //
    //   MC/DC pairs (one condition changes, outcome changes):
    //     A=true,  B=true  → loop runs
    //     A=false, B=true  → loop exits  (A independently causes exit)
    //     A=true,  B=false → loop exits  (B independently causes exit) [found is
    //                         set just before return, so captured by the
    //                         "key found" path]
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – Loop: A=false (empty array) → loop never enters, returns -1")]
    public void Search_EmptyArray_ReturnsMinusOne()
    {
        var orders = Array.Empty<Order>();
        Assert.That(_searcher.Search(5, orders), Is.EqualTo(-1));
    }

    [Test]
    [Description("MC/DC – Loop: A=true B=true → key is found (both conditions true, loop runs and terminates via found=true)")]
    public void Search_KeyFound_SingleElement_ReturnsZero()
    {
        var orders = new[] { new Order(42, "Alice") };
        Assert.That(_searcher.Search(42, orders), Is.EqualTo(0));
    }

    [Test]
    [Description("MC/DC – Loop: A becomes false before key found → search exhausted, returns -1")]
    public void Search_KeyNotFound_SearchExhausted_ReturnsMinusOne()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C") };
        Assert.That(_searcher.Search(99, orders), Is.EqualTo(-1));
    }

    // -------------------------------------------------------------------------
    // Decision 2: orders[mid].OrderID == key
    //   C=true  → found! return mid
    //   C=false → branch to < or > check
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – Mid-check: C=true at first mid → returns mid index 1")]
    public void Search_KeyAtMid_ReturnsCorrectIndex()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C") };
        Assert.That(_searcher.Search(20, orders), Is.EqualTo(1));
    }

    // -------------------------------------------------------------------------
    // Decision 3: orders[mid].OrderID < key
    //   D=true  → move bottom up  (key is in upper half)
    //   D=false → move top down   (key is in lower half)
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – Direction: D=true → bottom moves up, finds key in upper half")]
    public void Search_KeyInUpperHalf_ReturnsCorrectIndex()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C"), new Order(40, "D"), new Order(50, "E") };
        Assert.That(_searcher.Search(40, orders), Is.EqualTo(3));
    }

    [Test]
    [Description("MC/DC – Direction: D=false → top moves down, finds key in lower half")]
    public void Search_KeyInLowerHalf_ReturnsCorrectIndex()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C"), new Order(40, "D"), new Order(50, "E") };
        Assert.That(_searcher.Search(10, orders), Is.EqualTo(0));
    }

    [Test]
    [Description("MC/DC – Key is last element (right boundary)")]
    public void Search_KeyAtLastElement_ReturnsLastIndex()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C") };
        Assert.That(_searcher.Search(30, orders), Is.EqualTo(2));
    }

    [Test]
    [Description("MC/DC – Key is first element (left boundary)")]
    public void Search_KeyAtFirstElement_ReturnsZero()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C") };
        Assert.That(_searcher.Search(10, orders), Is.EqualTo(0));
    }
}

// =============================================================================

[TestFixture]
[Category("FeatureBranch")]
public class InterpolationSearcherMCDCTests
{
    private InterpolationSearcher _searcher = null!;

    [SetUp]
    public void Setup() => _searcher = new InterpolationSearcher();

    // -------------------------------------------------------------------------
    // Decision 1 (compound loop guard):
    //   bottom <= top  &&  key >= orders[bottom].OrderID  &&  key <= orders[top].OrderID
    //   Conditions: A = (bottom <= top)
    //               B = (key >= orders[bottom].OrderID)
    //               C = (key <= orders[top].OrderID)
    //
    //   MC/DC pairs:
    //     A alone causes exit → empty array (A=false)
    //     B alone causes exit → key < min element (B=false, A=true, C irrelevant)
    //     C alone causes exit → key > max element (C=false, A=true, B=true)
    //     All true            → loop executes
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – Loop guard: A=false (empty array) → returns -1")]
    public void Search_EmptyArray_ReturnsMinusOne()
    {
        var orders = Array.Empty<Order>();
        Assert.That(_searcher.Search(5, orders), Is.EqualTo(-1));
    }

    [Test]
    [Description("MC/DC – Loop guard: B=false (key < min) → loop skipped, returns -1")]
    public void Search_KeyBelowMinimum_ReturnsMinusOne()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C") };
        Assert.That(_searcher.Search(5, orders), Is.EqualTo(-1));
    }

    [Test]
    [Description("MC/DC – Loop guard: C=false (key > max) → loop skipped, returns -1")]
    public void Search_KeyAboveMaximum_ReturnsMinusOne()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C") };
        Assert.That(_searcher.Search(99, orders), Is.EqualTo(-1));
    }

    [Test]
    [Description("MC/DC – Loop guard: A=B=C=true → loop executes and finds key")]
    public void Search_KeyInRange_Found_ReturnsCorrectIndex()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C") };
        Assert.That(_searcher.Search(20, orders), Is.EqualTo(1));
    }

    // -------------------------------------------------------------------------
    // Decision 2: bottom == top
    //   D=true  → single-element sub-array; check if it matches
    //   D=false → compute interpolated position
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – SingleElement: D=true AND key matches → returns bottom (0)")]
    public void Search_SingleElement_KeyFound_ReturnsZero()
    {
        var orders = new[] { new Order(42, "Alice") };
        Assert.That(_searcher.Search(42, orders), Is.EqualTo(0));
    }

    [Test]
    [Description("MC/DC – SingleElement: D=true AND key does NOT match → returns -1")]
    public void Search_SingleElement_KeyNotFound_ReturnsMinusOne()
    {
        var orders = new[] { new Order(42, "Alice") };
        Assert.That(_searcher.Search(99, orders), Is.EqualTo(-1));
    }

    // -------------------------------------------------------------------------
    // Decision 3: orders[pos].OrderID == key  (after interpolation)
    //   E=true  → return pos
    //   E=false → branch on < or >
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – Pos hit: E=true at interpolated position → returns correct index")]
    public void Search_KeyAtInterpolatedPosition_ReturnsCorrectIndex()
    {
        // Uniformly spaced so interpolation lands exactly on the element
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C"), new Order(40, "D"), new Order(50, "E") };
        Assert.That(_searcher.Search(30, orders), Is.EqualTo(2));
    }

    // -------------------------------------------------------------------------
    // Decision 4: orders[pos].OrderID < key
    //   F=true  → bottom = pos + 1  (search upper)
    //   F=false → top    = pos - 1  (search lower)
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – Direction F=true: key in upper portion → bottom advances")]
    public void Search_KeyInUpperPortion_ReturnsCorrectIndex()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C"), new Order(40, "D"), new Order(50, "E") };
        Assert.That(_searcher.Search(40, orders), Is.EqualTo(3));
    }

    [Test]
    [Description("MC/DC – Direction F=false: key in lower portion → top retreats")]
    public void Search_KeyInLowerPortion_ReturnsCorrectIndex()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C"), new Order(40, "D"), new Order(50, "E") };
        Assert.That(_searcher.Search(10, orders), Is.EqualTo(0));
    }

    [Test]
    [Description("MC/DC – Key at last element (top boundary)")]
    public void Search_KeyAtTopBoundary_ReturnsLastIndex()
    {
        var orders = new[] { new Order(10, "A"), new Order(20, "B"), new Order(30, "C") };
        Assert.That(_searcher.Search(30, orders), Is.EqualTo(2));
    }
}

// =============================================================================

[TestFixture]
[Category("MainBranch")]
public class OrderSearchManagerMCDCTests
{
    private OrderSearchManager _manager = null!;
    // Shared sorted array (required by Binary/Interpolation searchers)
    private Order[] _orders = null!;

    [SetUp]
    public void Setup()
    {
        _manager = new OrderSearchManager();
        _orders = new[] { new Order(10, "Alice"), new Order(20, "Bob"), new Order(30, "Carol") };
    }

    // -------------------------------------------------------------------------
    // Decision 1: searchType == "Linear"
    //   A=true  → delegate to LinearSearcher and return its result
    //   A=false → fall through to next check
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – searchType: A=true (Linear) → uses LinearSearcher, returns correct index")]
    public void FindOrder_LinearSearch_KeyFound_ReturnsCorrectIndex()
    {
        Assert.That(_manager.FindOrder(20, _orders, "Linear"), Is.EqualTo(1));
    }

    [Test]
    [Description("MC/DC – searchType: A=true (Linear) → key not found returns -1")]
    public void FindOrder_LinearSearch_KeyNotFound_ReturnsMinusOne()
    {
        Assert.That(_manager.FindOrder(99, _orders, "Linear"), Is.EqualTo(-1));
    }

    // -------------------------------------------------------------------------
    // Decision 2: searchType == "Binary"
    //   B=true  → delegate to BinarySearcher and return its result
    //   B=false → fall through
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – searchType: B=true (Binary) → uses BinarySearcher, returns correct index")]
    public void FindOrder_BinarySearch_KeyFound_ReturnsCorrectIndex()
    {
        Assert.That(_manager.FindOrder(10, _orders, "Binary"), Is.EqualTo(0));
    }

    [Test]
    [Description("MC/DC – searchType: B=true (Binary) → key not found returns -1")]
    public void FindOrder_BinarySearch_KeyNotFound_ReturnsMinusOne()
    {
        Assert.That(_manager.FindOrder(99, _orders, "Binary"), Is.EqualTo(-1));
    }

    // -------------------------------------------------------------------------
    // Decision 3: searchType == "Interpolation"
    //   C=true  → delegate to InterpolationSearcher and return its result
    //   C=false → fall through to default return -1
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – searchType: C=true (Interpolation) → uses InterpolationSearcher, returns correct index")]
    public void FindOrder_InterpolationSearch_KeyFound_ReturnsCorrectIndex()
    {
        Assert.That(_manager.FindOrder(30, _orders, "Interpolation"), Is.EqualTo(2));
    }

    [Test]
    [Description("MC/DC – searchType: C=true (Interpolation) → key not found returns -1")]
    public void FindOrder_InterpolationSearch_KeyNotFound_ReturnsMinusOne()
    {
        Assert.That(_manager.FindOrder(99, _orders, "Interpolation"), Is.EqualTo(-1));
    }

    // -------------------------------------------------------------------------
    // Default: all conditions false → returns -1
    // -------------------------------------------------------------------------

    [Test]
    [Description("MC/DC – searchType: A=B=C=false (unknown type) → returns -1")]
    public void FindOrder_UnknownSearchType_ReturnsMinusOne()
    {
        Assert.That(_manager.FindOrder(10, _orders, "HashSearch"), Is.EqualTo(-1));
    }

    [Test]
    [Description("MC/DC – searchType: empty string → all conditions false, returns -1")]
    public void FindOrder_EmptySearchType_ReturnsMinusOne()
    {
        Assert.That(_manager.FindOrder(10, _orders, ""), Is.EqualTo(-1));
    }
}
