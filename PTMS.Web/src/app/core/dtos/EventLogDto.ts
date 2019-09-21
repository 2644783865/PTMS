import { UserLightDto } from './UserLightDto';
import { EventLogFieldDto } from './EventLogFieldDto';

export interface EventLogDto
    {
        id: number;
        user: UserLightDto;
        timeStamp: Date | string;
        event: string;
        eventName: string;
        entityType: string;
        entityId: number;
        message: string;
        eventLogFields: EventLogFieldDto[];
    }