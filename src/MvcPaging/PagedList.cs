using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcPaging
{
	public class PagedList<T> : List<T>, IPagedList<T>
	{
		public PagedList(IEnumerable<T> source, int index, int pageSize, int? totalCount = null)
			: this(source.AsQueryable(), index, pageSize, totalCount)
		{
		}

		public PagedList(IQueryable<T> source, int index, int pageSize, int? totalCount = null)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException("index", "Value can not be below 0.");
			if (pageSize < 1)
				throw new ArgumentOutOfRangeException("pageSize", "Value can not be less than 1.");

			if (source == null)
				source = new List<T>().AsQueryable();

			var realTotalCount = source.Count();

			PageSize = pageSize;
			PageIndex = index;
			TotalItemCount = totalCount.HasValue ? totalCount.Value : realTotalCount;
			PageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;

			HasPreviousPage = (PageIndex > 0);
			HasNextPage = (PageIndex < (PageCount - 1));
			IsFirstPage = (PageIndex <= 0);
			IsLastPage = (PageIndex >= (PageCount - 1));

			ItemStart = PageIndex*PageSize + 1;
			ItemEnd = Math.Min(PageIndex*PageSize + PageSize, TotalItemCount);

			if (TotalItemCount <= 0)
				return;

			var realTotalPages = (int)Math.Ceiling(realTotalCount / (double)PageSize);

			if (realTotalCount < TotalItemCount && realTotalPages <= PageIndex)
				AddRange(source.Skip((realTotalPages - 1) * PageSize).Take(PageSize));
			else
				AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
		}

		#region IPagedList Members

		public int PageCount { get; private set; }
		public int TotalItemCount { get; private set; }
		public int PageIndex { get; private set; }
		public int PageNumber { get { return PageIndex + 1; } }
		public int PageSize { get; private set; }
		public bool HasPreviousPage { get; private set; }
		public bool HasNextPage { get; private set; }
		public bool IsFirstPage { get; private set; }
		public bool IsLastPage { get; private set; }
		public int ItemStart { get; private set; }
		public int ItemEnd { get; private set; }

		#endregion
	}
}