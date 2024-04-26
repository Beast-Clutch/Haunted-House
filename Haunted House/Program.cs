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
            Room r0 = new Room("Hallway", 2);
            Room r1 = new Room("Living Room", 8);
            Room r2 = new Room("Outside", 1000);
            r0.AddOccupant("Bob");
            r0.AddDoor(r2);
            Console.WriteLine(r0.DisplayInfo());




            Console.Read();
        }
    }

    public class House
    {
        Room CurrentRoom;
        Dictionary<String, Room> Rooms = new Dictionary<String, Room>();

    }

    public class Door
    {
        // Door properties
        private Room Room1;
        private Room Room2;
        
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
        public class LockableDoor : Door
        {
            private bool locked = true;
            public LockableDoor(Room Room1_, Room Room2_) : base(Room1_, Room2_)
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
                info += "\tDoor " + i + " leading to the " + d.getOtherRoom(this).getName() + "\n";
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
    }
}
