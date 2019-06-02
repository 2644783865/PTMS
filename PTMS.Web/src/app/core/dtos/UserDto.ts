import { RoleDto } from './RoleDto';
import { UserStatusDto } from './UserStatusDto';
import { ProjectDto } from './ProjectDto';

export interface UserDto {
  id: number
  firstName: string
  lastName: string
  middleName: string
  fullName: string
  longName: string
  description: string
  role: RoleDto
  email: string
  phoneNumber: string
  status: UserStatusDto
  project: ProjectDto
}
