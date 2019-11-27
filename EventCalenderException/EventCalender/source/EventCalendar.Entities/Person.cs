using System;
using System.Collections.Generic;
using System.Text;

namespace EventCalendar.Entities
{

    /// <summary>
    /// Person kann sowohl zu einer Veranstaltung einladen,
    /// als auch an Veranstaltungen teilnehmen
    /// </summary>
    public class Person : IComparable<Person>
    {
        public string LastName { get; }
        public string FirstName { get; }
        public string MailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public List<Event> PersonEvents { get; set; }

        public int CountEventsForPerson 
        {
            get
            {
                return PersonEvents.Count;
            }
        }


        public Person(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
            PersonEvents = new List<Event>();
        }

        public bool AddEventToPerson(Event personEvents)
        {
            bool eventCanBeAdded = false;

            if(!(PersonEvents.Contains(personEvents)))
            {
                PersonEvents.Add(personEvents);
            }

            return eventCanBeAdded;
        }

        public bool RemoveEventFromPerson(Event personEvent)
        {
            bool eventCanBeRemoved = false;

            if(PersonEvents.Contains(personEvent))
            {
                PersonEvents.Remove(personEvent);
                eventCanBeRemoved = true;
            }

            return eventCanBeRemoved;
        }

        public int CompareTo(Person other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (CountEventsForPerson.CompareTo(other.CountEventsForPerson) == 0)
            {
                if (LastName.CompareTo(other.LastName) == 0)
                {
                    return FirstName.CompareTo(other.FirstName);
                }
                else
                {
                    return LastName.CompareTo(other.LastName);
                }
            }
            return CountEventsForPerson.CompareTo(other.CountEventsForPerson) * -1;
        }
    }
}