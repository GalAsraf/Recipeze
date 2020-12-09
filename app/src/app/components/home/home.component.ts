import { Component, OnInit } from '@angular/core';
import { AllergyService } from 'src/app/shared/services/allergy.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  login: boolean = true;
  allergies: string[];
  places : string[];

  constructor(private allergiesService: AllergyService) {
    this.places = ['fadeInLeft', 'fadeInUp', 'fadeInRight'];
   }

  ngOnInit(): void {
    if (localStorage.getItem('currentUser') != null) {
      this.login == true;
      this.allergiesService.getSubstitutes().subscribe(
        res => {
          //res can be null if there is no sensitivities
          this.allergies = res;
        });
    }
    else
      this.login == false;
  }
}
