import { RoleEnum } from '../enums/role.enum';

export interface AccountIdentityDto {
  role: RoleEnum
  firstName: string
  lastName: string
  fullName: string
  id: number
}
