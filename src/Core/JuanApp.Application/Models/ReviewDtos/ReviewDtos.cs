using JuanApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace JuanApp.Application.Models.ReviewDtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public int Rate { get; set; }
        public ReviewStatus Status { get; set; }
        public int ProductId { get; set; }
        public string AppUserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string UserFullName { get; set; } = "";
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
    }

    public class CreateReviewDto
    {
        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Review content must be between 10 and 500 characters.")]
        public string Content { get; set; } = "";

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rate { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string AppUserId { get; set; } = "";
    }

    public class UpdateReviewDto
    {
        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Review content must be between 10 and 500 characters.")]
        public string Content { get; set; } = "";

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rate { get; set; }
    }

    public class CreateReviewResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public ReviewDto? Review { get; set; }
    }

    public class ReviewStatisticsDto
    {
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public int FiveStarCount { get; set; }
        public int FourStarCount { get; set; }
        public int ThreeStarCount { get; set; }
        public int TwoStarCount { get; set; }
        public int OneStarCount { get; set; }
    }
}