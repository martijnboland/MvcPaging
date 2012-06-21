using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Collections;

namespace MvcPaging.Tests
{
	[TestFixture]
	public class PagerTests
	{

		internal class PaginationComparer : IComparer
		{

			public int Compare(object x, object y)
			{
				var first = (PaginationLink)x;
				var second = (PaginationLink)y;

				var displayTextResult = first.DisplayText.CompareTo(second.DisplayText);
				if (displayTextResult != 0) return displayTextResult;

				if (first.Url == null && second.Url != null) return -1;
				if (first.Url != null && second.Url == null) return 1;
				if (first.Url != null && second.Url != null)
				{
					var urlResult = first.Url.CompareTo(second.Url);
					if (urlResult != 0) return urlResult;
				}
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
		public void Can_Action_Be_Set_Before_RouteData()
		{
			const string ACTION_NAME = "testaction";
			// Assemble
			var pager = new Pager(null, 2, 1, 5).Options(o => o.Action(ACTION_NAME).RouteValues(new { a = "b" }));

			// Act
			var result = pager.BuildPaginationModel(BuildUrl);

			// Assert
			Assert.AreEqual(result.Options.Action, ACTION_NAME);
			Assert.AreEqual(result.Options.RouteValues["action"], ACTION_NAME);
		}

		[Test]
		public void Is_Action_Overriden()
		{
			const string ACTION_NAME = "testaction";
			const string ACTION_NAME_2 = "testaction2";
			// Assemble
			var pager = new Pager(null, 2, 1, 5).Options(o => o.Action(ACTION_NAME).RouteValues(new { a = "b", action = ACTION_NAME_2 }));

			// Act
			var result = pager.BuildPaginationModel(BuildUrl);

			// Assert
			Assert.AreEqual(result.Options.Action, ACTION_NAME);
			Assert.AreEqual(result.Options.RouteValues["action"], ACTION_NAME_2);
		}

		[Test]
		public void Can_Build_Correct_Model_For_5_Items_With_2_Item_Per_Page()
		{
			// Assemble
			var pager = new Pager(null, 2, 1, 5);
			var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = false, DisplayText = "«", Url = null },
                new PaginationLink { Active = true, DisplayText = "1", PageIndex = 1, IsCurrent = true, Url = null },
                new PaginationLink { Active = true, DisplayText = "2", PageIndex = 2, Url = "/test/2" },
                new PaginationLink { Active = true, DisplayText = "3", PageIndex = 3, Url = "/test/3" },
                new PaginationLink { Active = true, DisplayText = "»", PageIndex = 2, Url = "/test/2" }
            };

			// Act
			var result = pager.BuildPaginationModel(BuildUrl);

			// Assert
			Assert.AreEqual(expectedPagination.Count, result.PaginationLinks.Count());
			CollectionAssert.AreEqual(expectedPagination, result.PaginationLinks, new PaginationComparer());
		}

		[Test]
		public void Can_Build_Correct_Model_For_10_Items_With_2_Item_Per_Page()
		{
			// Assemble
			var pager = new Pager(null, 2, 3, 10);
			var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = true, DisplayText = "«", PageIndex = 2, Url = "/test/2" },
                new PaginationLink { Active = true, DisplayText = "1", PageIndex = 1, Url = "/test/1" },
                new PaginationLink { Active = true, DisplayText = "2", PageIndex = 2, Url = "/test/2" },
                new PaginationLink { Active = true, DisplayText = "3", PageIndex = 3, IsCurrent = true, Url = null },
                new PaginationLink { Active = true, DisplayText = "4", PageIndex = 4, Url = "/test/4" },
                new PaginationLink { Active = true, DisplayText = "5", PageIndex = 5, Url = "/test/5" },
                new PaginationLink { Active = true, DisplayText = "»", PageIndex = 4, Url = "/test/4" }
            };

			// Act
			var result = pager.BuildPaginationModel(BuildUrl);

