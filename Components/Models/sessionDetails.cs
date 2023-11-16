namespace Grupparbete_OOP_grund_blazor.Models;

public class SessionDetails
{
    public static bool IsLoggedIn = false;
    public static Guest? SessionGuest {get; set; }

    //creating default rooms and guests booking
    public static void Initialize()
    {
        Hotel.Rooms.Add(new Room("1", "Royal suite", 3999.99, 5, 4));
        Hotel.Rooms.Add(new Room("2", "Double room", 1499.99, 3, 2));
        Hotel.Rooms.Add(new Room("3", "Single room", 599.99, 1, 3));
        Hotel.Rooms.Add(new Room("4", "Family room", 1799.99, 5, 2));

        GuestList.guestList.Add(new Guest("Henrik", "073", "henrik.com", "hola"));
        GuestList.guestList.Add(new Guest("Krister", "072", "krister.com", "hej"));
        GuestList.guestList.Add(new Guest("Sandra", "071", "sandra.com", "halla"));

        Booking b = new Booking(GuestList.guestList[0], new List<Room>(), new BookingPeriod(new DateOnly (2023,12,24), new DateOnly (2023,12,26)), 1);
        b.BookedRooms.Add(Hotel.Rooms[1]);

        List<Room> rooms = new List<Room>();
        rooms.Add(Hotel.Rooms[1]);
        Guest.BookRoom(rooms, new DateOnly (2023,12,24), new DateOnly (2023,12,26), b.Guest!);
    }

    public static void LogOut()
    {
        SessionGuest = null;
        IsLoggedIn = false;
    }
}
