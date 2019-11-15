import { Contact } from './contact.model';

export class User{
    Id: string;
    FirstName: string;
    LastName: string;
    UserName: string;
    Contacts: Contact[];
    Birthday: string;
    Password: string;
    Gender: boolean = true;
}