using JuanApp.Application.Models.ReviewDtos;

namespace JuanApp.Application.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetApprovedReviewsByProductIdAsync(int productId);
        Task<ReviewDto?> GetByIdAsync(int id);
        Task<bool> HasUserReviewedProductAsync(string userId, int productId);
        Task<CreateReviewResponseDto> CreateReviewAsync(CreateReviewDto createReviewDto);
        Task<bool> UpdateReviewAsync(int id, UpdateReviewDto updateReviewDto);
        Task<bool> DeleteReviewAsync(int id);
        Task<bool> ApproveReviewAsync(int id);
        Task<bool> RejectReviewAsync(int id);
        Task<ReviewStatisticsDto> GetReviewStatisticsAsync(int productId);
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<IEnumerable<ReviewDto>> GetPendingReviewsAsync();
    }
}