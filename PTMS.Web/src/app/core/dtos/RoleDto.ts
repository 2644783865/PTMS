import { RoleEnum } from '../enums/role.enum';

export interface RoleDto {
    id: number
    name: RoleEnum
    displayName: string
}
