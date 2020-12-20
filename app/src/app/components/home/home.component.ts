import { Component, OnInit } from '@angular/core';
import { Allergy } from 'src/app/shared/models/allergy.model';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { AllergyService } from 'src/app/shared/services/allergy.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  login: boolean = true;
  substitutes: string[];
  allergiesForUser: Allergy[] = [];
  places: string[];
 lastRecipes:Recipe[]
  constructor(private allergiesService: AllergyService) {
    //how can i make it like a carousel? that it would repeat the three options?
    this.places = ['fadeInLeft', 'fadeInUp', 'fadeInRight'];
  }

  ngOnInit(): void {
 this.lastRecipes=JSON.parse(localStorage.getItem('last-search'))
    this.allergiesService.getCurrentUserAllergies().subscribe(
      res => {
        this.allergiesForUser = res;
      });

    if (localStorage.getItem('currentUser') != null) {
      this.login == true;
      this.allergiesService.getSubstitutes().subscribe(
        res => {
          //res can be null if there is no sensitivities
          this.substitutes = res;
        });
    }
    else
      this.login == false;
  }
}
