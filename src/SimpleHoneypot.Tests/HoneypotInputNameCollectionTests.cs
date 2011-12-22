using System;
using System.Linq;
using SimpleHoneypot.Core;
using Xunit;

namespace SimpleHoneypot.Tests {
    public class HoneypotInputNameCollectionTests {
        [Fact]
        public void Add_WillPlaceStringInCollection_WhenPassedAValidString() {
            string str = "Mock String";
            var collection = new HoneypotInputNameCollection();

            collection.Add(str);

            string filterName = collection.First();
            Assert.Equal(1, collection.Count);
            Assert.Equal(str, filterName);
        }

        [Fact]
        public void Add_ShouldThorwArgumentException_WhenPassedEmptyString() {
            string str = string.Empty;
            var collection = new HoneypotInputNameCollection();

            Assert.Throws<ArgumentException>(() => collection.Add(str));
        }

        [Fact]
        public void Add_ShouldThorwArgumentException_WhenPassedNull() {
            string str = null;
            var collection = new HoneypotInputNameCollection();

            Assert.Throws<ArgumentException>(() => collection.Add(str));
        }

        [Fact]
        public void Add_ShouldAddAllStringsInCollection_WhenPassedAnyInitializedIEnumerableOfString() {
            var arr = new[] {"Mock 1", "Mock 2", "Mock 3"};
            var collection = new HoneypotInputNameCollection();

            collection.Add(arr);

            Assert.Equal(3, collection.Count);
        }

        [Fact]
        public void Remove_ShouldRemoveGivenStringFromCollection_WhenPassedAValidString() {
            string str = "Mock String";
            var collection = new HoneypotInputNameCollection();

            collection.Add(str);
            Assert.Equal(1, collection.Count);

            collection.Remove(str);
            Assert.Equal(0, collection.Count);
        }

        [Fact]
        public void Remove_ShouldThorwArgumentException_WhenPassedEmptyString() {
            string str = string.Empty;
            var collection = new HoneypotInputNameCollection();

            Assert.Throws<ArgumentException>(() => collection.Remove(str));
        }

        [Fact]
        public void Remove_ShouldThorwArgumentException_WhenPassedNull() {
            string str = null;
            var collection = new HoneypotInputNameCollection();

            Assert.Throws<ArgumentException>(() => collection.Remove(str));
        }

        [Fact]
        public void Clear_ShouldRemoveAllEntriesInCollection() {
            var arr = new[] {"Mock 1", "Mock 2", "Mock 3"};
            var collection = new HoneypotInputNameCollection();

            collection.Add(arr);
            Assert.Equal(3, collection.Count);

            collection.Clear();
            Assert.Equal(0, collection.Count);
        }
    }
}