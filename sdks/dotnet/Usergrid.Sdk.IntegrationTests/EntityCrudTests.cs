using System;
using NUnit.Framework;
using Usergrid.Sdk.Model;

namespace Usergrid.Sdk.IntegrationTests {
    [TestFixture]
    public class EntityCrudTests : BaseTest {
        private IClient _client;
        [SetUp]
        public void Setup() {
            _client = InitializeClientAndLogin(AuthType.Organization);
        }

        [Test]
        public void ShouldCrudPocoEntity() {
            const string collectionName = "friends";
            var friend = new Friend
                {
                    Name = "EntityName",
                    Age = 25
                };

            DeleteEntityIfExists<Friend>(_client, collectionName, friend.Name);

            // Create a new entity
            _client.CreateEntity(collectionName, friend);

            // Get it back
            var friendFromUsergrid = _client.GetEntity<Friend>(collectionName, friend.Name);

            // Assert that the entity returned is correct
            Assert.IsNotNull(friendFromUsergrid);
            Assert.AreEqual(friend.Name, friendFromUsergrid.Name);
            Assert.AreEqual(friend.Age, friendFromUsergrid.Age);

            // Get it back with query
            string query = "select * where name = '" + friend.Name + "'";
            UsergridCollection<Friend> friends = _client.GetEntities<Friend>(collectionName, query: query);

            // Assert the collection is correct
            Assert.IsNotNull(friends);
            Assert.AreEqual(1, friends.Count);
            Assert.IsFalse(friends.HasNext);
            Assert.IsFalse(friends.HasPrevious);
            friendFromUsergrid = friends[0];
            Assert.IsNotNull(friendFromUsergrid);
            Assert.AreEqual(friend.Name, friendFromUsergrid.Name);
            Assert.AreEqual(friend.Age, friendFromUsergrid.Age);


            // Update the entity
            Friend friendToUpdate = friendFromUsergrid;
            friendToUpdate.Age = 30;
            _client.UpdateEntity(collectionName, friendToUpdate.Name, friendToUpdate);

            // Get it back
            friendFromUsergrid = _client.GetEntity<Friend>(collectionName, friendToUpdate.Name);

            // Assert that entity is updated
            Assert.AreEqual(friendToUpdate.Age, friendFromUsergrid.Age);

            // Delete the entity
            _client.DeleteEntity(collectionName, friend.Name);

            // Get it back
            friendFromUsergrid = _client.GetEntity<Friend>(collectionName, friend.Name);

            // Assert that it doesn't exist
            Assert.IsNull(friendFromUsergrid);
        }

        [Test]
        public void ShouldCrudUserGridEntity() {
            const string collectionName = "friends";
            var friend = new UsergridFriend
                {
                    Name = "EntityName",
                    Age = 25
                };

            DeleteEntityIfExists<Friend>(_client, collectionName, friend.Name);

            // Create a new entity
            _client.CreateEntity(collectionName, friend);

            // Get it back
            var friendFromUsergrid = _client.GetEntity<UsergridFriend>(collectionName, friend.Name);

            // Assert that the entity returned is correct
            Assert.IsNotNull(friendFromUsergrid);
            Assert.IsNotEmpty(friendFromUsergrid.Uuid);
            Assert.IsNotEmpty(friendFromUsergrid.Type);
            Assert.IsTrue(friendFromUsergrid.CreatedDate > DateTime.MinValue);
            Assert.IsTrue(friendFromUsergrid.ModifiedDate > DateTime.MinValue);
            Assert.AreEqual(friend.Name, friendFromUsergrid.Name);
            Assert.AreEqual(friend.Age, friendFromUsergrid.Age);

            // Get it back with query
            string query = "select * where name = '" + friend.Name + "'";
            UsergridCollection<UsergridFriend> friends = _client.GetEntities<UsergridFriend>(collectionName, query: query);

            // Assert the collection is correct
            Assert.IsNotNull(friends);
            Assert.AreEqual(1, friends.Count);
            Assert.IsFalse(friends.HasNext);
            Assert.IsFalse(friends.HasPrevious);
            friendFromUsergrid = friends[0];
            Assert.IsNotNull(friendFromUsergrid);
            Assert.AreEqual(friend.Name, friendFromUsergrid.Name);
            Assert.AreEqual(friend.Age, friendFromUsergrid.Age);
            Assert.IsNotEmpty(friendFromUsergrid.Uuid);
            Assert.IsNotEmpty(friendFromUsergrid.Type);
            Assert.IsTrue(friendFromUsergrid.CreatedDate > DateTime.MinValue);
            Assert.IsTrue(friendFromUsergrid.ModifiedDate > DateTime.MinValue);


            // Update the entity
            UsergridFriend friendToUpdate = friendFromUsergrid;
            friendToUpdate.Age = 30;
            _client.UpdateEntity(collectionName, friendToUpdate.Name, friendToUpdate);

            // Get it back
            friendFromUsergrid = _client.GetEntity<UsergridFriend>(collectionName, friendToUpdate.Name);

            // Assert that entity is updated
            Assert.AreEqual(friendToUpdate.Age, friendFromUsergrid.Age);

            // Delete the entity
            _client.DeleteEntity(collectionName, friend.Name);

            // Get it back
            friendFromUsergrid = _client.GetEntity<UsergridFriend>(collectionName, friend.Name);

            // Assert that it doesn't exist
            Assert.IsNull(friendFromUsergrid);
        }
    }
}