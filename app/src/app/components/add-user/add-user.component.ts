import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/shared/models/user.model';
import { UserService } from 'src/app/shared/services/user.service';
import {FormControl,FormGroup} from '@angular/forms'

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  user: User = new User();
  myform:FormGroup
  
  
  constructor(private userService: UserService) { }

  ngOnInit(): void {
     
     this.myform=new FormGroup({
      username:new FormControl(''),
      password:new FormControl('')
    }) 



    this.user.UserName = "ss";
    this.user.Password = "dd";
    

    this.addUser();
    this.userService.getUserExist().subscribe(
      res => { console.log('is exist' + res) },
      err => { console.error(err) }
    )
  }

  addUser() {
    this.userService.addUser(this.user).subscribe(
      res => { console.log(res) },
      err => { console.error(err) }

    )
  }
}
