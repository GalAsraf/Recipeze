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
  color:any;
  constructor(private loginService: LoginService, private router:Router) {
    this.isLogin = loginService.CurrnetUser
  }

  ngOnInit(): void {
    this.color="white";
  }

  
  userLogedIn() {
    

    let user= localStorage.getItem('currentUser') != null;
    return user;

  }

  logOut() {
    localStorage.clear();
    this.router.navigate(['/home'])
  }

  onActivate(event) {
    window.scroll(0,0);
    //or document.body.scrollTop = 0;
    //or document.querySelector('body').scrollTo(0,0)
  }
}
