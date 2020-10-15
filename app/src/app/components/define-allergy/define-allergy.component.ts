import { Component, OnInit } from '@angular/core';
import { Allergy } from 'src/app/shared/models/allergy.model';
import { AllergyService } from 'src/app/shared/services/allergy.service';

@Component({
  selector: 'app-define-allergy',
  templateUrl: './define-allergy.component.html',
  styleUrls: ['./define-allergy.component.css']
})
export class DefineAllergyComponent implements OnInit {

allergies : Allergy[] = []

  constructor(private allergiesService: AllergyService) { }

  ngOnInit(): void {
    this.allergiesService.getAllAllergies().subscribe(
      res => {this.allergies = res;
    // this.selectSubCategories(null);
      }
    )
  }

}
