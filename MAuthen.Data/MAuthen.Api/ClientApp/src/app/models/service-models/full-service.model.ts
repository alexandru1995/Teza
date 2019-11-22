import { Service } from './service.model';

export class FullService extends Service{
    logOutUrl : string;
    tokenExpirationTime: string;
    servicePassword: string;
}