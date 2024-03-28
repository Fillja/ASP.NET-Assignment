using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.Contact;
using Infrastructure.Repositories;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Services;

public class ContactService(ContactFactory contactFactory, ContactRepository contactRepository)
{
    private readonly ContactRepository _contactRepository = contactRepository;
    private readonly ContactFactory _contactFactory = contactFactory;

    public async Task<ResponseResult> CreateContactAsync(ContactModel model)
    {
        try
        {
            var contactEntity = _contactFactory.PopulateContactEntity(model);
            
            if(contactEntity != null)
            {
                var createResult = await _contactRepository.CreateAsync(contactEntity);

                if (createResult.StatusCode == StatusCode.OK)
                    return ResponseFactory.Ok();
            }

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> ApiCallCreateContactRequestAsync(ContactModel model)
    {
        try
        {
            using var http = new HttpClient();
            var json = JsonConvert.SerializeObject(model);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await http.PostAsync($"https://localhost:7130/api/contact/?key=MWVhMGJjZjgtZGZhMC00ZjA4LWJiMjctZDQ2NWU0YjQxZWQ5", content);

            if(response.IsSuccessStatusCode)
                return ResponseFactory.Ok("Contact request sent. Thank you for contacting Silicon.");

            return ResponseFactory.Error("Something went wrong with sending the message, please try again.");

        }
        catch (Exception ex)
        { 
            return ResponseFactory.Error(ex.Message);
        }
    }
}
