import { ObjectDto } from './ObjectDto';
import { RouteDto } from './RouteDto';

export interface TrolleybusTodayStatusDto
{
    trolleybus: ObjectDto;
    place: string;
    newRoute: RouteDto;
    coordinationTime: Date | string | null;
}
