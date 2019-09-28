import { ObjectDto } from './ObjectDto';
import { RouteDto } from './RouteDto';

export interface TrolleybusTodayStatusDto
{
    id: number;
    trolleybus: ObjectDto;
    place: string;
    newRoute: RouteDto;
    coordinationTime: Date | string | null;
    isNotDefined: boolean;
}
