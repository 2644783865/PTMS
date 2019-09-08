export interface EventLogFieldDto
    {
        id: number;
        eventLogId: number;
        fieldName: string;
        oldFieldValue: string;
        newFieldValue: string;
    }