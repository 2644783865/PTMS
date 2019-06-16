import { CarBrandDto } from './CarBrandDto';
import { RouteDto } from './RouteDto';
import { ProjectDto } from './ProjectDto';
import { ProviderDto } from './ProviderDto';

export interface ObjectDto {
  name: string
  objId: number
  lastTime: Date
  lastLon: number
  lastLat: number
  lastSpeed: number
  projId: number
  lastStation: number
  lastStationTime: Date
  lastRout: number
  vehicleType: number
  azmth: number
  providerId: number
  id: number
  carBrandId: number
  userComment: string
  dateInserted: Date
  objOutput: number
  objOutputDate: Date
  phone: number
  yearRelease: number
  dispRoute: number
  lastAddInfo: number
  lowfloor: number
  statusName: string
  carBrand: CarBrandDto
  provider: ProviderDto
  route: RouteDto
  project: ProjectDto
}
