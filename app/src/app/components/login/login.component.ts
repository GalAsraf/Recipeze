import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/shared/models/user.model';
import { LoginService } from 'src/app/shared/services/login.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  user: User = new User();
  loginForm: any
  currenttype: string;
  currentStatus: string;
  loginAlert:boolean;
  // field-icon
  eye: string = "fa fa-fw fa-eye field-icon toggle-password";
  slash: string = "fa fa-fw fa-eye-slash field-icon toggle-password";
  // eyes: boolean;
  // icon: string;

  constructor(private userService: UserService, private formBuilder: FormBuilder,
     private loginService: LoginService, private router: Router) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      'email': ['', Validators.required],
      'password': ['', [Validators.required]]
    });
    this.currenttype = "fa fa-fw fa-eye field-icon toggle-password";
    this.currentStatus = "password";
  }

  getUserExist() {
    debugger
    this.user.Email = this.loginForm.controls.email.value
    this.user.Password = this.loginForm.controls.password.value
    this.userService.getUserExist(this.user).subscribe(
      res => { localStorage.setItem('currentUser', res.toString()) },
      err => { alert("user not found");
    this.loginAlert==true; }
      //console.error(err)
    )
    this.loginUser(this.user);
  }

  loginUser(user) {
    //here it's need to get the id of the current user from the services, and ot use it throw the app
    const currentUser = user;
    this.loginService.setCurrentUser(currentUser);
    console.log(currentUser);
    this.router.navigate(['/home'])
  }

  toggle() {
    if (this.currentStatus == "text") {
      this.currenttype = this.eye;
      this.currentStatus = "password"
    }
    else {
      this.currenttype = this.slash;
      this.currentStatus = "text"
    }

  }



}
