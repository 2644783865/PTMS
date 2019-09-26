import { CarBrandDto } from './CarBrandDto';
import { RouteDto } from './RouteDto';
import { ProjectDto } from './ProjectDto';
import { ProviderDto } from './ProviderDto';
import { GranitDto } from './GranitDto';

export interface ObjectDto {
  id: number;
  name: string;
  objId: number;
  lastTime: Date | string | null;
  lastLongitude: number | null;
  lastLatitude: number | null;
  lastSpeed: number | null;
  projectId: number;
  lastStationId: number | null;
  lastStationTime: Date | string | null;
  lastRouteId: number | null;
  carTypeId: number | null;
  azimuth: number | null;
  providerId: number;
  carBrandId: number | null;
  userComment: string;
  dateInserted: Date | string | null;
  objectOutput: boolean;
  objectOutputDate: Date | string | null;
  phone: number;
  yearRelease: number | null;
  dispRouteId: number | null;
  lastAddInfo: number | null;
  lowfloor: boolean;
  statusName: string;

  carBrand: CarBrandDto
  provider: ProviderDto
  route: RouteDto
  project: ProjectDto
  block: GranitDto
}