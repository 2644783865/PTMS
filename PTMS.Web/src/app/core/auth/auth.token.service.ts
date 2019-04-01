import { Injectable } from '@angular/core';

@Injectable()
export class AuthTokenService {
  private authToken: string;

  getToken(): string {
    return this.authToken || window.localStorage.getItem("authToken");
  }

  setToken(token: string): void {
    this.authToken = token;
    window.localStorage.setItem("authToken", token);
  }
}
