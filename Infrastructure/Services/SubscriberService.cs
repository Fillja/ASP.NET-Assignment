using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.Home;
using Infrastructure.Repositories;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Infrastructure.Services;

public class SubscriberService(SubscriberRepository subscriberRepository, SubscriberFactory subscriberFactory)
{
    private readonly SubscriberRepository _subscriberRepository = subscriberRepository;
    private readonly SubscriberFactory _subscriberFactory = subscriberFactory; 

    public async Task<ResponseResult> CreateSubscriberAsync(NewsLetterModel model)
    {
        try
        {
            var existResult = await _subscriberRepository.ExistsAsync(x => x.Email == model.Email);

            if (existResult.StatusCode == StatusCode.NOT_FOUND)
            {
                var subscriberEntity = _subscriberFactory.PopulateSubscriberEntity(model);

                if(subscriberEntity != null)
                {
                    var createResult = await _subscriberRepository.CreateAsync(subscriberEntity);
                    if (createResult.StatusCode == StatusCode.OK)
                        return ResponseFactory.Ok("Subscriber created successfully.");
                }
            }
            else if (existResult.StatusCode == StatusCode.EXISTS)
            {
                return ResponseFactory.Exists("A subscriber with this email already exists.");
            }

            return ResponseFactory.Error("Something went wrong with creating the subscription.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> ApiCallCreateSubscriberAsync(NewsLetterModel model)
    {
        try
        {
            using var http = new HttpClient();
            var json = JsonConvert.SerializeObject(model);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await http.PostAsync($"https://localhost:7130/api/subscribers/?key=MWVhMGJjZjgtZGZhMC00ZjA4LWJiMjctZDQ2NWU0YjQxZWQ5", content);

            if (response.IsSuccessStatusCode)
                return ResponseFactory.Ok("You have been subscribed successfully.");

            else if (response.StatusCode == HttpStatusCode.Conflict)
                return ResponseFactory.Exists("This email is already subscribed.");

            return ResponseFactory.Error("Something went wrong with subscribing.");
        }
        catch(Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }
}
