using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
{
    public enum SortingOption
    {
        [Display(Name = "Relevance")]
        Relevance,
        [Display(Name = "Popularity")]
        Popularity,
        [Display(Name = "Price--Low To High")]
        Price_Low_To_High,
        [Display(Name = "Price--High To Low")]
        Price_High_To_Low,
        [Display(Name = "Newest First")]
        Newest_First,
    }
}