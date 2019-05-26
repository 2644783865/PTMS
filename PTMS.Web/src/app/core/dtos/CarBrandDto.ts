import { CarTypeDto } from './CarTypeDto';

export interface CarBrandDto {
    cbId: number
    cbName: string
    carTypeId: number
    l: string
    w: string
    h: string
    carType: CarTypeDto
}
