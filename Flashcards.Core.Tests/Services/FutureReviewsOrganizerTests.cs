using Flashcards.Core.Models;
using Flashcards.Core.Services;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Services
{
    public class FutureReviewsOrganizerTests
    {
        private Axis[] futureReviewsXAxes =
        {
            new Axis
            {
                Labels = new List<string>()
            }
        };

        private ObservableCollection<ISeries> futureReviewsSeries { get; set; } =
            new ObservableCollection<ISeries>();

        [Fact]
        public void OrganizeFutureReviews_UserIsNull_ShouldThrowArgumentNullException()
        {
            var organizer = new FutureReviewsOrganizer(futureReviewsXAxes, futureReviewsSeries);

            User user = null;

            Assert.Throws<ArgumentNullException>(() => organizer.OrganizeFutureReviews(user));
        }

        [Fact]
        public void OrganizeFutureReviews_UserHasNoDecks_ShouldReturnFalse()
        {
            var organizer = new FutureReviewsOrganizer(futureReviewsXAxes, futureReviewsSeries);

            var userOne = new User()
            {
                Email = "user1@user",
                Name = "user1",
                Id = 1,
                PasswordHash = "hash"
            };

            var userTwo = new User()
            {
                Decks = new ObservableCollection<Deck>(),
                Email = "user2@user",
                Id = 2,
                PasswordHash = "hash",
                Name = "user2"
            };

            Assert.False(organizer.OrganizeFutureReviews(userOne));
            Assert.False(organizer.OrganizeFutureReviews(userTwo));
        }

        [Fact]
        public void OrganizeFutureReviews_UserHasDeckWithoutFlashcards_ShouldReturnFalse()
        {
            var organizer = new FutureReviewsOrganizer(futureReviewsXAxes, futureReviewsSeries);

            var userOne = new User()
            {
                Email = "user1@user",
                Name = "user1",
                Id = 1,
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>()
            };

            var deckOne = new Deck()
            {
                Flashcards = new ObservableCollection<Flashcard>(),
                Id = 1,
                Name = "deck1",
                UserId = 1
            };

            var deckTwo = new Deck()
            {
                Id = 1,
                Name = "deck2",
                UserId = 1
            };

            userOne.Decks.Add(deckOne);
            userOne.Decks.Add(deckTwo);

            Assert.False(organizer.OrganizeFutureReviews(userOne));
        }

        [Fact]
        public void OrganizeFutureReviews_UserHasOneDeckWithFlashcards_ShouldReturnTrue()
        {
            var organizer = new FutureReviewsOrganizer(futureReviewsXAxes, futureReviewsSeries);

            var user = new User()
            {
                Decks = new ObservableCollection<Deck>(),
                Email = "email",
                Id = 1,
                Name = "user",
                PasswordHash = "hash"
            };

            var deck = new Deck()
            {
                Flashcards = new ObservableCollection<Flashcard>(),
                Id = 1,
                Name = "deck1",
                UserId = 1
            };

            var flashcardOne = new Flashcard()
            {
                Back = "back1",
                Front = "front1",
                DeckId = 1,
                Id = 1,
                Level = 0,
                NextReview = DateTime.Today.AddDays(2)
            };

            var flashcardTwo = new Flashcard()
            {
                Back = "back2",
                Front = "front2",
                DeckId = 1,
                Id = 2,
                Level = 2,
                NextReview = DateTime.Today.AddDays(2)
            };

            var flashcardThree = new Flashcard()
            {
                Back = "back3",
                Front = "front3",
                DeckId = 1,
                Id = 3,
                Level = 1,
                NextReview = DateTime.Today.AddDays(3)
            };
            deck.Flashcards.Add(flashcardOne);
            deck.Flashcards.Add(flashcardTwo);
            deck.Flashcards.Add(flashcardThree);
            user.Decks.Add(deck);

            var expectedValues = new ObservableCollection<WeightedPoint>();
            int i = 0;
            expectedValues.Add(new WeightedPoint(i, (int)DateTime.Today.DayOfWeek, 0));
            if ((int)DateTime.Today.DayOfWeek + 1 == 7) i++;
            expectedValues.Add(new WeightedPoint(i, (int)DateTime.Today.AddDays(1).DayOfWeek, 0));
            if ((int)DateTime.Today.AddDays(1).DayOfWeek + 1 == 7) i++;
            expectedValues.Add(new WeightedPoint(i, (int)DateTime.Today.AddDays(2).DayOfWeek, 2));
            if ((int)DateTime.Today.AddDays(2).DayOfWeek + 1 == 7) i++;
            expectedValues.Add(new WeightedPoint(i, (int)DateTime.Today.AddDays(3).DayOfWeek, 1));
            if ((int)DateTime.Today.AddDays(3).DayOfWeek + 1 == 7) i++;
            FillExpectedValues(i, expectedValues, DateTime.Today.AddDays(4));

            Axis[] expectedAxes = { new Axis { Labels = new List<string>() } };
            var firstDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            for (int j = 0; j <= 25; j++)
            {
                expectedAxes[0].Labels.Add(firstDate.ToString("d"));
                firstDate = firstDate.AddDays(7);
            }

            bool outcome = organizer.OrganizeFutureReviews(user);

            Assert.True(outcome);
            Assert.Single(organizer.FutureReviewsXAxes);
            Assert.Single(organizer.FutureReviewsSeries);

            for (i = 0; i < expectedValues.Count; i++)
            {
                Assert.Equal(expectedValues[i].X, organizer.FutureReviewsObservable[i].X);
                Assert.Equal(expectedValues[i].Y, organizer.FutureReviewsObservable[i].Y);
                Assert.Equal(expectedValues[i].Weight, organizer.FutureReviewsObservable[i].Weight);
            }

            for (i = 0; i < expectedAxes[0].Labels.Count; i++)
            {
                Assert.Equal(expectedAxes[0].Labels[i], organizer.FutureReviewsXAxes[0].Labels[i]);
            }
        }

        [Fact]
        public void OrganizeFutureReviews_UserHasMultipleDecksWithOrWithoutFlashcards_ShouldReturnTrue()
        {
            var organizer = new FutureReviewsOrganizer(futureReviewsXAxes, futureReviewsSeries);

            var user = new User()
            {
                Decks = new ObservableCollection<Deck>(),
                Email = "email",
                Id = 1,
                Name = "user",
                PasswordHash = "hash"
            };

            var deckOne = new Deck()
            {
                Flashcards = new ObservableCollection<Flashcard>(),
                Id = 1,
                Name = "deck1",
                UserId = 1
            };

            var deckTwo = new Deck()
            {
                Id = 2,
                Name = "deck2",
                UserId = 1
            };

            var deckThree = new Deck()
            {
                Flashcards = new ObservableCollection<Flashcard>(),
                Id = 3,
                Name = "deck3",
                UserId = 1
            };

            var deckFour = new Deck()
            {
                Flashcards = new ObservableCollection<Flashcard>(),
                Id = 4,
                Name = "deck4",
                UserId = 1
            };

            var deckOne_flashcardOne = new Flashcard()
            {
                Back = "back1",
                Front = "front1",
                DeckId = 1,
                Id = 1,
                Level = 0,
                NextReview = DateTime.Today
            };

            var deckOne_flashcardTwo = new Flashcard()
            {
                Back = "back2",
                Front = "front2",
                DeckId = 1,
                Id = 2,
                Level = 2,
                NextReview = DateTime.Today.AddDays(-12)
            };

            var deckOne_flashcardThree = new Flashcard()
            {
                Back = "back3",
                Front = "front3",
                DeckId = 1,
                Id = 3,
                Level = 1,
                NextReview = DateTime.Today.AddDays(3)
            };

            var deckFour_flashcardOne = new Flashcard()
            {
                Back = "back1",
                Front = "front1",
                DeckId = 4,
                Id = 1,
                Level = 0,
                NextReview = DateTime.Today.AddDays(2)
            };

            var deckFour_flashcardTwo = new Flashcard()
            {
                Back = "back2",
                Front = "front2",
                DeckId = 4,
                Id = 2,
                Level = 2,
                NextReview = DateTime.Today
            };

            var deckFour_flashcardThree = new Flashcard()
            {
                Back = "back3",
                Front = "front3",
                DeckId = 4,
                Id = 3,
                Level = 1,
                NextReview = DateTime.Today.AddDays(-3)
            };

            var deckFour_flashcardFour = new Flashcard()
            {
                Back = "back4",
                Front = "front4",
                DeckId = 4,
                Id = 4,
                Level = 1,
                NextReview = DateTime.Today.AddDays(3)
            };

            user.Decks.Add(deckOne);
            user.Decks.Add(deckTwo);
            user.Decks.Add(deckThree);
            user.Decks.Add(deckFour);

            deckOne.Flashcards.Add(deckOne_flashcardOne);
            deckOne.Flashcards.Add(deckOne_flashcardTwo);
            deckOne.Flashcards.Add(deckOne_flashcardThree);

            deckFour.Flashcards.Add(deckFour_flashcardOne);
            deckFour.Flashcards.Add(deckFour_flashcardTwo);
            deckFour.Flashcards.Add(deckFour_flashcardThree);
            deckFour.Flashcards.Add(deckFour_flashcardFour);

            var expectedValues = new ObservableCollection<WeightedPoint>();
            int i = 0;
            expectedValues.Add(new WeightedPoint(i, (int)DateTime.Today.DayOfWeek, 4));
            if ((int)DateTime.Today.DayOfWeek + 1 == 7) i++;
            expectedValues.Add(new WeightedPoint(i, (int)DateTime.Today.AddDays(1).DayOfWeek, 0));
            if ((int)DateTime.Today.AddDays(1).DayOfWeek + 1 == 7) i++;
            expectedValues.Add(new WeightedPoint(i, (int)DateTime.Today.AddDays(2).DayOfWeek, 1));
            if ((int)DateTime.Today.AddDays(2).DayOfWeek + 1 == 7) i++;
            expectedValues.Add(new WeightedPoint(i, (int)DateTime.Today.AddDays(3).DayOfWeek, 2));
            if ((int)DateTime.Today.AddDays(3).DayOfWeek + 1 == 7) i++;
            FillExpectedValues(i, expectedValues, DateTime.Today.AddDays(4));

            Axis[] expectedAxes = { new Axis { Labels = new List<string>() } };
            var firstDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            for (int j = 0; j <= 25; j++)
            {
                expectedAxes[0].Labels.Add(firstDate.ToString("d"));
                firstDate = firstDate.AddDays(7);
            }

            bool outcome = organizer.OrganizeFutureReviews(user);

            Assert.True(outcome);
            Assert.Single(organizer.FutureReviewsXAxes);
            Assert.Single(organizer.FutureReviewsSeries);

            for (i = 0; i < expectedValues.Count; i++)
            {
                Assert.Equal(expectedValues[i].X, organizer.FutureReviewsObservable[i].X);
                Assert.Equal(expectedValues[i].Y, organizer.FutureReviewsObservable[i].Y);
                Assert.Equal(expectedValues[i].Weight, organizer.FutureReviewsObservable[i].Weight);
            }

            for (i = 0; i < expectedAxes[0].Labels.Count; i++)
            {
                Assert.Equal(expectedAxes[0].Labels[i], organizer.FutureReviewsXAxes[0].Labels[i]);
            }
        }

        [Fact]
        public void OrganizeFutureReviews_DeckIsNull_ShouldThrowArgumentNullException()
        {
            var organizer = new FutureReviewsOrganizer(futureReviewsXAxes, futureReviewsSeries);

            Deck deck = null;

            Assert.Throws<ArgumentNullException>(() => organizer.OrganizeFutureReviews(deck));
        }

        [Fact]
        public void OrganizeFutureReviews_DeckHasNoFlashcards_ShouldReturnFalse()
        {
            var organizer = new FutureReviewsOrganizer(futureReviewsXAxes, futureReviewsSeries);

            var deckOne = new Deck()
            {
                Flashcards = new ObservableCollection<Flashcard>(),
                Id = 1,
                Name = "deck1",
                UserId = 1
            };

            var deckTwo = new Deck()
            {
                Id = 1,
                Name = "deck2",
                UserId = 1
            };

            Assert.False(organizer.OrganizeFutureReviews(deckOne));
            Assert.False(organizer.OrganizeFutureReviews(deckTwo));
        }

        [Fact]
        public void OrganizeFutureReviews_DeckWithFlashcards_ShouldReturnTrue()
        {
            var organizer = new FutureReviewsOrganizer(futureReviewsXAxes, futureReviewsSeries);

            var deck = new Deck()
            {
                Flashcards = new ObservableCollection<Flashcard>(),
                Id = 1,
                Name = "deck1",
                UserId = 1
            };

            var flashcardOne = new Flashcard()
            {
                Back = "back1",
                Front = "front1",
                DeckId = 1,
                Id = 1,
                Level = 0,
                NextReview = DateTime.Today.AddDays(-1)
            };

            var flashcardTwo = new Flashcard()
            {
                Back = "back2",
                Front = "front2",
                DeckId = 1,
                Id = 2,
                Level = 2,
                NextReview = DateTime.Today
            };

            var flashcardThree = new Flashcard()
            {
                Back = "back3",
                Front = "front3",
                DeckId = 1,
                Id = 3,
                Level = 1,
                NextReview = DateTime.Today.AddDays(2)
            };
            deck.Flashcards.Add(flashcardOne);
            deck.Flashcards.Add(flashcardTwo);
            deck.Flashcards.Add(flashcardThree);

            var expectedValues = new ObservableCollection<WeightedPoint>();
            int i = 0;
            expectedValues.Add(new WeightedPoint(i, (int)DateTime.Today.DayOfWeek, 2));
            if ((int)DateTime.Today.DayOfWeek + 1 == 7) i++;
            expectedValues.Add(new WeightedPoint(i, (int)DateTime.Today.AddDays(1).DayOfWeek, 0));
            if ((int)DateTime.Today.AddDays(1).DayOfWeek + 1 == 7) i++;
            expectedValues.Add(new WeightedPoint(i, (int)DateTime.Today.AddDays(2).DayOfWeek, 1));
            if ((int)DateTime.Today.AddDays(2).DayOfWeek + 1 == 7) i++;
            FillExpectedValues(i, expectedValues, DateTime.Today.AddDays(3));

            Axis[] expectedAxes = { new Axis { Labels = new List<string>() } };
            var firstDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            for (int j = 0; j <= 25; j++)
            {
                expectedAxes[0].Labels.Add(firstDate.ToString("d"));
                firstDate = firstDate.AddDays(7);
            }

            bool outcome = organizer.OrganizeFutureReviews(deck);

            Assert.True(outcome);
            Assert.Single(organizer.FutureReviewsXAxes);
            Assert.Single(organizer.FutureReviewsSeries);

            for (i = 0; i < expectedValues.Count; i++)
            {
                Assert.Equal(expectedValues[i].X, organizer.FutureReviewsObservable[i].X);
                Assert.Equal(expectedValues[i].Y, organizer.FutureReviewsObservable[i].Y);
                Assert.Equal(expectedValues[i].Weight, organizer.FutureReviewsObservable[i].Weight);
            }

            for (i = 0; i < expectedAxes[0].Labels.Count; i++)
            {
                Assert.Equal(expectedAxes[0].Labels[i], organizer.FutureReviewsXAxes[0].Labels[i]);
            }

        }

        private void FillExpectedValues(int weekNumber,
                                        ObservableCollection<WeightedPoint> expectedValues,
                                        DateTime currentDate)
        {
            int dayOfTheWeek = (int)currentDate.DayOfWeek;
            while (dayOfTheWeek <= 6)
            {
                expectedValues.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                currentDate = currentDate.AddDays(1);
            }
            weekNumber++;
            while (weekNumber <= 25)
            {
                 dayOfTheWeek = 0;
                expectedValues.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                expectedValues.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                expectedValues.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                expectedValues.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                expectedValues.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                expectedValues.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                expectedValues.Add(new WeightedPoint(weekNumber++, dayOfTheWeek++, 0));
                currentDate = currentDate.AddDays(6);
            }
        }
    }
}
