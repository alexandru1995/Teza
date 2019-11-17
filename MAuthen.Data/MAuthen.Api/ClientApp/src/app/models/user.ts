import { Contact } from './contact.model';

export class User{
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    contacts: Contact[];
    birthday: string;
    password: string;
    gender: boolean = true;
}