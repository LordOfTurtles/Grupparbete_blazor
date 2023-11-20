
using System.Security.Cryptography.X509Certificates;

namespace Grupparbete_OOP_grund_blazor.Models;

//class for a specific hotel, including its name, how many guests are currently staying and a list of all the rooms
class Hotel

{
   
    public string? Name;
    public int? CurrentGuests;

    public static string Password = "Hotel123";
    public static List<Room> Rooms = new List<Room>();
    //list of rooms at the hotel
    
    //method for staff to add rooms through the console
    public static void AddRoom(Room r, out string outputMessage)
    {
        if(Rooms.Exists(x => x.RoomNr!.Contains(r.RoomNr!)))
        {
            outputMessage = "Error, room with the specified number already exists";
        }
        else
        {
            Rooms.Add(r);
            outputMessage = $"Room '{r.RoomNr}. {r.Description}' added succesfully";
        }

    }

    //method for staff to remove rooms through the console
    public static void RemoveRoom(Room remove)
    {
        Rooms.Remove(remove);
    }

    //method for staff to check in guests to a room
    public static void CheckIn(Booking b)
    {
        b.IsChecked = true;
    }

    //method for staff to check guests out of their room
    public static void  CheckOut(Booking b)
    
    {
        b.IsChecked = false;
        b.Guest!.guestPastBookings.Add(b);
        b.Guest.guestBookings.Remove(b);
        foreach(Room r in b.BookedRooms)
        {
        r.pastBookings.Add(b);
        r.roomBookings.Remove(b);
        }

    }

    //method for staff to check the availability of rooms
    public static string RoomAvaliability()
    
    {
        string output = "Current room availability";
        foreach(Room r in Rooms)
        {
            if(r.roomBookings.Exists(x => x.IsChecked == true))
            {
                output += $"{r}\nStatus: Unavailable\n";
            }
            else
            {
                output += $"{r} \nStatus: Available\n";
            }
        }
        return output;
        
    }  
}


