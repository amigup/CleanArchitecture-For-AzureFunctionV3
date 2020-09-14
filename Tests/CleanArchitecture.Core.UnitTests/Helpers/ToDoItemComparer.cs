using AZV3CleanArchitecture.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CleanArchitecture.Core.UnitTests.Helpers
{
    public class ToDoItemComparer : IEqualityComparer<ToDoItem>
    {
        public bool Equals([AllowNull] ToDoItem x, [AllowNull] ToDoItem y)
        {
            if (x != null && y != null)
            {
                return x.Id == y.Id
                && x.Title == y.Title
                && x.UserId == y.UserId
                && x.Completed == y.Completed;
            }

            return x == null && y == null;
        }

        public int GetHashCode([DisallowNull] ToDoItem obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
