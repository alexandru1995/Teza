import { Service } from './service.model';

export class FullService extends Service{
    logoutUrl : string;
    tokenExpirationTime: string;
    servicePassword: string;
}