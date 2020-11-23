import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Allergy } from 'src/app/shared/models/allergy.model';
import { AllergyService } from 'src/app/shared/services/allergy.service';

@Component({
  selector: 'app-define-allergy',
  templateUrl: './define-allergy.component.html',
  styleUrls: ['./define-allergy.component.css']
})
export class DefineAllergyComponent implements OnInit {

  allergies: Allergy[] = [];
  allergiesForUser: Allergy[] = [];
  selectedAllergies: number[] = [];

  constructor(private allergiesService: AllergyService, private router: Router) { }

  ngOnInit(): void {
    this.allergiesService.getAllAllergies().subscribe(
      res => {
        this.allergies = res;
        // this.selectSubCategories(null);
      }
    );
    if (localStorage.getItem('currentUser') != undefined) {
      this.allergiesService.getCurrentUserAllergies().subscribe(
        res => {
          this.allergiesForUser = res;
          this.selectedAllergies=Array.from(this.allergiesForUser,a=>a.AllergyCode)
          console.log(Array.from(this.allergiesForUser,a=>a.AllergyCode))
        }
      );
    }
  }


  defineAllergies() {
    this.allergiesService.defineAllergiesForUser(this.selectedAllergies).subscribe(res => console.log(res));
    //הוא לא מוציא כאן את הקודמים שהוא כבר הסיר אותם מהרשימה, צריך לטפל בזה!!!
    this.router.navigate(['/home'])
  }

  toggleAllergy(AllergyCode: number) {
    let i = this.selectedAllergies.findIndex(a => a == AllergyCode);
    if (i == -1)
      this.selectedAllergies.push(AllergyCode);
    else
      this.selectedAllergies.splice(i, 1);
  }

  checkLocalStorage() {
    // let user = localStorage.getItem('currentUser');
    // if (user != undefined)
    //   return user;
    // else
    //   return alert("if you want to define your allergies, you must login!");
    //   //we should do here something nicer than an alert!!
  }

  isChecked(allergy: number) {
    // this.allergiesForUser.forEach(a => {
      
    //   if (a.AllergyCode == allergy)
    //     return true;
    // });
    // return false;
   if(this.allergiesForUser.findIndex(a=>a.AllergyCode==allergy)==-1)
   return false;
   return true;
  }
  skip(){
    this.router.navigate(['/home'])
  }

}
