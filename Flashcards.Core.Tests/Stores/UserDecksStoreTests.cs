using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Services.UserDataChangers;
using Flashcards.Core.Services.UserDataCreators;
using Flashcards.Core.Services.UserDataDestroyers;
using Flashcards.Core.Services.UserDataProviders;
using Flashcards.Core.Stores;
using Flashcards.Core.Tests.DbConnection;
using Flashcards.Core.ViewModels;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Stores
{
    public class UserDecksStoreTests
    {
        private readonly Mock<IUserDataProvider> _dataProviderMock;
        private readonly Mock<IUserDataCreator> _dataCreatorMock;
        private readonly Mock<IUserDataDestroyer> _dataDestroyerMock;
        private readonly Mock<IUserDataChanger> _dataChangerMock;
        private readonly SelectionStore _selectionStore;
        private readonly Mock<NavigationStore> _navigationStoreMock;
        private readonly NavigationService<UserIconViewModel> _rightNavService;
        private readonly NavigationService<HomeViewModel> _leftNavService;
        private readonly Mock<ObservableObject> _observableObjectMock;
        public UserDecksStoreTests()
        {
            _dataProviderMock = new Mock<IUserDataProvider>();
            _dataCreatorMock = new Mock<IUserDataCreator>();
            _dataDestroyerMock = new Mock<IUserDataDestroyer>();
            _dataChangerMock = new Mock<IUserDataChanger>();
            _selectionStore = new SelectionStore();
            _navigationStoreMock = new Mock<NavigationStore>();
            _observableObjectMock = new Mock<ObservableObject>();


            _rightNavService = new NavigationService<UserIconViewModel>(_navigationStoreMock.Object, () => null);
            _leftNavService = new NavigationService<HomeViewModel>(_navigationStoreMock.Object, () => null);
        }

        private List<User> PrepareData(UsersContext context)
        {
            User userOne = new User()
            {
                Name = "user1",
                Email = "user1@test",
                PasswordHash = "hash"
            };
            context.Users.Add(userOne);

            User userTwo = new User()
            {
                Name = "user2",
                Email = "user2@test",
                PasswordHash = "hash"
            };
            context.Users.Add(userTwo);

            User userThree = new User()
            {
                Name = "user3",
                Email = "user3@test",
                PasswordHash = "hash"
            };
            context.Users.Add(userThree);
            context.SaveChanges();


            Deck deckOne = new Deck()
            {
                Name = "deck1",
                UserId = userOne.Id
            };
            context.Decks.Add(deckOne);

            Deck deckTwo = new Deck()
            {
                Name = "deck2",
                UserId = userOne.Id
            };
            context.Decks.Add(deckTwo);

            Deck deckThree = new Deck()
            {
                Name = "deck3",
                UserId = userTwo.Id
            };
            context.Decks.Add(deckThree);
            context.SaveChanges();


            Flashcard flashcardOne = new Flashcard()
            {
                Front = "front1",
                Back = "back1",
                DeckId = deckOne.Id,
                Level = 0,
                NextReview = DateTime.Parse("2022-04-14")
            };
            context.Flashcards.Add(flashcardOne);

            Flashcard flashcardTwo = new Flashcard()
            {
                Front = "front2",
                Back = "back2",
                DeckId = deckOne.Id,
                Level = 0,
                NextReview = DateTime.Parse("2022-04-14")
            };
            context.Flashcards.Add(flashcardTwo);

            Flashcard flashcardThree = new Flashcard()
            {
                Front = "front3",
                Back = "back3",
                DeckId = deckTwo.Id,
                Level = 0,
                NextReview = DateTime.Parse("2022-04-14")
            };
            context.Flashcards.Add(flashcardThree);
            context.SaveChanges();

            DailyActivity activityOne = new DailyActivity()
            {
                Day = DateTime.Parse("2022-03-30"),
                ReviewedFlashcardsCount = 1,
                UserId = userOne.Id
            };
            context.DailyActivity.Add(activityOne);

            DailyActivity activityTwo = new DailyActivity()
            {
                Day = DateTime.Today,
                ReviewedFlashcardsCount = 1,
                UserId = userTwo.Id
            };
            context.DailyActivity.Add(activityTwo);

            DailyActivity activityThree = new DailyActivity()
            {
                Day = DateTime.Today.AddDays(-1),
                ReviewedFlashcardsCount = 1,
                UserId = userTwo.Id
            };
            context.DailyActivity.Add(activityThree);

            context.SaveChanges();

            return context.Users.ToList();
        }

        [Fact]
        public async void InitializeTest()
        {
            TestDbContextFactory _contextFactory = new TestDbContextFactory(nameof(InitializeTest));
            var context = _contextFactory.CreateDbContext();
            var users = PrepareData(context);
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 1))).Returns(Task.FromResult(users[0]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 2))).Returns(Task.FromResult(users[1]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 3))).Returns(Task.FromResult(users[2]));

            var store = new UserDecksStore(_dataProviderMock.Object, _dataCreatorMock.Object, _dataDestroyerMock.Object,
                                            _dataChangerMock.Object, _selectionStore, _navigationStoreMock.Object,
                                            _rightNavService, _leftNavService);

            await store.Initialize(users[0]);
            Assert.Equal(users[0], store.User);
            Assert.Equal(0, store.Streak);
            Assert.False(store.IfTodayActivity);

            await store.Initialize(users[1]);
            Assert.Equal(users[1], store.User);
            // need to test user model
            Assert.Equal(2, store.Streak);
            Assert.True(store.IfTodayActivity);
        }

        [Fact]
        public async void ChangeUserEmailTest_WithFalseOutcome_ShouldReturnFalse()
        {
            TestDbContextFactory _contextFactory = new TestDbContextFactory(nameof(ChangeUserEmailTest_WithFalseOutcome_ShouldReturnFalse));
            var context = _contextFactory.CreateDbContext();
            var users = PrepareData(context);
            _dataChangerMock.Setup(ch => ch.ChangeUserEmailAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(false));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 1))).Returns(Task.FromResult(users[0]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 2))).Returns(Task.FromResult(users[1]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 3))).Returns(Task.FromResult(users[2]));
            var store = new UserDecksStore(_dataProviderMock.Object, _dataCreatorMock.Object, _dataDestroyerMock.Object,
                                            _dataChangerMock.Object, _selectionStore, _navigationStoreMock.Object,
                                            _rightNavService, _leftNavService);
            await store.Initialize(users[0]);
            bool outcome = await store.ChangeUserEmail("changed@email");
            Assert.False(outcome);
            Assert.Equal("user1@test", store.User.Email);
        }

        [Fact]
        public async void ChangeUserEmailTest_WithTrueOutcome_ShouldReturnTrue()
        {
            TestDbContextFactory _contextFactory = new TestDbContextFactory(nameof(ChangeUserEmailTest_WithTrueOutcome_ShouldReturnTrue));
            var context = _contextFactory.CreateDbContext();
            var users = PrepareData(context);
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 1))).Returns(Task.FromResult(users[0]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 2))).Returns(Task.FromResult(users[1]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 3))).Returns(Task.FromResult(users[2]));
            _dataChangerMock.Setup(ch => ch.ChangeUserEmailAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            var store = new UserDecksStore(_dataProviderMock.Object, _dataCreatorMock.Object, _dataDestroyerMock.Object,
                                            _dataChangerMock.Object, _selectionStore, _navigationStoreMock.Object,
                                            _rightNavService, _leftNavService);
            await store.Initialize(users[0]);
            bool outcome = await store.ChangeUserEmail("changed@email");
            Assert.True(outcome);
            Assert.Equal("changed@email", store.User.Email);
        }

        [Fact]
        public async void ChangeUserNameTest_WithFalseOutcome_ShouldReturnFalse()
        {
            TestDbContextFactory _contextFactory = new TestDbContextFactory(nameof(ChangeUserNameTest_WithFalseOutcome_ShouldReturnFalse));
            var context = _contextFactory.CreateDbContext();
            var users = PrepareData(context);
            _dataChangerMock.Setup(ch => ch.ChangeUserNameAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(false));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 1))).Returns(Task.FromResult(users[0]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 2))).Returns(Task.FromResult(users[1]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 3))).Returns(Task.FromResult(users[2]));
            var store = new UserDecksStore(_dataProviderMock.Object, _dataCreatorMock.Object, _dataDestroyerMock.Object,
                                            _dataChangerMock.Object, _selectionStore, _navigationStoreMock.Object,
                                            _rightNavService, _leftNavService);
            await store.Initialize(users[0]);
            bool outcome = await store.ChangeUserName("changedUsername");
            Assert.False(outcome);
            Assert.Equal("user1", store.User.Name);
        }

        [Fact]
        public async void ChangeUserNameTest_WithTrueOutcome_ShouldReturnTrue()
        {
            TestDbContextFactory _contextFactory = new TestDbContextFactory(nameof(ChangeUserNameTest_WithTrueOutcome_ShouldReturnTrue));
            var context = _contextFactory.CreateDbContext();
            var users = PrepareData(context);
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 1))).Returns(Task.FromResult(users[0]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 2))).Returns(Task.FromResult(users[1]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 3))).Returns(Task.FromResult(users[2]));
            _dataChangerMock.Setup(ch => ch.ChangeUserNameAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            var store = new UserDecksStore(_dataProviderMock.Object, _dataCreatorMock.Object, _dataDestroyerMock.Object,
                                            _dataChangerMock.Object, _selectionStore, _navigationStoreMock.Object,
                                            _rightNavService, _leftNavService);
            await store.Initialize(users[0]);
            bool outcome = await store.ChangeUserName("changedUsername");
            Assert.True(outcome);
            Assert.Equal("changedUsername", store.User.Name);
        }

        [Fact]
        public async void LogOutUserTest()
        {
            TestDbContextFactory _contextFactory = new TestDbContextFactory(nameof(LogOutUserTest));
            var context = _contextFactory.CreateDbContext();
            var users = PrepareData(context);
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 1))).Returns(Task.FromResult(users[0]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 2))).Returns(Task.FromResult(users[1]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 3))).Returns(Task.FromResult(users[2]));
            _navigationStoreMock.SetupAllProperties();
            

            var store = new UserDecksStore(_dataProviderMock.Object, _dataCreatorMock.Object, _dataDestroyerMock.Object,
                                            _dataChangerMock.Object, _selectionStore, _navigationStoreMock.Object,
                                            _rightNavService, _leftNavService);
            await store.Initialize(users[0]);
            _selectionStore.SelectedDeck = users[0].Decks[0];
            _selectionStore.SelectedFlashcard = users[0].Decks[0].Flashcards[0];

            store.LogOutUser();
            Assert.Null(store.User);
            Assert.Null(store.SelectionStore.SelectedDeck);
            Assert.Null(store.SelectionStore.SelectedFlashcard);
            Assert.Null(_navigationStoreMock.Object.LeftViewModel);
            Assert.Null(_navigationStoreMock.Object.RightViewModel);
        }

        [Fact]
        public async void AlterFlashcardTest()
        {
            TestDbContextFactory _contextFactory = new TestDbContextFactory(nameof(AlterFlashcardTest));
            var context = _contextFactory.CreateDbContext();
            var users = PrepareData(context);
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 1))).Returns(Task.FromResult(users[0]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 2))).Returns(Task.FromResult(users[1]));
            _dataProviderMock.Setup(p => p.LoadUserDecksAsync(It.Is<int>(i => i == 3))).Returns(Task.FromResult(users[2]));
            _dataChangerMock.Setup(p => p.ChangeFlashcard(It.IsAny<Flashcard>())).Verifiable();
            var store = new UserDecksStore(_dataProviderMock.Object, _dataCreatorMock.Object, _dataDestroyerMock.Object,
                                            _dataChangerMock.Object, _selectionStore, _navigationStoreMock.Object,
                                            _rightNavService, _leftNavService);
            await store.Initialize(users[0]);
            _selectionStore.SelectedDeck = users[0].Decks[0];
            _selectionStore.SelectedFlashcard = users[0].Decks[0].Flashcards[0];
            string front = "new front text";
            string back = "new back text";
            await store.AlterFlashcard(front, back);
            Assert.Equal(front, _selectionStore.SelectedFlashcard.Front);
            Assert.Equal(back, _selectionStore.SelectedFlashcard.Back);
            Assert.Equal(front, store.User.Decks[0].Flashcards[0].Front);
            Assert.Equal(back, store.User.Decks[0].Flashcards[0].Back);
            _dataChangerMock.VerifyAll();
        }
    }
}
