export interface ICreateGroupChat {
    name: string;
    password?: string;
    createLink: boolean;
    members: string[];
}