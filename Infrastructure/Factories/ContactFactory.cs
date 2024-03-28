using Infrastructure.Entities;
using Infrastructure.Models.Contact;

namespace Infrastructure.Factories;

public class ContactFactory
{
    public ContactEntity PopulateContactEntity(ContactModel model)
    {
        var contactEntity = new ContactEntity();

        if(model != null)
        {
            contactEntity.FullName = model.FullName;
            contactEntity.Email = model.Email;
            contactEntity.Service = model.Service;
            contactEntity.Message = model.Message;
        }

        return contactEntity;
    }
}
