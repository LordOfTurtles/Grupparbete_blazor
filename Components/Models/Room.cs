namespace Grupparbete_OOP_grund_blazor.Models;

public class Room
//class for rooms at the hotel, includes a list of periods during which the room is booked, a roomnumber, a description of the room, bools for whether the room is checked into/booked or not, a price for one night, guest capacity and floor number
{
    public List<Booking> roomBookings = new List<Booking>();
    public List<Booking> pastBookings = new List<Booking>();
    public string RoomNr;
    public string Description;
    public double RoomPrice;
    public int Capacity;

    public int FloorNr;

    public Room(string roomNr, string description, double roomPrice, int capacity, int floorNr)
    //constructor for the room class, does not include a list of bookingperiods since those are added/removed when the room is booked or checked out of
    {
        RoomNr  = roomNr;
        Description = description;
        RoomPrice = roomPrice;
        Capacity = capacity;
        FloorNr = floorNr;
    }

    public Room()
    {
        
    }

    //An override method that overrides whats typed out in the console when a room object is the input 
    public override string ToString()
    {
        string rom = $"<b>Room number:</b> {RoomNr}. '{Description}'<br /><b>Price:</b> {RoomPrice}kr <br /><b>Capacity:</b> {Capacity} <br /><b>Currently booked dates:</b> ";
        
        if(roomBookings.Count > 0)
        
            //checks if there are any bookings in the current room "r"
            {

                foreach(Booking bp in roomBookings)
                {
                    rom += $"<br />{bp.BookingPeriod.StartDate} until {bp.BookingPeriod.EndDate} ";
                }
                //types out every booking that is currently made for room "r"
                rom += "";
            }
            else
            {
                rom += "No bookings at the moment";
            }
        return rom;
    }
}

