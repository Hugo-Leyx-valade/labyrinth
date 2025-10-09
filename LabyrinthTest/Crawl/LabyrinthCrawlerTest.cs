using Labyrinth.Crawl;
using Labyrinth.Items;
using Labyrinth.Tiles;

namespace LabyrinthTest.Crawl;

[TestFixture(Description = "Integration test for the crawler implementation in the labyrinth")]
public class LabyrinthCrawlerTest
{
    #region Initialization
    [Test]
    public void InitWithCenteredX()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                | x|
                +--+
                """);
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void InitWithMultipleXUsesLastOne()
    {
        var laby = new Labyrinth.Labyrinth("""
                +---+
                | x |
                |  x|
                +---+
                """);
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(3));
        Assert.That(test.Y, Is.EqualTo(2));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Room>());
    }

    [Test]
    public void InitWithNoXThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Labyrinth.Labyrinth("""
                +--+
                |  |
                +--+
                """));
    }
    #endregion

    #region Labyrinth borders
    [Test]
    public void FacingNorthOnUpperTileReturnsOutside()
    {
        var map = string.Join('\n', new[]
        {
            "- +",
            "|x|",
            "--+"
        });
        var laby = new Labyrinth.Labyrinth(map);
        var test = laby.NewCrawler();

        test.Walk();

        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingWestOnFarLeftTileReturnsOutside()
    {
        var map = string.Join('\n', new[]
        {
            "---",
            "x |",
            "---"
        });
        var laby = new Labyrinth.Labyrinth(map);
        var test = laby.NewCrawler();

        test.Direction.TurnLeft();


        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingEastOnFarRightTileReturnsOutside()
    {
        var map = string.Join('\n', new[]
        {
            "   ",
            "  x",
            "---"
        });
        var laby = new Labyrinth.Labyrinth(map);
        var test = laby.NewCrawler();

        test.Direction.TurnRight();

        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
        var map = string.Join('\n', new[]
        {
            "---",
            " x ",
            "   "
        });
        var laby = new Labyrinth.Labyrinth(map);
        var test = laby.NewCrawler();

        test.Direction.TurnRight();
        test.Direction.TurnRight();
        test.Walk();

        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }
    #endregion

    #region Moves
    [Test]
    public void TurnLeftFacesWestTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                | x|
                |  |
                +--+
                """);
        var test = laby.NewCrawler();

        test.Direction.TurnLeft();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.Direction, Is.EqualTo(Direction.West));
        Assert.That(test.FacingTile, Is.TypeOf<Room>());
    }

    [Test]
    public void WalkReturnsInventoryAndChangesPositionAndFacingTile()
    {
        var map = string.Join('\n', new[]
        {
            "   ",
            " x ",
            "---"
        });
        var laby = new Labyrinth.Labyrinth(map);
        var test = laby.NewCrawler();

        var inventory = test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(inventory, Is.TypeOf<MyInventory>());
        Assert.That(inventory.HasItem, Is.False);
        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(0));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
        var map = string.Join('\n', new[]
        {
            "   ",
            " x ",
            "---"
        });
        var laby = new Labyrinth.Labyrinth(map);
        var test = laby.NewCrawler();

        test.Direction.TurnRight();
        var inventory = test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(inventory.HasItem, Is.False);
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.East));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void WalkOnNonTraversableTileThrowsInvalidOperationException()
    {
        var laby = new Labyrinth.Labyrinth("""
                +-+
                |x|
                +-+
                """);
        var test = laby.NewCrawler();

        Assert.That(() => test.Walk(), Throws.InvalidOperationException);
    }
    #endregion

    #region Items and doors
    [Test]
    public void WalkInARoomWithAnItem()
    {
        var map = string.Join('\n', new[]
        {
            "/---",
            "|xk|",
            "+--+"
        });
        var laby = new Labyrinth.Labyrinth(map);
        var test = laby.NewCrawler();

        test.Direction.TurnRight();
        var inventory = test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(inventory.HasItem, Is.True);
        Assert.That(inventory.ItemType, Is.EqualTo(typeof(Key)));
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        var map = string.Join('\n', new[]
        {
            "/--",
            "x k",
            "---"
        });
        var laby = new Labyrinth.Labyrinth(map);
        var test = laby.NewCrawler();
        var door = (Door)test.FacingTile;

        var wrongKeyInventory = new MyInventory(new Key());
        var result = door.Open(wrongKeyInventory);

        using var all = Assert.EnterMultipleScope();

        Assert.That(result, Is.False);
        Assert.That(door.IsLocked, Is.True);
        Assert.That(wrongKeyInventory.HasItem, Is.True);
    }

    [Test]
    public void WalkUseKeyToOpenADoorAndPass()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |xk|
                +-/|
                """);
        var test = laby.NewCrawler();

        test.Direction.TurnRight();

        var inventory = test.Walk();

        test.Direction.TurnRight();
        ((Door)test.FacingTile).Open(inventory);

        test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(2));
        Assert.That(test.Direction, Is.EqualTo(Direction.South));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }
    #endregion
}
