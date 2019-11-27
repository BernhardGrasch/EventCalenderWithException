using EventCalendar.Entities;
using EventCalendar.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EventCalendar.Test
{
    [TestClass]
    public class ControllerTests
    {
        [TestMethod]
        public void Constructor_NoEvent_ShouldReturnCountZero()
        {
            // Arrange
            Controller controller = new Controller();
            // Act
            // Assert
            Assert.AreEqual(0, controller.EventsCount);
        }

        [TestMethod]
        public void CreateEvent_AllOk_ShouldNotThrowAnyException()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            try
            {
                // Act
                controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            }
            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, but got: " + e.Message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateEvent_InvitorNull_ShouldThrowArgumentNullExceptionInvitor()
        {
            // Arrange
            Controller controller = new Controller();

            try
            {
                // Act
                controller.CreateEvent(null, "First Event", DateTime.Now.AddDays(1));
            }
            // Assert
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(String.Equals(e.ParamName, "invitor", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateEvent_TitleNullOrEmpty_ShouldThrowArgumentNullExceptionTitle()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };

            try
            {
                // Act
                controller.CreateEvent(invitor, "", DateTime.Now.AddDays(1));
            }
            // Assert
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(String.Equals(e.ParamName, "title", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateEvent_DateInThePast_ShouldThrowArgumentException()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };

            try
            {
                // Act
                controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(-1));
            }
            // Assert
            catch (ArgumentException e)
            {
                Assert.AreEqual("The 'dateTime' parameter has to be in the future!", e.Message);
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateEvent_TitleNotUnique_ShouldThrowArgumentException()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };

            // Act
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));

            try
            {
                controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            }
            // Assert
            catch (ArgumentException e)
            {
                Assert.AreEqual("The 'title' parameter has to be unique!", e.Message);
                throw;
            }
        }


        [TestMethod]
        public void EventsCount_FirstEventWithoutParticipators_ShouldReturnOneEvent()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            // Act
            // Assert
            Assert.AreEqual(1, controller.EventsCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEvent_TitleNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Controller controller = new Controller();

            try
            {
                // Act
                Event ev = controller.GetEvent(null);
            }
            // Assert
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(String.Equals(e.ParamName, "title", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEvent_TitleEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            Controller controller = new Controller();

            try
            {
                // Act
                controller.GetEvent("");
            }
            // Assert
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(String.Equals(e.ParamName, "title", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }


        [TestMethod]
        public void GetEvent_TitleFound_ShouldReturnNull()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            // Act
            var ev = controller.GetEvent("Second Event");
            // Assert
            Assert.IsNull(ev);
        }

        [TestMethod]
        public void GetEvent_Title_ShouldReturnEvent()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            // Act
            var ev = controller.GetEvent("First Event");
            // Assert
            Assert.IsNotNull(ev);
            Assert.AreEqual("First Event", ev.Title);
        }

        [TestMethod]
        public void CountEventsForPerson_TwoEventsRegistered_ShouldReturnTwo()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            controller.CreateEvent(invitor, "Second Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator1, ev2);
            // Act
            int eventsCounterParticipatoricipator1 = controller.CountEventsForPerson(participator1);
            // Assert
            Assert.AreEqual(2, eventsCounterParticipatoricipator1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CountEventsForPerson_OneEventRegisteredTwice_ShouldThrowInvalidOperationException()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part1", "Hans");

            // Act
            try
            {
                controller.RegisterPersonForEvent(participator1, ev1);
                controller.RegisterPersonForEvent(participator1, ev1);

                controller.CountEventsForPerson(participator1);
            }
            // Assert
            catch (InvalidOperationException e)
            {
                Assert.IsTrue(String.Equals(e.Message, "The person is already registered", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CancelEvent_EventIsAlreadyCanceled_ShouldThrowInvalidOperationException()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            ev1.CancelEvent();

            // Act
            try
            {
                ev1.CancelEvent();
            }
            // Assert
            catch (InvalidOperationException e)
            {
                Assert.IsTrue(String.Equals(e.Message, "The event is already canceled!", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }

        [TestMethod]
        public void CountEventsForPerson_OneEventsRegisteredAndUnregistered_ShouldReturnZero()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.UnregisterPersonForEvent(participator1, ev1);
            // Act
            int eventsCounterParticipator1 = controller.CountEventsForPerson(participator1);
            // Assert
            Assert.AreEqual(0, eventsCounterParticipator1);
        }

        [TestMethod]
        public void CountEventsForPerson_ZeroRegisteredAndUnregistered_ShouldReturnZero()
        {
            // Arrange
            Controller controller = new Controller();
            Person participator1 = new Person("Part1", "Hans");
            // Act
            int eventsCounterParticipator1 = controller.CountEventsForPerson(participator1);
            // Assert
            Assert.AreEqual(0, eventsCounterParticipator1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountEventsForPerson_PersonNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Controller controller = new Controller();

            try
            {
                // Act
                controller.CountEventsForPerson(null);
            }
            // Assert
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(String.Equals(e.ParamName, "participator", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }

        [TestMethod]
        public void GetParticipatorsForEvent_FirstEventWithoutParticipators_ShouldReturnEmptyList()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            // Act
            var participators = controller.GetParticipatorsForEvent(ev1);
            // Assert
            Assert.AreEqual(0, participators.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetParticipatorsForEvent_EventNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Controller controller = new Controller();

            try
            {
                // Act
                controller.GetParticipatorsForEvent(null);
            }
            // Assert
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(String.Equals(e.ParamName, "event", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }


        [TestMethod]
        public void GetParticipatorsForEvent_TwoParticipatorsDifferentEventCounterCorrectOrder_ShouldReturnSameOrder()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            Person participator2 = new Person("Part2", "Franz");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator2, ev1);
            
            // Act
            var people = controller.GetParticipatorsForEvent(ev1);
            
            // Assert
            Assert.AreSame(participator1, people[0]);
            Assert.AreSame(participator2, people[1]);
        }

        [TestMethod]
        public void GetParticipatorsForEvent_TwoParticipatorsDifferentEventCounterRegisteredWrongOrder_ShouldReturnCorrectOrder()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part1", "Hans");
            Person participator2 = new Person("Part2", "Franz");
            controller.RegisterPersonForEvent(participator2, ev1);
            controller.RegisterPersonForEvent(participator1, ev1);
            
            // Act
            var people = controller.GetParticipatorsForEvent(ev1);

            // Assert
            Assert.AreSame(participator1, people[0]);
            Assert.AreSame(participator2, people[1]);
        }

        [TestMethod]
        public void GetParticipatorsForEvent_TwoParticipatorsSameEventCounterRegisteredWrongOrder_ShouldReturnCorrectOrder()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part", "Franz");
            Person participator2 = new Person("Part", "Hans");
            controller.RegisterPersonForEvent(participator2, ev1);
            controller.RegisterPersonForEvent(participator1, ev1);
            // Act
            var people = controller.GetParticipatorsForEvent(ev1);
            // Assert
            Assert.AreSame(participator1, people[0]);
            Assert.AreSame(participator2, people[1]);
        }

        [TestMethod]
        public void GetParticipatorsForEvent_TwoParticipatorsSameEventCounterRegisteredCorrectOrder_ShouldReturnCorrectOrder()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part", "Franz");
            Person participator2 = new Person("Part", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator2, ev1);
            // Act
            var people = controller.GetParticipatorsForEvent(ev1);
            // Assert
            Assert.AreSame(participator1, people[0]);
            Assert.AreSame(participator2, people[1]);
        }

        [TestMethod]
        public void GetEventsForPerson_TwoEventsRegisteredInCorrectOrder_ShouldReturnTwoEvents()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            controller.CreateEvent(invitor, "Second Event", DateTime.Now.AddDays(2));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev2);
            controller.RegisterPersonForEvent(participator1, ev1);
            // Act
            var events = controller.GetEventsForPerson(participator1);
            // Assert
            Assert.AreSame(ev1, events[0]);
            Assert.AreSame(ev2, events[1]);
        }

        [TestMethod]
        public void GetEventsForPerson_TwoEventsRegisteredInWrongOrder_ShouldReturnTwoEvents()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            controller.CreateEvent(invitor, "Second Event", DateTime.Now.AddDays(2));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator1, ev2);
            // Act
            var events = controller.GetEventsForPerson(participator1);
            // Assert
            Assert.AreSame(ev1, events[0]);
            Assert.AreSame(ev2, events[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEventsForPerson_PersonNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Controller controller = new Controller();

            try
            {
                // Act
                var events = controller.GetEventsForPerson(null);
            }
            // Assert
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(String.Equals(e.ParamName, "person", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }


        [TestMethod]
        public void UnregisterPersonFromEvent_UnregisterOneOfTwo_ShouldNotThrowAnyException()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            controller.CreateEvent(invitor, "Second Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator1, ev2);
            try
            {
                // Act
                controller.UnregisterPersonForEvent(participator1, ev1);
            }
            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, but got: " + e.Message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnregisterPersonFromEvent_PersonNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");

            try
            {
                // Act
                controller.UnregisterPersonForEvent(null, ev1);
            }
            // Assert
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(String.Equals(e.ParamName, "person", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnregisterPersonFromEvent_EventNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Controller controller = new Controller();
            Person participator1 = new Person("Part1", "Hans");

            try
            {
                // Act
                controller.UnregisterPersonForEvent(participator1, null);
            }
            // Assert
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(String.Equals(e.ParamName, "event", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }

        

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnregisterPersonFromEvent_PersonNotRegistered_ShouldThrowInvalidOperationException()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            controller.CreateEvent(invitor, "Second Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            Person participator2 = new Person("Part2", "Franz");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator1, ev2);
            controller.RegisterPersonForEvent(participator2, ev1);

            try
            {
                // Act
                controller.UnregisterPersonForEvent(participator2, ev2);
            }
            // Assert
            catch (InvalidOperationException e)
            {
                Assert.IsTrue(String.Equals(e.Message, "The person has no registration for the event!", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }

        [TestMethod]
        public void RegisterForLimitedEvent_SecondOfTwoParticipators_ShouldNotThrowAnyException()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "Limited Event", DateTime.Now.AddDays(1), 2);
            Event ev1 = controller.GetEvent("Limited Event");
            Person participator1 = new Person("Part1", "Hans");
            Person participator2 = new Person("Part2", "Franz");
            controller.RegisterPersonForEvent(participator1, ev1);

            try
            {
                // Act
                controller.RegisterPersonForEvent(participator2, ev1);
            }
            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, but got: " + e.Message);
            }
        }

        [TestMethod]
        public void RegisterPersonForEvent_FirstRegistration_ShouldNotThrowAnyExceptions()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part1", "Hans");

            try
            {
                // Act
                controller.RegisterPersonForEvent(participator1, ev1);
            }
            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, but got: " + e.Message);
            }
        }

        

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterPersonForEvent_PersonNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");

            try
            {
                // Act
                controller.RegisterPersonForEvent(null, ev1);
            }
            // Assert
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(String.Equals(e.ParamName, "person", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterPersonForEvent_EventNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Controller controller = new Controller();
            Person participator1 = new Person("Part1", "Hans");

            try
            {
                // Act
                controller.RegisterPersonForEvent(participator1, null);
            }
            // Assert
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(String.Equals(e.ParamName, "event", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterForLimitedEvent_SecondParticipatorByLimitOne_ShouldThrowInvalidOperationException()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "Limited Event", DateTime.Now.AddDays(1), 1);
            Event ev1 = controller.GetEvent("Limited Event");
            Person participator1 = new Person("Part1", "Hans");
            Person participator2 = new Person("Part2", "Franz");
            controller.RegisterPersonForEvent(participator1, ev1);

            try
            {
                // Act
                controller.RegisterPersonForEvent(participator2, ev1);
            }
            catch (InvalidOperationException e)
            {
                Assert.IsTrue(String.Equals(e.Message, "The maximum number of registrations has been reached!", StringComparison.OrdinalIgnoreCase));
                throw;
            }
        }



    }
}