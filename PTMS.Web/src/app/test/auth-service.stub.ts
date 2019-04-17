import { BehaviorSubject, of } from "rxjs";
import { Role } from '@app/core/enums/role';
import { AuthService } from '@app/core/auth/auth.service';

const authStubService = {
  state: {
    token: 'test-token',
    identity: {
      roles: [Role.Administrator]
    }
  },
  
  isAuthInProcess$: new BehaviorSubject(false),

  getToken(): string {
    return this.state.token;
  },

  get isLoading$() {
    return this.isAuthInProcess$.asObservable();
  },

  get identity$() {
    return of(this.state.identity);
  },

  login() {
    
  },

  getState() {
    return of(this.state);
  },

  logout() {
  },

  isInRole(role: Role) {
    return this.state.identity.roles.includes(role);
  }
}

export const authStubProvider = {
  provide: AuthService,
  useValue: authStubService
}
