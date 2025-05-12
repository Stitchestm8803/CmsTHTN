export interface UserModel {
    id: string;
    email: string;
    lastName: string;  
    roles: string[];
    permissions: any;
}