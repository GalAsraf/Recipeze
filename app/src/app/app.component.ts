import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from './shared/services/login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Recipeze';
  isLogin: any;
  constructor(private loginService: LoginService, private router:Router) {
    this.isLogin = loginService.CurrnetUser
  }

  userLogedIn() {
    

    let user= localStorage.getItem('currentUser') != null;
    return user;

  }

  logOut() {
    localStorage.clear();
    this.router.navigate(['/home'])
  }
}
