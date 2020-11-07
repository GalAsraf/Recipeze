import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, FormControl, ValidatorFn } from '@angular/forms';
import { Allergy } from 'src/app/shared/models/allergy.model';
import { Category } from 'src/app/shared/models/category.model';
import { AllergyService } from 'src/app/shared/services/allergy.service';
import { CategoryService } from 'src/app/shared/services/category.service';
//import { ListboxModule } from 'primeng/listbox';



@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})


export class CategoriesComponent implements OnInit {
  categories: Category[] = []
  baseCategories: Category[] = []
  form: FormGroup
  selected: string;
  checked: boolean = false;
  selectedCategories: Category[] = []
  categoriesToSelect: Category[][] = [];

  constructor(private categoryService: CategoryService,
    private allergiesService: AllergyService,
    private formBuilder: FormBuilder,
    private router: Router) { }


  submit() {
  }

  ngOnInit(): void {
    this.categoryService.getAllCategories().subscribe(
      res => {
        this.categories = res;
        this.selectSubCategories(null);
      }
    )
  }
  //saves the selected value of the first combobox
  selectChangeHandler(event: any, index: number) {
    this.selected = event.target.value;
    this.selectSubCategories(event.target.value)
  }

  googleSearch() {
    //, this.checked
    this.router.navigate(['recipes', this.selected]);
  }

  selectSubCategories(id: number) {
    let sub = this.categories.filter(c => c.MasterCategoryId == id);
    if (sub.length > 0)
      this.categoriesToSelect.push(sub);
  }

  TreatSensitivity() {
    if (this.checked == false)
      this.checked = true;
    else
      this.checked = false;
  }
}