			// Assert
			Assert.AreEqual(expectedPagination.Count, result.PaginationLinks.Count());
			CollectionAssert.AreEqual(expectedPagination, result.PaginationLinks, new PaginationComparer());
		}


		[Test]
		public void Can_Build_Correct_Model_For_33_Items_With_2_Item_Per_Page()
		{
			// Assemble
			var pager = new Pager(null, 2, 13, 33);
			var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = true, DisplayText = "«", PageIndex = 12, Url = "/test/12" },
                new PaginationLink { Active = true, DisplayText = "1", PageIndex = 1, Url = "/test/1" },
                new PaginationLink { Active = true, DisplayText = "2", PageIndex = 2, Url = "/test/2" },
                new PaginationLink { Active = true, DisplayText = "...", Url = null },
                new PaginationLink { Active = true, DisplayText = "8", PageIndex = 8, Url = "/test/8" },
                new PaginationLink { Active = true, DisplayText = "9", PageIndex = 9, Url = "/test/9" },
                new PaginationLink { Active = true, DisplayText = "10", PageIndex = 10, Url = "/test/10" },
                new PaginationLink { Active = true, DisplayText = "11", PageIndex = 11, Url = "/test/11" },
                new PaginationLink { Active = true, DisplayText = "12", PageIndex = 12, Url = "/test/12" },
                new PaginationLink { Active = true, DisplayText = "13", PageIndex = 13, IsCurrent = true, Url = null },
                new PaginationLink { Active = true, DisplayText = "14", PageIndex = 14, Url = "/test/14" },
                new PaginationLink { Active = true, DisplayText = "15", PageIndex = 15, Url = "/test/15" },
                new PaginationLink { Active = true, DisplayText = "16", PageIndex = 16, Url = "/test/16" },
                new PaginationLink { Active = true, DisplayText = "17", PageIndex = 17, Url = "/test/17" },                
                new PaginationLink { Active = true, DisplayText = "»", PageIndex = 14, Url = "/test/14" }
            };

			// Act
			var result = pager.BuildPaginationModel(BuildUrl);

			// Assert
			Assert.AreEqual(expectedPagination.Count, result.PaginationLinks.Count());
			CollectionAssert.AreEqual(expectedPagination, result.PaginationLinks, new PaginationComparer());
		}

		[Test]
		public void Can_Build_Correct_Model_For_33_Items_With_2_Item_Per_Page_And_Max_5_Pages()
		{
			// Assemble
			var pager = new Pager(null, 2, 1, 33).Options(o => o.MaxNrOfPages(5));
			var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = false, DisplayText = "«", Url = null },
                new PaginationLink { Active = true, DisplayText = "1", PageIndex = 1, IsCurrent = true, Url = null },
                new PaginationLink { Active = true, DisplayText = "2", PageIndex = 2, Url = "/test/2"},
                new PaginationLink { Active = true, DisplayText = "3", PageIndex = 3, Url = "/test/3" },
				new PaginationLink { Active = true, DisplayText = "4", PageIndex = 4, Url = "/test/4" },
				new PaginationLink { Active = true, DisplayText = "5", PageIndex = 5, Url = "/test/5" },
				new PaginationLink { Active = true, DisplayText = "...", Url = null },
				new PaginationLink { Active = true, DisplayText = "16", PageIndex = 16, Url = "/test/16" },
                new PaginationLink { Active = true, DisplayText = "17", PageIndex = 17, Url = "/test/17" }, 
                new PaginationLink { Active = true, DisplayText = "»", PageIndex = 2, Url = "/test/2" }
            };

			// Act
			var result = pager.BuildPaginationModel(BuildUrl);

			// Assert
			Assert.AreEqual(expectedPagination.Count, result.PaginationLinks.Count());
			CollectionAssert.AreEqual(expectedPagination, result.PaginationLinks, new PaginationComparer());
		}

		private string BuildUrl(int pageNumber)
		{
			return string.Format("/test/{0}", pageNumber);
		}
	}
}