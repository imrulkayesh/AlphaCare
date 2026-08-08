using RetailCare.Models.ServiceModel;

namespace RetailCare.Interface.ServiceInterface
{
    public interface IFeedbackRepository
    {
        public bool AddNewFeedback(FeedabackModel Feedback);
    }
}
