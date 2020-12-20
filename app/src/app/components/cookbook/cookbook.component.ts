import { Component, OnInit } from '@angular/core';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { RecipeService } from 'src/app/shared/services/recipe.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { CurrentRecipeComponent } from '../current-recipe/current-recipe.component';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';



@Component({
  selector: 'app-cookbook',
  templateUrl: './cookbook.component.html',
  styleUrls: ['./cookbook.component.css']
})
export class CookbookComponent implements OnInit {
  cookbookList: Recipe[] = [];
  cookbookToShow: Recipe[] = [];
  title = "angular-text-search-hightlight";
  searchText = '';
  currentRecipe: Recipe;
  closeResult: string;

  // characters = [
  //   'chocolate chip cookies',
  //   'tuna pasta salad',
  //   'sweet potato fries',
  //   'avocado salad',
  //   'vanilla cake',
  //   'chocolate mousse'
  // ];
  // cookbookToShow: string[] = [];

  constructor(  private modalService: NgbModal, private route: ActivatedRoute, private recipeService: RecipeService,
     private router: Router) { }
// public dialogService: DialogService

  ngOnInit(): void {
   this.loadRecipes();

  }

  loadRecipes()
  {
    debugger
    this.recipeService.getUserCookbook().subscribe(
      res => {
        this.cookbookList = res;
        console.log(res)
        this.showRecipeList();
      },
      err => { console.error(err) }
    );

  }

  showRecipeList() {
    this.cookbookToShow = this.cookbookList;
    // this.cookbookList.forEach(r => {
    //   this.cookbookToShow.push(r.RecipeName)
    // });
  }


  removeRecipeFromCookbook(recipe: Recipe) {
    this.recipeService.deleteRecipeFromCookbook(recipe).subscribe(
      res =>{ console.log(res);
        //this.loadRecipes();
        this.aa();
      }
      );
  }

  aa()
  {
    alert("yy")
  }


  showRecipe(recipe:Recipe){
    this.router.navigate(['current-recipe',JSON.stringify(recipe)]);

    
  //   const ref = this.dialogService.open(CurrentRecipeComponent, {
  //     data: {currentRecipe:recipe},
  //     header: recipe.RecipeName,
  //     width: '70%'
  // });
  }


  open(content, recipe) {
    this.currentRecipe = recipe;
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  deleteRecipeFromCookbook(recipe: Recipe) {
    this.recipeService.deleteRecipeFromCookbook(recipe).subscribe(
      res => console.log(res));
  }

}



