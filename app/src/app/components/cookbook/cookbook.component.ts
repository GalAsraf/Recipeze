import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-cookbook',
  templateUrl: './cookbook.component.html',
  styleUrls: ['./cookbook.component.css']
})
export class CookbookComponent implements OnInit {
  cookbookList: string[] = []
  constructor(private route: ActivatedRoute, private userService: UserService) { }

  ngOnInit(): void {


    // this.userService.cookbook().subscribe(
    //   res => {
    //     this.cookbookList = res;
    //     console.log(res)
    //   },
    //   err => { console.error(err) }
    // );
  }
  


}



