import { UserStatusDto } from './UserStatusDto';

export interface UserLightDto
    {
        id: number;
        firstName: string;
        lastName: string;
        middleName: string;

        fullName: string;

        longName: string;

        description: string;

        roleId: number;

        email: string;

        phoneNumber: string;

        projectId: number | null;

        status: UserStatusDto;
    }