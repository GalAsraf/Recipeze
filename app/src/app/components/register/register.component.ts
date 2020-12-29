import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/shared/models/user.model';
import { LoginService } from 'src/app/shared/services/login.service';
import { UserService } from 'src/app/shared/services/user.service';
import { CheckPassword } from 'src/app/shared/validators/valid';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  user: User = new User();
  userForm: any;
  currenttype1: string;
  currentStatus1: string;
  currenttype2: string;
  currentStatus2: string;
  eye: string = "fa fa-fw fa-eye field-icon toggle-password";
  slash: string = "fa fa-fw fa-eye-slash field-icon toggle-password";

  constructor(private userService: UserService, private formBuilder: FormBuilder, private loginService: LoginService, private router: Router) { }

  ngOnInit(): void {
    this.userForm = this.formBuilder.group({
      'name': ['', Validators.required],
      'email': ['', [Validators.required]],
      'password': ['', [Validators.required]],
      'confirmPassword': ['', [Validators.required]],
      'accept': ['', [Validators.requiredTrue]]
    }
    //  { validators: CheckPassword('password', 'confirm') });

  ,);

    this.currenttype1 = "fa fa-fw fa-eye field-icon toggle-password";
    this.currentStatus1 = "password";
    this.currenttype2 = "fa fa-fw fa-eye field-icon toggle-password";
    this.currentStatus2 = "password";
  }

  addUser() {

    this.user.UserName = this.userForm.value.name
    this.user.Password = this.userForm.value.password
    this.user.Email = this.userForm.value.email
    this.userService.getUserExist(this.user).subscribe(
      res => {
        if (res == -1)
          alert("user allready exist. try again!");
        else {
          this.userService.addUser(this.user).subscribe(
            res => { localStorage.setItem('currentUser', res.toString()) },
            err => { console.error(err) }
          )
          this.loginUser(this.user);
        }
      }
    );
  }

  loginUser(user) {
    //here it's need to get the id of the current user from the services, and ot use it throw the app
    const currentUser = user;
    this.loginService.setCurrentUser(currentUser);
    console.log(currentUser);

    this.router.navigate(['/allergies'])

  }

  toggle1() {
    if (this.currentStatus1 == "text") {
      this.currenttype1 = this.eye;
      this.currentStatus1 = "password"
    }
    else {
      this.currenttype1 = this.slash;
      this.currentStatus1 = "text"
    }
  }


  toggle2() {
    if (this.currentStatus2 == "text") {
      this.currenttype2 = this.eye;
      this.currentStatus2 = "password"
    }
    else {
      this.currenttype2 = this.slash;
      this.currentStatus2 = "text"
    }
  }


}
