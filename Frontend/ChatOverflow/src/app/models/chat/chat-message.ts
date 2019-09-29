import { IUserDetails } from 'src/app/models/user/user-details';
export interface IChatMessage {
    id: string;
    message: string;
    createdAt: Date;
    createdBy: IUserDetails;
}