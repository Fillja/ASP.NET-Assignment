using Infrastructure.Entities;
using Infrastructure.Models.Home;

namespace Infrastructure.Factories;

public class SubscriberFactory
{
    public SubscriberEntity PopulateSubscriberEntity(NewsLetterModel model)
    {
        var subscriberEntity = new SubscriberEntity();

        if(model != null)
        {
            subscriberEntity.Email = model.Email;
            subscriberEntity.DailyNewsletter = model.DailyNewsletter;
            subscriberEntity.EventUpdates = model.EventUpdates;
            subscriberEntity.Podcasts = model.Podcasts;
            subscriberEntity.AdvertisingUpdates = model.AdvertisingUpdates;
            subscriberEntity.WeekInReview = model.WeekInReview;
            subscriberEntity.StartupsWeekly = model.StartupsWeekly;
        }

        return subscriberEntity;    
    }
}
