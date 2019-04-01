import { RouteDto } from './RouteDto';
import { TransporterDto } from './TransporterDto';
import { VehicleTypeDto } from './VehicleTypeDto';

export interface VehicleDto {
  id: number
  plateNumber: string
  routeId: number
  transporterId: number
  vehicleTypeId: number
  transporter: TransporterDto
  route: RouteDto
  vehicleType: VehicleTypeDto
}
