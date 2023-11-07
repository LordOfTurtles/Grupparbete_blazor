using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using Grupparbete_OOP_grund_blazor.Models;

static class ConsoleUI
{
    //Main menu for the user (guest or staff)
    public static void MainMenu()
    {
        bool isRunning = true;
        while(isRunning)
        {
            Console.WriteLine("Welcome to Hotel Booking.");
            Console.WriteLine("Choose an option: \n1.Guest \n2.Staff");
            string userInput = Console.ReadLine()!;
            if(userInput == "1")
            {
                GuestMenu();
            }

            else if(userInput == "2")
            {
                StaffMenu();
            }
            else
            {
                PrintErrorMessanger(null);
            }
        }      
    }
    //Guest menu with options to login, sign up or go back to main menu
    public static void GuestMenu()
    {
        Console.Clear();
        Guest guest = null!;
        Console.WriteLine("1. Log in\n2. Sign up as new guest\n3. Main menu");
        string userInput = Console.ReadLine()!;
        switch(userInput)
        {
            case "1":
                guest = LoginGuest();
            break;
            case "2":
                guest = RegisterGuest();
            break;
            case "3":
                MainMenu();
            break;
            default:
                PrintErrorMessanger(null);
                GuestMenu();
            break;          
        }
        bool isRunning = true;
        while(isRunning)
        {
            Console.Clear();
            Console.WriteLine("Please select an option: \n1.Check availability \n2.Start a new booking \n3.Write a review \n4.Read reviews \nLog out[x]");
            userInput = Console.ReadLine()!;
            Console.Clear();
            switch(userInput)
            {
                case "1":
                    Console.WriteLine(Guest.AvaliableRooms());
                    Console.WriteLine("Would you like to book a room? [Y]/[N]");
                    userInput = Console.ReadLine()!;
                    if(userInput.ToLower() == "y")
                    {
                        BookingInput(guest);
                    }
                    else if(userInput.ToLower() == "n")
                    {

                    }
                    else
                    {
                        PrintErrorMessanger(null);
                        
                    }
                break;

                case "2":
                CustomizedBooking(guest);
                break;

                case "3":
                ReviewInput(guest);
                break;

                case "4":
                ReadReviews();
                break;

                case "x":
                isRunning = false;
                break;

                default:
                PrintErrorMessanger(null);
                break;

            }
        }
    }
    //menu for the staff, enter a admin password.
    public static void StaffMenu()
    {
        bool isRunning = true;
        Console.WriteLine("Please enter admin password: ");
        string userInput = Console.ReadLine()!;

        if( userInput == Hotel.Password)
        {
            
            while(isRunning)
            {
                Console.Clear();
                Console.WriteLine("1. Add or remove a room: \n2. Check in guest \n3. Check out guest: \n4. Check room avaliability:\nLog out[x] ");
                userInput = Console.ReadLine()!;
                switch(userInput)
                {
                    case "1":
                    Console.WriteLine("Do you want to add or remove a room? [a]/[r]");
                    string input = Console.ReadLine()!;

                    if (input.ToLower() == "a")
                    {
                        Console.Write("Room number: ");
                        string roomNr = Console.ReadLine()!;

                        Console.Write("Description: ");
                        string description = Console.ReadLine()!;

                        Console.Write("Room price: ");
                        double roomPrice = double.Parse(Console.ReadLine()!);

                        Console.Write("Capacity: ");
                        int capacity = int.Parse(Console.ReadLine()!);     

                        Console.Write("Floor number: ");
                        int floorNr = int.Parse(Console.ReadLine()!);

                        Hotel.AddRoom(new Room(roomNr, description, roomPrice, capacity, floorNr));
                        //adds a new room with the details specified by the staff through console inputs
                    }
                    else if (input.ToLower() == "r")
                    {
                        int i = 0;
                        foreach(Room r in Hotel.Rooms)
                        {   
                            i++;
                            Console.WriteLine($"{i}.  {r.RoomNr}. '{r.Description}'");
                        }
                        //Lists all the rooms that are available to be removed with a corresponding number starting at 1
                        Console.Write("Choose room to be removed");
                        int remove = int.Parse(Console.ReadLine()!);
                        Hotel.RemoveRoom(remove -1);
                        //removes a room corresponding to the number input through the console
                    }
                    break;

                    case "2":
                    {
                        Console.Write("What roomnumber is getting checked into?: ");
                        string roomNr = Console.ReadLine()!;
                        if(Hotel.Rooms.Exists(x => x.RoomNr.Contains(roomNr)))
                        //checks if a room with the roomnumber specified through the console exists
                        {
                            int i = Hotel.Rooms.FindIndex(x => x.RoomNr.Contains(roomNr));
                            //makes an integer for the index used in the list of rooms matching the roomnumber input by the staff
                            if(Hotel.Rooms[i].roomBookings.Exists(x => x.IsChecked == true) && Hotel.Rooms[i].roomBookings.Count > 0)
                            //checks that the specified room is not currently checked into and that there is a booking for the room
                            {
                                PrintErrorMessanger($"Room number: {Hotel.Rooms[i].RoomNr}, '{Hotel.Rooms[i].Description}' is already checked into.");
                            }
                            //checks if if there are any existing booking
                            else if(Hotel.Rooms[i].roomBookings.Count > 0)
                            {
                                int bNr = 1;
                                foreach(Booking b in Hotel.Rooms[i].roomBookings)
                                {
                                    Console.WriteLine($"{bNr}. {b.Guest.Name} {b.BookingPeriod}");
                                    bNr++;
                                }
                                Console.WriteLine("Please choose which booking you would like to check in: ");
                                int uInput = int.Parse(Console.ReadLine()!) -1;
                                if(uInput < Hotel.Rooms[i].roomBookings.Count)
                                {
                                    Hotel.CheckIn(i, uInput);
                                    Console.WriteLine($"Room number: {Hotel.Rooms[i].RoomNr}, '{Hotel.Rooms[i].Description}' has been checked into.");
                                }
                                else
                                {
                                    PrintErrorMessanger(null);
                                }
                            
                            }
                            else
                            {
                                PrintErrorMessanger($"Room number: {Hotel.Rooms[i].RoomNr}, '{Hotel.Rooms[i].Description}' has not been booked.");
                                
                            }
                            Console.WriteLine(GuestList.guestList[0].guestBookings[0].IsChecked);
                        }
                        else
                        {
                            PrintErrorMessanger($"no room with roomnumber '{roomNr}' exists");
                        }
                        Console.ReadKey();
                    }
                    break;

                    case "3":
                    {
                        Console.Write("What roomnumber is getting checked out of?: ");
                        string roomNr = Console.ReadLine()!;
                        if(Hotel.Rooms.Exists(x => x.RoomNr.Contains(roomNr)))
                        //checks if a room with the roomnumber specified through the console exists
                        {
                            int i = Hotel.Rooms.FindIndex(x => x.RoomNr.Contains(roomNr));
                            //makes an integer for the index used in the list of rooms matching the roomnumber input by the staff
                            if(Hotel.Rooms[i].roomBookings.Exists(x => x.IsChecked == true))
                            //checks that the specified room is currently checked into
                            {
                                int j = Hotel.Rooms[i].roomBookings.FindIndex(x => x.IsChecked == true);
                                Console.WriteLine($"Room number: {Hotel.Rooms[i].RoomNr}. {Hotel.Rooms[i].Description}, has been checked out of.");
                                Console.WriteLine("would you like to add an review? [y]/[n]");
                                input = Console.ReadLine()!;
                                if(input.ToLower() == "y")
                                {
                                    Console.WriteLine("Please write your review: ");
                                    string review = Console.ReadLine()!;

                                    Guest.AddReview(Hotel.Rooms[i].roomBookings[j], review );


                                }
                                Hotel.CheckOut(Hotel.Rooms[i].roomBookings[j], Hotel.Rooms[i]);

                                //sets the status of the room to not be checked into and removes the booking
                            }
                            else
                            {
                                PrintErrorMessanger($"Room number: {Hotel.Rooms[i].RoomNr}. {Hotel.Rooms[i].Description}, is currently not checked into.");
                                
                            }
                        }
                        else
                        {
                            PrintErrorMessanger($"no room with roomnumber '{roomNr}' exists");
                            
                        }
                        Console.WriteLine(GuestList.guestList[0].guestBookings[0].IsChecked);
                        Console.ReadKey();
                    }
                    break;

                    case "4":
                    Console.WriteLine(Hotel.RoomAvaliability());
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    break;

                    case "x":
                    isRunning = false;
                    break;

                    //direktBokning();

                    default:
                    PrintErrorMessanger(null);
                    Console.ReadKey();
                    break;
                }
            }           
        }
        else
        {
            Console.WriteLine("incorrect password");
        }       
    }

