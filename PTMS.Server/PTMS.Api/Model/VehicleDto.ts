


    export interface VehicleModel {
        
        id: number
        plateNumber: string
        routeId: number
        transporterId: number
        vehicleTypeId: number
        transporter: TransporterModel
        route: RouteModel
        vehicleType: VehicleTypeModel
    }