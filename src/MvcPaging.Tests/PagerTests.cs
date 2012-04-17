using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections;

namespace MvcPaging.Tests
{
    [TestFixture]
    public class PagerTests
    {

        internal class PagionationComparer : IComparer
        {

            public int Compare(object x, object y)
            {
                PaginationModel first = (PaginationModel)x;
                PaginationModel second = (PaginationModel)y;
           
                var displayTextResult = first.DisplayText.CompareTo(second.DisplayText);
                if (displayTextResult != 0) return displayTextResult;

                if (!first.PageIndex.HasValue && second.PageIndex.HasValue) return -1;
                if (first.PageIndex.HasValue && !second.PageIndex.HasValue) return 1;
                if (first.PageIndex.HasValue && second.PageIndex.HasValue)
                {
                    var pageIndexResult = first.PageIndex.Value.CompareTo(second.PageIndex.Value);
                    if (pageIndexResult != 0) return pageIndexResult;
                }
                var activeResult = first.Active.CompareTo(second.Active);
                if (activeResult != 0) return activeResult;
                var isCurrentResult = first.IsCurrent.CompareTo(second.IsCurrent);
                if (isCurrentResult != 0) return isCurrentResult;

                return 0;
            }
        }

        [Test]
        public void Can_Build_Correct_Model_For_5_Items_With_2_Item_Per_Page()
        {
            // Assemble
            var pager = new Pager(null, 2, 1, 5, null, null);
            var expectedPagination = new List<PaginationModel>()
            {
                new PaginationModel { Active = false, DisplayText = "«" },
                new PaginationModel { Active = true, DisplayText = "1", PageIndex = 1, IsCurrent = true },
                new PaginationModel { Active = true, DisplayText = "2", PageIndex = 2 },
                new PaginationModel { Active = true, DisplayText = "3", PageIndex = 3 },
                new PaginationModel { Active = true, DisplayText = "»", PageIndex = 2 }
            };

            // Act
            var result =  pager.BuildPaginationModel();

            // Assert
            
            Assert.AreEqual(expectedPagination.Count, result.Count);
            CollectionAssert.AreEqual(expectedPagination, result, new PagionationComparer());
        }

        [Test]
        public void Can_Build_Correct_Model_For_10_Items_With_2_Item_Per_Page()
        {
            // Assemble
            var pager = new Pager(null, 2, 3, 10, null, null);
            var expectedPagination = new List<PaginationModel>()
            {
                new PaginationModel { Active = true, DisplayText = "«", PageIndex = 2 },
                new PaginationModel { Active = true, DisplayText = "1", PageIndex = 1 },
                new PaginationModel { Active = true, DisplayText = "2", PageIndex = 2 },
                new PaginationModel { Active = true, DisplayText = "3", PageIndex = 3, IsCurrent = true },
                new PaginationModel { Active = true, DisplayText = "4", PageIndex = 4 },
                new PaginationModel { Active = true, DisplayText = "5", PageIndex = 5 },
                new PaginationModel { Active = true, DisplayText = "»", PageIndex = 4 }
            };

            // Act
            var result = pager.BuildPaginationModel();

            // Assert

            Assert.AreEqual(expectedPagination.Count, result.Count);
            CollectionAssert.AreEqual(expectedPagination, result, new PagionationComparer());
        }


        [Test]
        public void Can_Build_Correct_Model_For_33_Items_With_2_Item_Per_Page()
        {
            // Assemble
            var pager = new Pager(null, 2, 13, 33, null, null);
            var expectedPagination = new List<PaginationModel>()
            {
                new PaginationModel { Active = true, DisplayText = "«", PageIndex = 12 },
                new PaginationModel { Active = true, DisplayText = "1", PageIndex = 1 },
                new PaginationModel { Active = true, DisplayText = "2", PageIndex = 2 },
                new PaginationModel { Active = true, DisplayText = "..." },
                new PaginationModel { Active = true, DisplayText = "8", PageIndex = 8 },
                new PaginationModel { Active = true, DisplayText = "9", PageIndex = 9 },
                new PaginationModel { Active = true, DisplayText = "10", PageIndex = 10 },
                new PaginationModel { Active = true, DisplayText = "11", PageIndex = 11 },
                new PaginationModel { Active = true, DisplayText = "12", PageIndex = 12 },
                new PaginationModel { Active = true, DisplayText = "13", PageIndex = 13, IsCurrent = true },
                new PaginationModel { Active = true, DisplayText = "14", PageIndex = 14 },
                new PaginationModel { Active = true, DisplayText = "15", PageIndex = 15 },
                new PaginationModel { Active = true, DisplayText = "16", PageIndex = 16 },
                new PaginationModel { Active = true, DisplayText = "17", PageIndex = 17 },                
                new PaginationModel { Active = true, DisplayText = "»", PageIndex = 14 }
            };

            // Act
            var result = pager.BuildPaginationModel();

            // Assert

            Assert.AreEqual(expectedPagination.Count, result.Count);
            CollectionAssert.AreEqual(expectedPagination, result, new PagionationComparer());
        }

    }
}