    static void BookingInput(Guest guest)
    {
        {
            List<Room> roomBooking = new List<Room>();
            //a temporary list in which rooms that are to be booked are added
            Console.Write("What roomnumber would you like to book?: ");
            string roomNr = Console.ReadLine()!;
            if(Hotel.Rooms.Exists(x => x.RoomNr.Contains(roomNr)))
            //checks if the roomnumber input by the guest matches the list of rooms for the hotel
            {
                Room? room = Hotel.Rooms.Find(x => x.RoomNr.Contains(roomNr));
                //makes an integer for the index used in the list of rooms matching the roomnumber input by the guest
                Console.WriteLine($"Currently booked periods for room number {room!.RoomNr}. '{room!.Description}':");
                foreach(Booking bp in room!.roomBookings)
                //types out the currently booked periods for the room chosen
                {
                    Console.Write($"\n{bp.BookingPeriod.StartDate} until {bp.BookingPeriod.EndDate} ");
                }       
                Console.WriteLine($"check in at: 15.00");
                Console.Write("\nPlease input a startdate for your booking (mm/dd/yyyy): ");
                string userInput = Console.ReadLine()!;
                if(DateOnly.TryParse(userInput, out DateOnly startDate) == true)
                //checks if the string "userInput" input by the guest is in a valid format for being converted to DateTime and if so produces a DateTime variable "startDate"
                {
                    Console.WriteLine(startDate);
                    Console.WriteLine("check out at 11.00");
                    Console.Write("\nPlease input an enddate for your booking (mm/dd/yyyy):");
                    userInput = Console.ReadLine()!;
                    if(DateOnly.TryParse(userInput, out DateOnly endDate) == true && endDate > startDate)
                    //checks if the string "userInput" input by the guest is in a valid format for being converted to DateTime and if so produces a DateTime variable "endDate"
                    {
                        //checks that the timeperiod input by the guest doesnt coincide with an already existing booking
                        if(Guest.CompareDates(startDate, endDate, room) == true)
                        {
                            Console.WriteLine(Guest.BookRoom(room, startDate, endDate, guest));
                            Console.ReadKey();
                        }
                        else
                        {
                            PrintErrorMessanger("room is already booked for part of the specified period");
                            
                        }
                    }
                    else if(endDate < startDate)
                    {
                        PrintErrorMessanger("enddate must be later than startdate");
                    }
                    else
                    {
                        PrintErrorMessanger("incorrect format for enddate");
                    }
                }
                else
                {
                    PrintErrorMessanger("incorrect format for startdate");
                }
            }
        }

    }

