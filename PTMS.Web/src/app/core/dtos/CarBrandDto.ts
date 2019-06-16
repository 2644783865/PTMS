import { CarTypeDto } from './CarTypeDto';

export interface CarBrandDto {
    id: number
    name: string
    carTypeId: number
    l: string
    w: string
    h: string
    carType: CarTypeDto
}
