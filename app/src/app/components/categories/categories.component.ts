import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, FormControl, ValidatorFn, Validators } from '@angular/forms';
import { Allergy } from 'src/app/shared/models/allergy.model';
import { Category } from 'src/app/shared/models/category.model';
import { AllergyService } from 'src/app/shared/services/allergy.service';
import { CategoryService } from 'src/app/shared/services/category.service';

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
  allergies: Allergy[] = []
  selectedAllergies: number[] = []
  searchText: string;

  SearchForm: any;
  color: any;

  constructor(private categoryService: CategoryService,
    private allergiesService: AllergyService,
    private formBuilder: FormBuilder,
    private router: Router
  ) { }


  submit() {
  }

  ngOnInit(): void {
    this.color = "black";
    this.SearchForm = this.formBuilder.group({
      'search': ['', Validators.required]
    });



    this.categoryService.getAllCategories().subscribe(
      res => {
        this.categories = res;
        this.selectSubCategories(null);
      }
    );

    this.allergiesService.getCurrentUserAllergies().subscribe(
      res => {
        this.allergies = res;
      }
    )
  }
  //saves the selected value of the first combobox
  selectChangeHandler(event: any, index: number) {
    this.selected = event.target.value;

    let len = this.categoriesToSelect.length
    if (index < len - 1) {
      this.categoriesToSelect.splice(index + 1, len - (index + 1));
    }
    this.selectSubCategories(event.target.value)
  }

  googleSearch() {
    //, this.checked
    //this.router.navigate(['recipes', this.selected]);

    this.searchText = this.SearchForm.value.search;
    if (this.searchText == "") {
      localStorage.setItem('search-line', this.selected)
      this.router.navigate(['recipes', this.selected, JSON.stringify(this.selectedAllergies)]);
    }
    else
      this.router.navigate(['recipes', this.searchText, JSON.stringify(this.selectedAllergies)]);

  }

  // googleSearchByText() {
  //   this.searchText = this.SearchForm.value.search;
  //   this.router.navigate(['recipes', this.searchText, JSON.stringify(this.selectedAllergies)]);

  // }

  selectSubCategories(id: number) {
    let sub = this.categories.filter(c => c.MasterCategoryId == id);
    if (sub.length > 0)
      this.categoriesToSelect.push(sub);
  }

  TreatSensitivity() {
    if (this.checked == false) {
      this.checked = true;
    }
    else
      this.checked = false;
  }

  toggleAllergy(AllergyCode: number) {
    let i = this.selectedAllergies.findIndex(a => a == AllergyCode);
    if (i == -1)
      this.selectedAllergies.push(AllergyCode);
    else
      this.selectedAllergies.splice(i, 1);
  }
}




