import { ProjectDto } from './ProjectDto';
import { BusStationRouteDto } from './BusStationRouteDto';

export interface RouteFullDto {
    id: number
    name: string
    routeActive: boolean
    projectId: number | null
    project: ProjectDto
    busStationRoutes: BusStationRouteDto[]
}
