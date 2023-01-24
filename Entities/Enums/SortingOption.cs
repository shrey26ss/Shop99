using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
{
    public enum SortingOption
    {
        [Display(Name = "Sorting items")]
        Sortingitems = 0,
        [Display(Name = "Relevance")]
        Relevance = 1,
        [Display(Name = "Popularity")]
        Popularity = 2,
        [Display(Name = "Price--Low To High")]
        Price_Low_To_High = 3,
        [Display(Name = "Price--High To Low")]
        Price_High_To_Low = 4,
        [Display(Name = "Newest First")]
        Newest_First = 5,
    }
}