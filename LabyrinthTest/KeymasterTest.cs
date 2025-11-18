using Labyrinth.Build;
using Labyrinth.Items;
using Labyrinth.Tiles;

namespace LabyrinthTest;

public class KeymasterTest
{
    [Test]
    public void Dispose_ShouldThrow_WhenUnplacedKeyOrEmptyRoomExists()
    {
        var keymaster = new Keymaster();
        keymaster.NewKeyRoom(); // crée une room sans clé

        Assert.That(() => keymaster.Dispose(),
            Throws.TypeOf<InvalidOperationException>()
            .With.Message.EqualTo("Unmatched key/door creation"));
    }

    [Test]
    public void NewKeyRoom_ShouldCreateRoom_AndNotThrow()
    {
        var keymaster = new Keymaster();
        var room = keymaster.NewKeyRoom();

        Assert.That(room, Is.Not.Null);
        Assert.That(room, Is.InstanceOf<Room>());
    }

    [Test]
    public void NewDoor_ShouldCreateDoor_AndLockIt()
    {
        var keymaster = new Keymaster();
        // Crée d’abord une salle vide
        keymaster.NewKeyRoom();
        var door = keymaster.NewDoor();

        Assert.That(door, Is.Not.Null);
        Assert.That(door, Is.InstanceOf<Door>());
    }

    [Test]
    public void PlaceKey_ShouldDistributeKeys_WhenDoorAndRoomExist()
    {
        var keymaster = new Keymaster();

        // On crée d’abord une salle vide
        var room = keymaster.NewKeyRoom();
        // Puis une porte qui génère une clé
        var door = keymaster.NewDoor();

        // Après placement, les collections internes doivent être vides
        var unplacedKeyField = typeof(Keymaster)
            .GetField("unplacedKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        var unplacedKey = (MyInventory)unplacedKeyField.GetValue(keymaster)!;

        var emptyRoomsField = typeof(Keymaster)
            .GetField("emptyKeyRoom", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        var emptyRooms = (List<Room>)emptyRoomsField.GetValue(keymaster)!;

        Assert.That(unplacedKey.HasItems, Is.False);
        Assert.That(emptyRooms.Count, Is.EqualTo(0));
    }

    [Test]
    public void MultipleKeysAndDoors_ShouldMatchCounts()
    {
        var keymaster = new Keymaster();

        // 3 rooms avant portes
        var r1 = keymaster.NewKeyRoom();
        var r2 = keymaster.NewKeyRoom();
        var r3 = keymaster.NewKeyRoom();

        var d1 = keymaster.NewDoor();
        var d2 = keymaster.NewDoor();
        var d3 = keymaster.NewDoor();

        Assert.DoesNotThrow(() => keymaster.Dispose());
    }

    [Test]
    public void MissingDoor_ShouldCauseDisposeError()
    {
        var keymaster = new Keymaster();
        keymaster.NewKeyRoom(); // crée une salle sans clé

        Assert.That(() => keymaster.Dispose(),
            Throws.TypeOf<InvalidOperationException>()
            .With.Message.EqualTo("Unmatched key/door creation"));
    }

    [Test]
    public void MissingKeyRoom_ShouldCauseDisposeError()
    {
        var keymaster = new Keymaster();
        keymaster.NewDoor(); // crée une clé sans salle

        Assert.That(() => keymaster.Dispose(),
            Throws.TypeOf<InvalidOperationException>()
            .With.Message.EqualTo("Unmatched key/door creation"));
    }

    [Test]
    public void DelayedKeyAndDoor_ShouldMatchCounts()
    {
        var keymaster = new Keymaster();

        // 3 rooms avant portes
        var r1 = keymaster.NewKeyRoom();
        var d1 = keymaster.NewDoor();
        var d2 = keymaster.NewDoor();
        var r2 = keymaster.NewKeyRoom();

        Assert.DoesNotThrow(() => keymaster.Dispose());
    }
}
