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
  selected:string;
  // selectedFirstValue: Category
  // selectedSecondValue: Category
  // selectedThirdValue: Category

  selectedCategories: Category[]=[]
  categoriesToSelect:Category[][]=[];


  


  constructor(private categoryService: CategoryService, private allergiesService: AllergyService, private formBuilder: FormBuilder) { }


  submit() {
  }

  ngOnInit(): void {
    

    this.categoryService.getAllCategories().subscribe(
      res => {this.categories = res;
     this.selectSubCategories(null);
      }
    )
  }
  //saves the selected value of the first combobox
  selectChangeHandler(event: any,index:number) {
    this.selected =event.target.value;
    this.selectSubCategories(event.target.value)
    // this.selectedFirstValue = this.categories.filter(m => m.MasterCategoryId == event.target.value);
  }
  // secondSelect(event: any) {
  //  this.selectedSecondValue = event.target.value;
  // }
  // thirdSelected(event: any) {
  //   this.selectedThirdValue = event.target.value;
   
  // }
  // filterSubById(id) {
  //   return this.subCategories.filter(item => item.parentId === id);
  // }

  googleSearch(){

    this.categoryService.googleSearch(this.selected).subscribe(
      res => { console.log(res) },
      err => { console.error(err) }
    )

    // this.selectedCategories[0]=this.selectedFirstValue
    // this.selectedCategories[1]=this.selectedSecondValue
    // this.selectedCategories[2]=this.selectedThirdValue

    // this.categoryService.googleSearch(this.selectedCategories).subscribe(
    //   res => { console.log(res) },
    //   err => { console.error(err) }
    // )
  }

  selectSubCategories(id:number)
  {
    let sub=this.categories.filter(c=>c.MasterCategoryId==id);
    if(sub.length>0)
    this.categoriesToSelect.push(sub);
  }
}