    static void CustomizedBooking(Guest guest)
    {
        Room tempRoom = new Room("0","temp", 0, 0, 0);
        bool isRunning = true;
        while(isRunning)
        {
            Console.Clear();
            string userInput;
            
            while(isRunning)
            {
                Console.WriteLine("What is your max price per night?");
                userInput = Console.ReadLine()!;
                if(double.TryParse(userInput, out double rP))
                {
                    tempRoom.RoomPrice = rP;
                    break;
                }
                else
                {
                    PrintErrorMessanger("invalid input");
                    continue;
                }

            }
            while(isRunning)
            {
                Console.WriteLine("What capacity size is requiered for the room?");
                userInput = Console.ReadLine()!;
                if(int.TryParse(userInput, out int rS))
                {
                    tempRoom.Capacity = rS;
                    break;
                }
                {
                    PrintErrorMessanger("invalid input");
                    continue;
                }
            }
            while(isRunning)
            {
                Console.WriteLine("What date would you like to start your stay? (mm/dd/yyyy):");
                string dateInput = Console.ReadLine()!;
                if(DateOnly.TryParse(dateInput,out DateOnly startDate) == true)
                {
                    Console.WriteLine("What date will you be ending your stay (mm/dd/yyyy):");
                    dateInput = Console.ReadLine()!;
                    if(DateOnly.TryParse(dateInput, out DateOnly endDate) == true && endDate > startDate)
                    {
                        List<Room> tempList = new List<Room>();
                        int safe = 0;
                        foreach( Room r in Hotel.Rooms)
                        {
                            if( tempRoom.RoomPrice >= r.RoomPrice && tempRoom.Capacity <= r.Capacity && Guest.CompareDates(startDate, endDate, r))
                            {
                                safe++;
                                Console.WriteLine($"\n{r}\n");
                                tempList.Add(r);
                            }
                        }
                        if (safe > 0)
                        {
                            while(isRunning)
                            {
                                Console.Write("What roomnumber would you like to book?: ");
                                userInput = Console.ReadLine()!;
                                if(tempList.Exists(x => x.RoomNr.Contains(userInput)))
                                {
                                    Room? room = tempList.Find(x => x.RoomNr.Contains(userInput));
                                    Console.WriteLine(Guest.BookRoom(room!, startDate, endDate, guest));
                                    Console.ReadKey();
                                    isRunning = false;
                                }
                                else
                                {
                                    PrintErrorMessanger("invalid input");
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("There is no match to you request");
                        }
                        
                    }
                    else if(endDate <= startDate)
                    {
                        PrintErrorMessanger("enddate must be later than startdate");
                    }
                    else
                    {
                        PrintErrorMessanger("incorrect format for enddate");
                    }
                
                }
                else
                {
                    PrintErrorMessanger("incorrect format for startdate");
                }
            }
        }
    }

    //Method for where a guest can write a review
    static void ReviewInput(Guest guest)
    {
        Console.Clear();
        Console.WriteLine("Which booking do you want to review?");
        for( int i = 0; i < guest.guestPastBookings.Count; i++)
        {
            if(guest.guestPastBookings[i].Review == null)
            {
                Console.WriteLine($"{i+1}. {guest.guestPastBookings[i]}");
                //Incomplete function: bookings with a review are not displayed but can be overwritten.
            }
        }
        
        int input = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter your review here:");
        string review = Console.ReadLine()!;

        Guest.AddReview(guest.guestPastBookings[input -1],review);

        Console.WriteLine($"Review {guest.guestPastBookings[input-1].Review}");

    }

    static void ReadReviews()
    {
        foreach(Room r in Hotel.Rooms)
        {
            Console.WriteLine($"{r.RoomNr} {r.Description}: ");
            foreach(Booking b in r.pastBookings)
            {
                if(b.Review != null)
                {
                    Console.WriteLine(b.Review);
                }
            }
        }
        System.Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

    //guest login method stored from previous registration
    static Guest LoginGuest()
    {
        Guest guest = null!;
        while(guest == null)
        {
            Console.Clear();
            Console.Write("Please enter your email address: ");
            string email = Console.ReadLine()!;
            Console.Write("Please enter your password: ");
            string password = Console.ReadLine()!;
            guest = GuestList.LogInAttempt(email, password, out string outputmessage);
            Console.WriteLine(outputmessage);
            if(guest == null)
            {
                Console.WriteLine("1. Try again\n2. Register new guest\n3. Main menu");
                string userInput = Console.ReadLine()!;
                switch(userInput)
                {
                    case "1":
                    
                    break;
                    case "2":
                        guest = RegisterGuest();
                    break;
                    case "3":
                        MainMenu();
                    break;
                    default:
                    
                    break;
                }
            }
        }
        return guest;
           
    }

    //guest registration method
    static Guest RegisterGuest()
    {
        Console.Clear();
        Console.WriteLine("Register new guest");
        Console.Write("Name: ");
        string name = Console.ReadLine()!;
        Console.Write("Phone number:");
        string phoneNr = Console.ReadLine()!;
        Console.Write("Email: ");
        string email = Console.ReadLine()!;
        Console.Write("Password: ");
        string password = Console.ReadLine()!;
        Guest guest = new Guest(name, phoneNr, email, password);
        GuestList.AddGuest(guest);
        return guest;
    }

//testing method that adds guest, rooms and bookings at start up.
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

    Guest.BookRoom(Hotel.Rooms[1], new DateOnly (2023,12,24), new DateOnly (2023,12,26), b.Guest);
}

public static void PrintErrorMessanger(string? message)
{
    if(message == null)
    {
        Console.WriteLine("Error invalid input");
    }
    else
    {
        Console.WriteLine($"Error, {message}");
    }
    Console.Write("Press any key to continue");
    Console.ReadKey();
}

}