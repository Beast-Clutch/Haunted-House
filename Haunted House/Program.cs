using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Haunted_House
{
    internal class Program
    {
        static void Main(string[] args)
        {
            House house = new House();
            bool stop = false;
            string input;
            
            while (!stop)
            {
                Console.Clear();
                Console.WriteLine(house.getLocation());
                Console.WriteLine($"\n Game Operation Choices: \n");
                Console.WriteLine("1 - Use Door");

                Console.WriteLine("X - Exit Game");
                Console.WriteLine("Please enter your interaction option.");
                input = Console.ReadLine();
                if (input == "X" || input == "x")
                {
                    stop = true;
                }
                else if (input == "1")
                {
                    Console.Clear();
                    Console.WriteLine($"Please select the door you would like to use \n");
                    Console.WriteLine(house.getLocation());
                    input = Console.ReadLine();

                    
                }

            }
        }
    }

    public class House
    {
        public Room CurrentRoom;
        //Dictionary<String, Room> Rooms1 = new Dictionary<String, Room>();
        private List<Room> Rooms = new List<Room>();

        public House()
        {
            //Creation of Rooms and Doors
            Room r0 = new Room("Hallway", 2);
            Rooms.Add(r0);
            Room r1 = new Room("Living Room", 8);
            Rooms.Add(r1);
            Room r2 = new Room("Outside", 10000);
            Rooms.Add(r2);
            Room r3 = new Room("Kitchen", 5);
            Rooms.Add(r3);
            Room r4 = new Room("Rear Hall", 2);
            Rooms.Add(r4);
            Room r5 = new Room("Store", 2);
            Rooms.Add(r5);
            Room r6 = new Room("Utility", 3);
            Rooms.Add(r6);
            Room r7 = new Room("WC", 1);
            Rooms.Add(r7);
            r0.AddOccupant("Bob");
            r0.AddDoor(r1);
            r0.AddLockableDoor(r2);
            r1.AddDoor(r3);
            r3.AddDoor(r5);
            r3.AddDoor(r4);
            r4.AddDoor(r6);
            r4.AddDoor(r7);
            r4.AddLockableDoor(r2);

            //Setting start point
            CurrentRoom = r0;
        }

        public string getLocation()
        {
            return CurrentRoom.DisplayInfo();
        }
        public void moveRoom()
        {
            CurrentRoom.RemoveOccupant("Bob");

        }
    }

    public class Door
    {
        // Door properties
        protected Room Room1;
        protected Room Room2;
        
        public Room move(Room currentRoom)
        {
            if (currentRoom == Room1)
            {
                return Room2;
            }
            else if (currentRoom == Room2)
            {
                return Room1;
            }
            else
            {
                return currentRoom;
            }
        }

        public Door(Room Room1_, Room Room2_)
        {
            Room1 = Room1_;
            Room2 = Room2_;
        }

        public Room getOtherRoom(Room currentRoom)
        {
            if (currentRoom == Room1)
            {
                return Room2;
            }
            else
            {
                return Room1;
            }
        }
        public virtual bool lockStatus() 
        {
            return false;
        }
        
    }
    public class LockableDoor : Door
    {
        private bool locked = true;
        public LockableDoor(Room Room1_, Room Room2_): base(Room1_, Room2_)
        {
            Room1 = Room1_;
            Room2 = Room2_;
        }
        public void Lock()
        {
            locked = true;
        }
        public void Unlock()
        {
            locked = false;
        }
        public override bool lockStatus()
        {
            return locked;
        }
    }


    public class Room
    {
        // Room properties
        protected string Name;
        protected int Capacity;
        protected List<string> Occupants;
        private List<Door> Doors = new List<Door>();

        public Room(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
            Occupants = new List<string>();
        }
        public Room(string name)
        {
            Name = name;
            Capacity = 10;
            Occupants = new List<string>();
        }

        public string getName()
        {
            return this.Name;
        }

        public virtual string AddOccupant(string occupant)
        {
            if (Occupants.Count < Capacity)
            {
                Occupants.Add(occupant);
                return $"{occupant} has entered {Name}.";
            }
            else
            {
                return $"Sorry, {Name} is full. Cannot add {occupant}.";
            }
        }

        public string RemoveOccupant(string occupant)
        {
            if (Occupants.Contains(occupant))
            {
                Occupants.Remove(occupant);
                return $"{occupant} has left {Name}.";
            }
            else
            {
                return $"{occupant} is not in {Name}.";
            }
        }

        public string DisplayInfo()
        {
            string info = $"Room: {Name}\n";
            info += $"Capacity: {Capacity}\n";
            info += $"Current Occupants: {string.Join(", ", Occupants)}\n";
            int i = 0;
            foreach (Door d in Doors)
            {
                if (d is LockableDoor)
                {
                    info += "\tDoor " + i + " leading to the " + d.getOtherRoom(this).getName() + ". Door lock status is: " + Convert.ToString(d.lockStatus()) + "\n";
                }
                else
                {
                    info += "\tDoor " + i + " leading to the " + d.getOtherRoom(this).getName() + "\n";
                }
                i++;
            }
            return info;
        }
        public void AddDoor(Room nextRoom)
        {
            Door newDoor = new Door(this, nextRoom);
            Doors.Add(newDoor);

            nextRoom.AddDoor(this, true);
        }
        public void AddDoor(Room nextRoom, bool OverFlow)
        {
            Door newDoor = new Door(this, nextRoom);
            Doors.Add(newDoor);
        }
        public void AddLockableDoor(Room nextRoom)
        {
            LockableDoor newDoor = new LockableDoor(this, nextRoom);
            Doors.Add(newDoor);
            nextRoom.AddDoor(this, true);
        }
        public void AddLockableDoor(Room nextRoom, bool OverFlow)
        {
            LockableDoor newDoor = new LockableDoor(this, nextRoom);
            Doors.Add(newDoor);
        }
    }
}
