namespace Grupparbete_OOP_grund_blazor.Models;

public class Booking
{
    //class for bookings made by guests, inludes a reference for a guest, a reference for a list of rooms, a reference for a bookingperiod, the total price for the booking, the total number of occupants and a review
    
    public Guest? Guest;
    //the guest who made the booking
    public List<Room> BookedRooms = new List<Room>();
    //a list of rooms that are booked
    public BookingPeriod? BookingPeriod;
    //a bookingperiod specifying a startdate and enddate
    public bool IsChecked;
    public double TotalPrice;
    //the total price of the booking (prices of all the rooms x timeperiod)
    public int TotalOccupants;
    //input by the guest to be compared to the total capacity of the rooms to be booked
    public string? Review;
    //a review written by the guest. Preferably at a nonspecified time after their stay.

    public Booking(Guest guest, List<Room> bookedRooms, BookingPeriod bookingPeriod, int totalOccupants)
    {
        //constructor for the booking class, does not include review since that is to be added at a later time.
        Guest = guest;
        BookedRooms = bookedRooms;
        BookingPeriod = bookingPeriod;
        //TotalPrice = totalPrice;
        foreach(Room r in bookedRooms)
        {
            TotalPrice += r.RoomPrice;
        }
        TotalOccupants = totalOccupants;
        IsChecked = false;    
    }
    public Booking()
    {
        
    }
    //An override method that overrides whats typed out in the console when a booking object is the input 
    public override string ToString()
    {
        string output = "Rooms:";

        foreach(Room r in BookedRooms)
        {
            output += $" '{r.RoomNr}. {r.Description}'";
        }
        output += $"<br />{BookingPeriod!.StartDate} - {BookingPeriod.EndDate}";
        return output;
    }
}
public class BookingPeriod
{
    //class for the period of a specific booking, includes a startdate and an enddate
    public DateOnly StartDate;
    public DateOnly EndDate;

    public TimeOnly StartTime;

    public TimeOnly EndTime;

    //bookingPeriod constructor with set start and endTimes
    public BookingPeriod(DateOnly startDate, DateOnly endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
        StartTime = new TimeOnly (15,00);
        EndTime = new TimeOnly (11,00);
    }

    //An override method that overrides whats typed out in the console when a bookingPeriod is the input 
    public override string ToString()
    {   

        return $"<b>Start:</b> {StartDate} <b>End:</b> {EndDate}";
    }




}