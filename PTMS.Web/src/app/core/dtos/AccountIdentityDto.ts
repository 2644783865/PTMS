import { Role } from '../enums/role';

export interface AccountIdentityDto {
  roles: Role[]
  firstName: string
  lastName: string
  fullName: string
}
