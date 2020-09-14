import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/shared/models/user.model';
import { UserService } from 'src/app/shared/services/user.service';
import { FormBuilder, Validators, FormControl, FormGroup } from '@angular/forms'

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
  //,providers:[UserService]
})
export class AddUserComponent implements OnInit {
  user: User = new User();
  userForm: any


  constructor(private userService: UserService, private formBuilder: FormBuilder) { }




  ngOnInit(): void {

    this.userForm = this.formBuilder.group({
      'name': ['', Validators.required],
      'email': ['', [Validators.required]],
      'password': ['', [Validators.required]],
      'confirmPassword': ['', [Validators.required]],
    })


    /* this.user.UserName = "ss";
    this.user.Password = "dd";
 */

    //this.addUser();
    this.userService.getUserExist().subscribe(
      res => { console.log('is exist' + res) },
      err => { console.error(err) }
    )
  }

  addUser() {

    this.user.UserName = this.userForm.value.name
    this.user.Password = this.userForm.value.password
    this.user.Email = this.userForm.value.email
    this.userService.addUser(this.user).subscribe(
      res => { console.log(res) },
      err => { console.error(err) }

    )
  }
}
