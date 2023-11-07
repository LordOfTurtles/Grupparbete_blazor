namespace Grupparbete_OOP_grund_blazor.Models;

public class Guest
{
    //class for guests, includes name of guest, an id, phonenr, email and a list of reviews made by the guest
    public string Name{get; set;}
    public string PhoneNr{get; set;}
    public string Email{get; set;}
    public string Password{get; set;}
    public List<Booking> guestBookings = new List<Booking>();
    public List<Booking> guestPastBookings = new List<Booking>();   
    //list of reviews made by a specific guest

    //constructor for the guest class, does not include a list of reviews which are added when written by the guest
    public Guest(string name, string phoneNr, string email, string password)
    {
       
        Name = name;
        PhoneNr = phoneNr;
        Email = email;
        Password = password;
    }

    //method for checking what rooms are available and when, also offers the guest the option to book a room through calling on the BookRoom method
    public static string AvaliableRooms()
    {
        string output = "Currently available rooms:";
        foreach(Room r in Hotel.Rooms)
        //goes through every room that exists in the static list in the hotel class
        {
            output += $"\n{r}\n";
        }
        return output;
    }

    public static string BookRoom(Room room, DateOnly startDate, DateOnly endDate, Guest guest)
    {
        List<Room> roomBooking = new List<Room>();
        BookingPeriod myBp = new BookingPeriod(startDate, endDate);
        roomBooking.Add(room);
        Booking myBooking = new Booking(guest, roomBooking, myBp, roomBooking[0].Capacity);
        room.roomBookings.Add(myBooking);
        guest.guestBookings.Add(myBooking);
        return $"{myBooking.Guest.Name}, {myBooking.BookedRooms[0].RoomNr}, Period: {myBooking.BookingPeriod.StartDate} until {myBooking.BookingPeriod.EndDate} check in at: {myBooking.BookingPeriod.StartTime} check out at: {myBooking.BookingPeriod.EndTime}";
    }

    //Method that checks that a timeperiod doesnt coincide with an already existing booking
    public static bool CompareDates(DateOnly startDate, DateOnly endDate, Room room)
    {
        foreach(Booking b in room.roomBookings)
        {
            if(startDate >= b.BookingPeriod.StartDate && startDate < b.BookingPeriod.EndDate || 
               endDate > b.BookingPeriod.StartDate && endDate <= b.BookingPeriod.EndDate)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return true;
    }
    //method to add a review for a specific booking
    public static void AddReview( Booking booking, string review)
    {
        
        booking.Review = review;
    }
}
//a class containing the list of guests 
class GuestList
{
    public static List<Guest> guestList = new List<Guest>();

    public static void AddGuest(Guest guest, out string outputMessage)
    {
        if(guestList.Exists(x => x.Email.Contains(guest.Email)))
        {
            outputMessage = "Error, account with the specified already exists";
        }
        else
        {
            guestList.Add(guest);
            outputMessage = "Signup succesful";
        }
    }

    //Method that logs in guest user with pre registred email and password through registration method
    public static Guest LogInAttempt(string email, string password, out string outputMessage)
    {
        if(guestList.Exists(x => x.Email.Contains(email)))
        {
            Guest? guest = guestList.Find(x => x.Email.Contains(email));
            if(guest!.Password == password)
            {
                outputMessage = "Login successful";
                return guest;
            }
        }
        outputMessage = "Error, incorrect email or password";
        return null!;
    }
}