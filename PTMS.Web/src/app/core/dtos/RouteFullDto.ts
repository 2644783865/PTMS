import { ProjectDto } from './ProjectDto';


export interface RouteFullDto {
    id: number
    name: string
    routeActive: boolean
    project: ProjectDto
}
