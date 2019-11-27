using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using EventCalendar.Entities;
using static System.String;

namespace EventCalendar.Logic
{
    public class Controller
    {
        private readonly ICollection<Event> _events;
        public int EventsCount { get { return _events.Count; } }

        public Controller()
        {
            _events = new List<Event>();
        }

        /// <summary>
        /// Ein Event mit dem angegebenen Titel und dem Termin wird für den Einlader angelegt.
        /// Der Titel muss innerhalb der Veranstaltungen eindeutig sein und das Datum darf nicht
        /// in der Vergangenheit liegen.
        /// Mit dem optionalen Parameter maxParticipators kann eine Obergrenze für die Teilnehmer festgelegt
        /// werden.
        /// </summary>
        /// <param name="invitor"></param>
        /// <param name="title"></param>
        /// <param name="dateTime"></param>
        /// <param name="maxParticipators"></param>
        /// <returns>Wurde die Veranstaltung angelegt</returns>
        public bool CreateEvent(Person invitor, string title, DateTime dateTime, int maxParticipators = 0)
        {
            bool eventCreated = false;

            if (invitor == null)
            {
                throw new ArgumentNullException(nameof(invitor));
            }

            if (title == null || title.Length < 1) //string.isNullOrEmpty(string) für(null, "")   string.IsNullOrWhitespace(string) für(null, "", " ")
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (dateTime < DateTime.Now)
            {
                throw new ArgumentException("The 'dateTime' parameter has to be in the future!");
            }

            if (!UniqueTitle(title))
            {
                throw new ArgumentException("The 'title' parameter has to be unique!");
            }

            Event myEvent;
            if (UniqueTitle(title))
            {
                if (maxParticipators == 0)
                {
                    myEvent = new Event(invitor, title, dateTime);
                }
                else
                {
                    myEvent = new EventWithLimit(invitor, title, dateTime, maxParticipators);
                }
                eventCreated = true;
                _events.Add(myEvent);
            }

            return eventCreated;
        }

        public bool UniqueTitle(string title)
        {
            bool isUnique = true;

            foreach (Event item in _events)
            {
                if (item.Title.Equals(title))
                {
                    isUnique = false;
                }
            }
            return isUnique;
        }

        /// <summary>
        /// Liefert die Veranstaltung mit dem Titel
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Event oder null, falls es keine Veranstaltung mit dem Titel gibt</returns>
        public Event GetEvent(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException(nameof(title));
            }
            Event myEvent = null;
            foreach (Event item in _events)
            {
                if (item.Title.Equals(title))
                {
                    myEvent = item;
                }
            }
            return myEvent;
        }

        /// <summary>
        /// Person registriert sich für Veranstaltung.
        /// Eine Person kann sich zu einer Veranstaltung nur einmal registrieren.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="ev">Veranstaltung</param>
        /// <returns>War die Registrierung erfolgreich?</returns>
        public bool RegisterPersonForEvent(Person person, Event ev)
        {
            bool register = false;

            if (ev == null)
            {
                throw new ArgumentNullException("event");
            }

            if (person == null)
            {
                throw new ArgumentNullException("person");
            }

            if (ev.GetType().Equals(typeof(EventWithLimit)))
            {
                EventWithLimit tmp = ev as EventWithLimit;

                if (tmp.Participators.Count < tmp.MaxParticipators)
                {
                    if (GetEvent(ev.Title).Equals(ev))
                    {
                        if (ev.AddParticipatorToEvent(person) == true)
                        {
                            person.AddEventToPerson(ev);
                            register = true;
                        }
                        else
                        {
                            throw new InvalidOperationException("The person is already registered");
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("The maximum number of registrations has been reached!");
                }
            }
            else
            {
                if (GetEvent(ev.Title).Equals(ev))
                {
                    if (ev.AddParticipatorToEvent(person) == true)
                    {
                        person.AddEventToPerson(ev);
                        register = true;
                    }
                    else
                    {
                        throw new InvalidOperationException("The person is already registered");
                    }
                }
            }

            return register;
        }


        /// <summary>
        /// Person meldet sich von Veranstaltung ab
        /// </summary>
        /// <param name="person"></param>
        /// <param name="ev">Veranstaltung</param>
        /// <returns>War die Abmeldung erfolgreich?</returns>
        public bool UnregisterPersonForEvent(Person person, Event ev)
        {
            bool unregister = false;

            if (person == null)
            {
                throw new ArgumentNullException("person");
            }

            if (ev == null)
            {
                throw new ArgumentNullException("event");
            }

            if (person != null && ev != null)
            {
                if (GetEvent(ev.Title).Equals(ev))
                {
                    if (ev.RemoveParticipatorFromEvent(person) == true)
                    {
                        person.RemoveEventFromPerson(ev);
                        unregister = true;
                    }
                    else
                    {
                        throw new InvalidOperationException("The person has no registration for the event!");
                    }
                }
            }

            return unregister;
        }

        /// <summary>
        /// Liefert alle Teilnehmer an der Veranstaltung.
        /// Sortierung absteigend nach der Anzahl der Events der Personen.
        /// Bei gleicher Anzahl nach dem Namen der Person (aufsteigend).
        /// </summary>
        /// <param name="ev"></param>
        /// <returns>Liste der Teilnehmer oder null im Fehlerfall</returns>
        public IList<Person> GetParticipatorsForEvent(Event ev)
        {
            List<Person> person = null;

            if (ev == null)
            {
                throw new ArgumentNullException("event");
            }

            if (ev != null)
            {
                person = ev.Participators;
                person.Sort();
            }

            return person;
        }

        /// <summary>
        /// Liefert alle Veranstaltungen der Person nach Datum (aufsteigend) sortiert.
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Liste der Veranstaltungen oder null im Fehlerfall</returns>
        public List<Event> GetEventsForPerson(Person person)
        {
            List<Event> events = null;

            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            if (person != null)
            {
                events = new List<Event>();
                foreach (Event ev in _events)
                {
                    if (ev.PersonInList(person) == true)
                    {
                        events.Add(ev);
                    }
                }
            }

            return events;
        }

        /// <summary>
        /// Liefert die Anzahl der Veranstaltungen, für die die Person registriert ist.
        /// </summary>
        /// <param name="participator"></param>
        /// <returns>Anzahl oder 0 im Fehlerfall</returns>
        public int CountEventsForPerson(Person participator)
        {
            int count = 0;

            if (participator == null)
            {
                throw new ArgumentNullException("participator");
            }

            if (participator != null)
            {
                count = participator.CountEventsForPerson;
            }

            return count;
        }

    }
}
