import { Component, OnInit } from '@angular/core';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { RecipeService } from 'src/app/shared/services/recipe.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { CurrentRecipeComponent } from '../current-recipe/current-recipe.component';



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
  characters = [
    'chocolate chip cookies',
    'tuna pasta salad',
    'sweet potato fries',
    'avocado salad',
    'vanilla cake',
    'chocolate mousse'
  ];
  // cookbookToShow: string[] = [];

  constructor(private route: ActivatedRoute, private recipeService: RecipeService,
     private router: Router) { }
// public dialogService: DialogService

  ngOnInit(): void {
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
      res => console.log(res));
  }


  showRecipe(recipe:Recipe){
    this.router.navigate(['current-recipe',JSON.stringify(recipe)]);

    
  //   const ref = this.dialogService.open(CurrentRecipeComponent, {
  //     data: {currentRecipe:recipe},
  //     header: recipe.RecipeName,
  //     width: '70%'
  // });
  }


}



