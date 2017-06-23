﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Collections;
using System.Web.Mvc;
using Moq;

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
        public void Can_Defaults_Be_Set()
        {
            var first = new PagerOptions();
            PagerOptions.Defaults.AlwaysAddFirstPageNumber = !PagerOptions.DefaultDefaults.AlwaysAddFirstPageNumber;
            PagerOptions.Defaults.DisplayTemplate = PagerOptions.DefaultDefaults.DisplayTemplate + "-test";
            PagerOptions.Defaults.MaxNrOfPages = PagerOptions.DefaultDefaults.MaxNrOfPages + 1;

            var second = new PagerOptions();

            Assert.AreEqual(second.AlwaysAddFirstPageNumber, PagerOptions.Defaults.AlwaysAddFirstPageNumber);
            Assert.AreEqual(second.DisplayTemplate, PagerOptions.Defaults.DisplayTemplate);
            Assert.AreEqual(second.MaxNrOfPages, PagerOptions.Defaults.MaxNrOfPages);

            Assert.AreNotEqual(first.AlwaysAddFirstPageNumber, second.AlwaysAddFirstPageNumber);
            Assert.AreNotEqual(first.DisplayTemplate, second.DisplayTemplate);
            Assert.AreNotEqual(first.MaxNrOfPages, second.MaxNrOfPages);

            // cleanup
            PagerOptions.Defaults.Reset();

            Assert.AreEqual(PagerOptions.DefaultDefaults.AlwaysAddFirstPageNumber, PagerOptions.Defaults.AlwaysAddFirstPageNumber);
            Assert.AreEqual(PagerOptions.DefaultDefaults.DisplayTemplate, PagerOptions.Defaults.DisplayTemplate);
            Assert.AreEqual(PagerOptions.DefaultDefaults.MaxNrOfPages, PagerOptions.Defaults.MaxNrOfPages);
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
        public void Can_Action_And_Controller_Be_Set_Before_RouteData()
        {
            const string ACTION_NAME = "testaction";
            const string CONTROLLER_NAME = "testcontroller";
            
            //arrange
            var pager = new Pager(null, 2, 1, 5).Options(o => o.Action(ACTION_NAME, CONTROLLER_NAME).RouteValues(new { a = "b" }));

            //act
            var result = pager.BuildPaginationModel(BuildUrl);

            //assert
            Assert.AreEqual(result.Options.Action, ACTION_NAME);
            Assert.AreEqual(result.Options.Controller, CONTROLLER_NAME);

            Assert.AreEqual(result.Options.RouteValues["action"], ACTION_NAME);
            Assert.AreEqual(result.Options.RouteValues["controller"], CONTROLLER_NAME);
        }

        [Test]
        public void Is_Action_And_Controller_Overriden()
        {
            const string ACTION_NAME = "testaction";
            const string ACTION_NAME_2 = "testaction2";

            const string CONTROLLER_NAME   = "testcontroller";
            const string CONTROLLER_NAME_2 = "testcontroller2";

            // arrange
            var pager = new Pager(null, 2, 1, 5).Options(o => o.Action(ACTION_NAME, CONTROLLER_NAME).RouteValues(new { a = "b", action = ACTION_NAME_2, controller = CONTROLLER_NAME_2 }));

            // Act
            var result = pager.BuildPaginationModel(BuildUrl);

            // Assert
            Assert.AreEqual(result.Options.Action, ACTION_NAME);
            Assert.AreEqual(result.Options.RouteValues["action"], ACTION_NAME_2);

            Assert.AreEqual(result.Options.Controller, CONTROLLER_NAME);
            Assert.AreEqual(result.Options.RouteValues["controller"], CONTROLLER_NAME_2);
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
                new PaginationLink { Active = false, DisplayText = "...", Url = null, IsSpacer = true },
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
                new PaginationLink { Active = false, DisplayText = "...", Url = null, IsSpacer = true },
                new PaginationLink { Active = true, DisplayText = "17", PageIndex = 17, Url = "/test/17" },
                new PaginationLink { Active = true, DisplayText = "»", PageIndex = 2, Url = "/test/2" }
            };

            // Act
            var result = pager.BuildPaginationModel(BuildUrl);

            // Assert
            Assert.AreEqual(expectedPagination.Count, result.PaginationLinks.Count());
            CollectionAssert.AreEqual(expectedPagination, result.PaginationLinks, new PaginationComparer());
        }

        [Test]
        public void Are_Pager_Propeties_Properly_Propagated()
        {
            // Arrange
            var pager = new Pager(null, 10, 3, 158);

            // Act
            var model = pager.BuildPaginationModel(BuildUrl);

            // Assert
            Assert.AreEqual(model.PageSize, 10);
            Assert.AreEqual(model.CurrentPage, 3);
            Assert.AreEqual(model.TotalItemCount, 158);
            Assert.AreEqual(model.PageCount, 16);
        }

        [Test]
        public void Can_Add_RouteData_Values_With_StrongTyping()
        {
            // Arrange
            var model = new TestModel { Foo = "bar" };
            var htmlHelper = GetHtmlHelperWithModel(model);
            var pager = new Pager<TestModel>(htmlHelper, 10, 3, 158);

            // Act
            pager = pager.Options(o => o.AddRouteValueFor(m => m.Foo));

            var result = pager.BuildPaginationModel(BuildUrl);

            // Assert
            Assert.That(result.Options.RouteValues.ContainsKey("Foo"));
            Assert.AreEqual(result.Options.RouteValues["Foo"], "bar");
        }

        [Test]
        public void Can_Add_RouteData_Values_With_StrongTyping_Nested()
        {
            // Arrange
            var model = new TestModel { Nested = new Nested { X = 34, Y = 21 } };
            var htmlHelper = GetHtmlHelperWithModel(model);
            var pager = new Pager<TestModel>(htmlHelper, 10, 3, 158);

            // Act
            pager = pager.Options(o => o.AddRouteValueFor(m => m.Nested.X));
            pager = pager.Options(o => o.AddRouteValueFor(m => m.Nested.Y));

            var result = pager.BuildPaginationModel(BuildUrl);

            // Assert
            Assert.That(result.Options.RouteValues.ContainsKey("Nested.X"));
            Assert.That(result.Options.RouteValues.ContainsKey("Nested.Y"));
            Assert.AreEqual(result.Options.RouteValues["Nested.X"], 34);
            Assert.AreEqual(result.Options.RouteValues["Nested.Y"], 21);
        }

        [Test]
        public void When_current_page_is_not_set_the_first_link_should_be_current()
        {
            // Arrange
            var pager = new Pager(null, 10, 0, 158);

            // Act
            var result = pager.BuildPaginationModel(BuildUrl);

            // Assert
            Assert.That(result.PaginationLinks.First(pl => pl.PageIndex != null).IsCurrent);
        }

        [Test]
        public void When_current_page_is_set_to_one_the_first_link_should_be_current()
        {
            // Arrange
            var pager = new Pager(null, 10, 1, 158);

            // Act
            var result = pager.BuildPaginationModel(BuildUrl);

            // Assert
            Assert.That(result.PaginationLinks.First(pl => pl.PageIndex != null).IsCurrent);
        }

        [Test]
        public void Can_Build_Correct_Model_With_Custom_Previous_And_Next_Text()
        {
            // Assemble
            var pager = new Pager(null, 2, 1, 5);
            pager.Options(o => o.SetPreviousPageText("Prev").SetNextPageText("Next"));
            var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = false, DisplayText = "Prev", Url = null },
                new PaginationLink { Active = true, DisplayText = "1", PageIndex = 1, IsCurrent = true, Url = null },
                new PaginationLink { Active = true, DisplayText = "2", PageIndex = 2, Url = "/test/2" },
                new PaginationLink { Active = true, DisplayText = "3", PageIndex = 3, Url = "/test/3" },
                new PaginationLink { Active = true, DisplayText = "Next", PageIndex = 2, Url = "/test/2" }
            };

            // Act
            var result = pager.BuildPaginationModel(BuildUrl);

            // Assert
            Assert.AreEqual(expectedPagination.Count, result.PaginationLinks.Count());
            CollectionAssert.AreEqual(expectedPagination, result.PaginationLinks, new PaginationComparer());
        }

        [Test]
        public void Can_Build_Correct_Model_With_One_Page_Using_ItemCount_As_PageCount()
        {
            // Assemble
            var pager = new Pager(null, 10, 1, 1);
            pager.Options(o => o.UseItemCountAsPageCount());
            var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = false, DisplayText = "«", Url = null },
                new PaginationLink { Active = true, DisplayText = "1", PageIndex = 1, IsCurrent = true, Url = null },
                new PaginationLink { Active = false, DisplayText = "»", Url = null }
            };

            // Act
            var result = pager.BuildPaginationModel(BuildUrl);

            // Assert
            Assert.AreEqual(expectedPagination.Count, result.PaginationLinks.Count());
            Assert.AreEqual(10, result.TotalItemCount);
            CollectionAssert.AreEqual(expectedPagination, result.PaginationLinks, new PaginationComparer());
        }

        [Test]
        public void Can_Build_Correct_Model_With_Three_Pages_Using_ItemCount_As_PageCount()
        {
            // Assemble
            var pager = new Pager(null, 10, 1, 3);
            pager.Options(o => o.UseItemCountAsPageCount());
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
            Assert.AreEqual(30, result.TotalItemCount);
            CollectionAssert.AreEqual(expectedPagination, result.PaginationLinks, new PaginationComparer());
        }

        [Test]
        public void Can_Build_Correct_Model_With_Thirteen_Pages_Using_ItemCount_As_PageCount_And_Max_5_Pages()
        {
            // Assemble
            var pager = new Pager(null, 10, 1, 13).Options(o => o.MaxNrOfPages(5));
            pager.Options(o => o.UseItemCountAsPageCount());
            var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = false, DisplayText = "«", Url = null },
                new PaginationLink { Active = true, DisplayText = "1", PageIndex = 1, IsCurrent = true, Url = null },
                new PaginationLink { Active = true, DisplayText = "2", PageIndex = 2, Url = "/test/2"},
                new PaginationLink { Active = true, DisplayText = "3", PageIndex = 3, Url = "/test/3" },
                new PaginationLink { Active = true, DisplayText = "4", PageIndex = 4, Url = "/test/4" },
                new PaginationLink { Active = true, DisplayText = "5", PageIndex = 5, Url = "/test/5" },
                new PaginationLink { Active = false, DisplayText = "...", Url = null, IsSpacer = true },
                new PaginationLink { Active = true, DisplayText = "13", PageIndex = 13, Url = "/test/13" },
                new PaginationLink { Active = true, DisplayText = "»", PageIndex = 2, Url = "/test/2" }
            };

            // Act
            var result = pager.BuildPaginationModel(BuildUrl);

            // Assert
            Assert.AreEqual(expectedPagination.Count, result.PaginationLinks.Count());
            Assert.AreEqual(130, result.TotalItemCount);
            CollectionAssert.AreEqual(expectedPagination, result.PaginationLinks, new PaginationComparer());
        }

        [Test]
        public void Can_Build_Correct_Model_With_6_Pages_And_Max_5_Pages_And_DisplayLastPage_False()
        {
            // Assemble
            var pager = new Pager(null, 20, 1, 105).Options(o => o.MaxNrOfPages(5));
            var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = false, DisplayText = "«", Url = null },
                new PaginationLink { Active = true, DisplayText = "1", PageIndex = 1, IsCurrent = true, Url = null },
                new PaginationLink { Active = true, DisplayText = "2", PageIndex = 2, Url = "/test/2"},
                new PaginationLink { Active = true, DisplayText = "3", PageIndex = 3, Url = "/test/3" },
                new PaginationLink { Active = true, DisplayText = "4", PageIndex = 4, Url = "/test/4" },
                new PaginationLink { Active = true, DisplayText = "5", PageIndex = 5, Url = "/test/5" },
                new PaginationLink { Active = false, DisplayText = "...", Url = null, IsSpacer = true },
                new PaginationLink { Active = true, DisplayText = "»", PageIndex = 2, Url = "/test/2" }
            };

            // Act
            var result = pager.BuildPaginationModel(BuildUrl);

            // Assert
            Assert.AreEqual(expectedPagination.Count, result.PaginationLinks.Count());
            CollectionAssert.AreEqual(expectedPagination, result.PaginationLinks, new PaginationComparer());
        }

        [Test]
        public void Can_Create_PagedList_From_List_With_Partial_Data()
        {
            // Assemble
            var allItems = new List<int>();
            for (int i = 1; i <= 97; i++) allItems.Add(i);

            int pageSize = 10, currentPageIndex = 5;
            var expectedList = allItems.ToPagedList(currentPageIndex, pageSize);
            int start = currentPageIndex * pageSize + 1;
            int end = start + pageSize - 1;

            // Act 1: when the List contains exactly one-page-number of items
            // items 51-60
            var currentPageItems = allItems.Where(el => (el >= start) && (el <= end)).Select(el => el);
            var pagedList = currentPageItems.ToPagedList(currentPageIndex, pageSize, allItems.Count());

            // Assert 1
            Assert.AreEqual(10, currentPageItems.Count());
            Assert.AreEqual(10, pagedList.Count());
            Assert.AreEqual(51, pagedList.ItemStart);
            Assert.AreEqual(expectedList, pagedList);
            CollectionAssert.AreEqual(expectedList, pagedList);

            // Act 2: when the List contains more than one-page-number of items
            // items 51-68
            end = start + pageSize + 7;
            currentPageItems = allItems.Where(el => (el >= start) && (el <= end)).Select(el => el);
            pagedList = currentPageItems.ToPagedList(currentPageIndex, pageSize, allItems.Count());

            // Assert 2
            Assert.AreEqual(18, currentPageItems.Count());
            Assert.AreEqual(10, pagedList.Count());
            Assert.AreEqual(expectedList, pagedList);
            CollectionAssert.AreEqual(expectedList, pagedList);

            // Act 3: when the List contains less than one-page-number of items
            // items 91-97
            currentPageIndex = 9;
            expectedList = allItems.ToPagedList(currentPageIndex, pageSize);
            start = currentPageIndex * pageSize + 1;
            end = start + 6;
            currentPageItems = allItems.Where(el => (el >= start) && (el <= end)).Select(el => el);
            pagedList = currentPageItems.ToPagedList(currentPageIndex, pageSize, allItems.Count());

            // Assert 3
            Assert.AreEqual(7, currentPageItems.Count());
            Assert.AreEqual(7, pagedList.Count());
            Assert.AreEqual(97, pagedList.Last());
            Assert.AreEqual(expectedList, pagedList);
            CollectionAssert.AreEqual(expectedList, pagedList);

            // Act 4: when the List contains no items
            currentPageItems = new List<int>();
            pagedList = currentPageItems.ToPagedList(currentPageIndex, pageSize, allItems.Count());

            // Assert 4
            Assert.AreEqual(0, pagedList.Count());
        }

        private string BuildUrl(int pageNumber)
        {
            return string.Format("/test/{0}", pageNumber);
        }

        private HtmlHelper<TModel> GetHtmlHelperWithModel<TModel>(TModel model)
        {
            var viewData = new ViewDataDictionary<TModel>(model);

            var dataContainerMock = new Mock<IViewDataContainer>();
            dataContainerMock.Setup(m => m.ViewData).Returns(viewData);

            var viewContext = new ViewContext { ViewData = viewData };
            var htmlHelper = new HtmlHelper<TModel>(viewContext, dataContainerMock.Object);

            return htmlHelper;
        }
    }
}
