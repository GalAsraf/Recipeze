import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private currentUser = undefined;

  constructor() { }

  get IsLogin() {
    return this.currentUser != undefined;
  }

  get CurrnetUser() {
    return this.currentUser;
  }

  setCurrentUser(user) {
    this.currentUser = user;
  }
} 
